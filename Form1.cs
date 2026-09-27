using DxLibDLL;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DxLibCSTest
{
    public partial class Form1 : Form
    {
        private bool isRunning = false;
        int[] nSFSound, nSound;
        string[] sFilenames = new string[nSOUND_QUANTITY];
        long total_samples = 0; // 読み込んだサンプルの数
        long currentSamplePos = 0; /// 現在の位置（サンプル単位）
        int channels = 0, bitsPerSample = 0, samplesPerSec = 0; // 2ch, 16bit, 48000hz/44100hz
        const int nSOUND_QUANTITY = 7; // 7個まで

        double dZoomRate = 2.0; // 1.0で1秒分の波形を描画する。2.0で2秒分の波形を描画する。
        const int nWAVE_SIZE = 3000;
        string JSON_FILENAME = @"\mydata.json";
        float[] waveform = new float[nWAVE_SIZE];
        DX.VERTEX2D[] vertices = new DX.VERTEX2D[nWAVE_SIZE];

        int nPushedL = -100, nPushedR = -100;
        int xsize = 1980, ysize = 1300;

        class UserData
        {
            public int nStartBar { get; set; } // 開始小節
            public int nBunshi { get; set; } // 分子
            public int nBunbo { get; set; } // 分母
            public double dBPM { get; set; } // BPM
            public int nMAX_MIN { get; set; } // 1小節の最大値-最小値
            public int nAVE_NUM { get; set; } // 平均個数
            public string sWaveFolder { get; set; } // 音声ファイルのフォルダ
            public string[] sSoundFile { get; set; } // 音声ファイル名
        }
        UserData userData = new UserData();

        private int SampleCount = 0;

        public Form1()
        {
            InitializeComponent();

            // イベントハンドラーの設定
            this.Shown += new EventHandler(Form1_Shown);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private bool WavesLoad()
        {
            if (isRunning)
            {
                DX.ClearDrawScreen();
                DX.ScreenFlip();
            }
            isRunning = false;

            nPushedL = nPushedR = -100; label5.Text = "";
            userData.nBunshi = 0;
            userData.nBunbo = 192;
            userData.nStartBar = 0;
            userData.dBPM = 0;
            textBoxBunshi.Text = userData.nBunshi.ToString();
            textBoxBunbo.Text = userData.nBunbo.ToString();
            textBoxStartBar.Text = userData.nStartBar.ToString();
            textBoxBPM.Text = userData.dBPM.ToString("F2");


            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                sFilenames[i] = "";
                if (nSound[i] != -1)
                {
                    DX.DeleteSoundMem(nSound[i]);
                    DX.DeleteSoundMem(nSFSound[i]);
                }
                nSFSound[i] = -1;
                nSound[i] = -1;
            }
            userData.sSoundFile = null;

            // ファイル選択ダイアログを表示して、音声ファイルを選択する
            // OpenFileDialogを作成
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "音声ファイル (*.wav;*.ogg;*.mp3)|*.wav;*.ogg;*.mp3|すべてのファイル (*.*)|*.*";
            openFileDialog.Multiselect = true; // 複数選択を許可
            openFileDialog.Title = "音声ファイルを選択してください (最大7つ)";
            openFileDialog.InitialDirectory = userData.sWaveFolder; // 初期ディレクトリを設定
            openFileDialog.RestoreDirectory = true; // ダイアログを閉じた後にカレントディレクトリを復元する
            openFileDialog.FileName = ""; // 初期ファイル名を空にする
            openFileDialog.CheckFileExists = true; // 存在するファイルのみ選択可能にする
            openFileDialog.CheckPathExists = true; // 存在するパスのみ選択可能にする
            openFileDialog.ValidateNames = true; // 有効なファイル名のみ選択可能にする

            // ファイルを選択する
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("音声ファイルが選択されませんでした。");
                return false;
            }
            if (openFileDialog.FileNames.Length > nSOUND_QUANTITY)
            {
                MessageBox.Show($"音声ファイルは最大{nSOUND_QUANTITY}つまで選択可能です。");
                return false;
            }

            MessageBox.Show("音声ファイルをロード中します。画面が反応しませんが、少々お待ちください。（MP3形式がある場合、OGG形式の数倍時間がかかります）", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // ソフトウエアで扱う波形データハンドルのサンプル数を取得する。バイト数ではなく、サンプルの数
            // 1サンプルは左右2チャンネルで16bit（2byte）なので、1サンプルは4byteになる
            // サンプル数(total_samples)を44100(samplesPerSec)で割れば、曲の秒数が求められる。12857472 / 44100 = 291.45秒（約4分51秒）
            nSFSound = new int[nSOUND_QUANTITY];
            nSound = new int[nSOUND_QUANTITY];

            userData.sWaveFolder = Path.GetDirectoryName(openFileDialog.FileNames[0]);
            userData.sSoundFile = null;
            if (openFileDialog.FileNames.Length > 0)
            {
                userData.sSoundFile = new string[nSOUND_QUANTITY];
                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    userData.sSoundFile[i] = openFileDialog.FileNames[i];
                }
            }

            return WavesLoad_Sub();
        }

        private bool WavesLoad_Sub()
        {
            isRunning = false;
            long t = 0;
            int sr = -1;
            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                nSFSound[i] = DX.LoadSoftSound(userData.sSoundFile[i]);
                sFilenames[i] = Path.GetFileName(userData.sSoundFile[i]);

                if (nSFSound[i] == -1)
                {
                    MessageBox.Show($"ソフトサウンド{nSFSound[i]}のロードに失敗しました。ファイルが存在するか確認してください。");
                    return false;
                }
                channels = 0; bitsPerSample = 0; samplesPerSec = 0; // 2ch, 16bit, 44100hz/48000hz
                int r = DX.GetSoftSoundFormat(nSFSound[i], out channels, out bitsPerSample, out samplesPerSec, out int isFloatType);
                if (sr == -1)
                {
                    sr = samplesPerSec;
                }
                else
                {
                    if (sr != samplesPerSec)
                    {
                        MessageBox.Show($"サンプルレートが異なる音声ファイルが混在しています。すべての音声ファイルのサンプルレートを統一してください。");
                        return false;
                    }
                }

                if (channels != 2 || bitsPerSample != 16 || (samplesPerSec != 44100 && samplesPerSec != 48000))
                {
                    MessageBox.Show($"{nSFSound[i]} の動画の音声フォーマットが2ch, 16bit, 44100hzまたは48000hzの形式のみ対応しています。");
                    return false;
                }

                total_samples = (int)DX.GetSoftSoundSampleNum(nSFSound[i]);
                if (total_samples > t) t = total_samples;
            }
            total_samples = t;

            currentSamplePos = 0;
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = (int)total_samples + (int)(total_samples / 20) - 1;
            hScrollBar1.LargeChange = (int)(total_samples / 20);
            hScrollBar1.SmallChange = (int)(total_samples / (1000 * 20));
            hScrollBar1.Value = 0;

            // ソフトウエアで扱う波形データハンドルのフォーマットを取得する
            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                nSound[i] = DX.LoadSoundMemFromSoftSound(nSFSound[i]); // ソフトサウンドハンドルから通常のサウンドハンドルを作成
            }
            SampleCount = samplesPerSec * 2; // 2秒分 　　点を打つ個数

            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                DX.StopSoundMem(nSound[i]);
                DX.SetCurrentPositionSoundMem(0, nSound[i]);
            }

            isRunning = true;
            return true;
        }

        // フォームが画面に表示された直後に呼ばれる
        private async void Form1_Shown(object sender, EventArgs e)
        {
            // 1. DXライブラリの初期設定
            DX.ChangeWindowMode(DX.TRUE);      // ウィンドウモード
            DX.SetUserWindow(this.Handle);    // 描画先をこのFormのハンドルに指定(Init以後は変更不可)
            DX.SetBackgroundColor(64, 64, 64);
            DX.SetGraphMode(xsize, ysize, 32);
            DX.SetEnableXAudioFlag(DX.FALSE); // XAudio2を無効にしてDirectSoundにする（再生位置取得とかはDSの方がいいらしい？）
            DX.SetAlwaysRunFlag(DX.TRUE); // ウィンドウが非アクティブでも描画を続ける
            DX.SetWaitVSyncFlag(DX.FALSE);
            DX.SetWindowSizeExtendRate(1.0);
            ClientSize = new Size(xsize, ysize);
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);

            if (DX.DxLib_Init() == -1)
            {
                MessageBox.Show("DXライブラリの初期化に失敗しました。");
                this.Close();
                return;
            }

            // ファイルからJSON文字列を読み込み
            string sFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            try
            {
                string jsonString = File.ReadAllText(sFolder + JSON_FILENAME);

                // JSON文字列をUserクラスのオブジェクトに変換
                UserData? data = JsonSerializer.Deserialize<UserData>(jsonString);
                if (data != null)
                {
                    userData = data;
                }
            }
            catch
            {
                userData = new UserData();
                userData.dBPM = 0;
                userData.nBunbo = 192;
                userData.nBunshi = 0;
                userData.nStartBar = 0;
                userData.nMAX_MIN = 5000;
                userData.nAVE_NUM = 64;
                userData.sWaveFolder = sFolder;
                userData.sSoundFile = null;
            }

            textBoxBPM.Text = userData.dBPM.ToString("F2");
            textBoxBunshi.Text = userData.nBunshi.ToString();
            textBoxBunbo.Text = userData.nBunbo.ToString();
            textBoxStartBar.Text = userData.nStartBar.ToString();
            textBoxMAX_MIN.Text = userData.nMAX_MIN.ToString();
            textBoxAVE_NUM.Text = userData.nAVE_NUM.ToString();

            nSFSound = new int[nSOUND_QUANTITY];
            nSound = new int[nSOUND_QUANTITY];
            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                nSFSound[i] = -1;
                nSound[i] = -1;
            }

            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            isRunning = false;
            if (userData.sSoundFile != null)
            {
                if (userData.sSoundFile.Length > 0)
                    WavesLoad_Sub();
            }

            Application.Idle += (s, e) => MainLoop();
        }

        private void MainLoop()
        {
            while (isRunning && DX.ProcessMessage() == 0)
            {
                // Windowsのメッセージを処理（フリーズ対策）
                Application.DoEvents();
                if (isRunning == false) return; // これ忘れずに

                MainLoop_Sub();
                Thread.Sleep(1);
            }
        }

        private void MainLoop_Sub()
        {
            // 画面をクリア
            if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == DX.TRUE)
            {
                nPushedL = nPushedR = -100; label5.Text = "";
            }

            DX.ClearDrawScreen();

            int nYOfs2 = 200;
            double dBPM = userData.dBPM;
            int nBPMSTART_1 = userData.nBunshi; // 分子　BMSヘッダに書くのは 分母-分子なので注意
            int nBPMSTART_2 = userData.nBunbo;　// 分母
            int nBPMSTART_3 = userData.nStartBar; // 開始小節

            // GetSoundCurrentPositionってサンプルじゃなくバイト数なんだよなｗ
            currentSamplePos = (long)((double)DX.GetSoundCurrentPosition(nSound[0]) / (double)(bitsPerSample / 8 * channels));
            if (currentSamplePos >= total_samples)
            {
                currentSamplePos = total_samples - 1;
                for (int i = 0; i < nSOUND_QUANTITY; i++)
                {
                    DX.StopSoundMem(nSound[i]);
                    DX.SetCurrentPositionSoundMem(currentSamplePos, nSound[i]);
                }
            }
            hScrollBar1.Value = (int)currentSamplePos;
            DX.DrawString(10, 65, $"Pos: {currentSamplePos}, TIme = {((double)currentSamplePos / (double)samplesPerSec).ToString("F3")}", DX.GetColor(255, 255, 255));

            int xs1, xs2;
            double n2;
            int yOsf = 120;
            SampleCount = (int)((double)samplesPerSec * dZoomRate); // dZoomRate=2の場合、441000サンプルを2秒分のデータを対象とする 　　点を打つ個数
            double dStartSamplePosUsingBPM = 0;
            double dDataSkipPitch = (double)SampleCount / (double)panelSOUND.Width; // Sampleデータを何個飛ばしで画面に描くか...横1ドット当たり何sampleか
            long j = (long)dDataSkipPitch;

            if (dBPM > 0.0)
            {
                double dSamples_Of_1Bar = (double)samplesPerSec * 60.0 / dBPM * 4.0; // 4分音符4つぶん（つまり1小節）のサンプル数
                dStartSamplePosUsingBPM = (double)samplesPerSec * 60.0 / dBPM * 4.0 * ((double)nBPMSTART_3 + ((double)nBPMSTART_1 / (double)nBPMSTART_2)); // 1小節を分母分割したときの分子番目から開始

                DX.DrawString(10, 80, $"samples/dot=X:{dDataSkipPitch.ToString("0.000")},Y:{32768 / 500}", DX.GetColor(255, 255, 255));
                int wd = (int)(dSamples_Of_1Bar / dDataSkipPitch); // 1小節分のピクセル数

                // 今、何個目のBPM 4小節単位ブロックか (=実際のサンプルから、開始までのサンプルを引き、小節あたりのサンプルで割る ---> 何個目の小節か（音楽開始からの）
                int nNum = (int)(((double)currentSamplePos - dStartSamplePosUsingBPM) / dSamples_Of_1Bar);
                if (nNum >= 0) // 音が鳴り始めるまでは nNum < 0 なので描画しない
                {
                    xs1 = nNum;
                    long pos = (long)(dStartSamplePosUsingBPM + dSamples_Of_1Bar * (double)nNum); // 4小節単位のBPMブロックの開始サンプル位置
                    xs2 = (int)(currentSamplePos - pos);
                    n2 = -((double)xs2 / dDataSkipPitch); // 画面の左端からのピクセル位置
                    for (int k = 0; k < ((panelSOUND.Width / wd) + 3) * 4; k++)
                    {
                        pos = (long)dStartSamplePosUsingBPM + (long)(dSamples_Of_1Bar * (double)nNum) + (long)((dSamples_Of_1Bar * (double)k) / 4);
                        if (n2 >= 0 && n2 < panelSOUND.Width)
                        {
                            if (k % 4 == 0)
                            {
                                DX.DrawString((int)(n2 + panelSOUND.Location.X), panelSOUND.Location.Y - 20 + nYOfs2, $"{xs1 + k / 4}小節,pos={pos}", DX.GetColor(255, 128, 192));
                                DX.DrawBox((int)(n2 + panelSOUND.Location.X), panelSOUND.Location.Y, (int)(n2 + panelSOUND.Location.X) + 2, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(255, 128, 192), DX.TRUE);
                            }
                            else
                            {
                                if (wd > 400)
                                    DX.DrawString((int)(n2 + panelSOUND.Location.X), panelSOUND.Location.Y - 20 + nYOfs2, $"{xs1 + k / 4}小節,pos={pos}", DX.GetColor(128, 64, 96));
                                DX.DrawBox((int)(n2 + panelSOUND.Location.X), panelSOUND.Location.Y, (int)(n2 + panelSOUND.Location.X) + 2, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(128, 64, 96), DX.TRUE);
                            }
                        }
                        n2 += (dSamples_Of_1Bar / dDataSkipPitch) / 4; // 1秒分のピクセル数を加算
                    }
                }
            }

            //// 秒単位の区切り縦線と秒数を描く 【ファイルのそもそもの秒】
            xs1 = (int)(currentSamplePos / samplesPerSec); // 何秒目か
            xs2 = (int)(currentSamplePos % samplesPerSec); // 何秒目かの余り
            n2 = -((double)xs2 / dDataSkipPitch); // 画面の左端からのピクセル位置
            for (int k = 0; k < (int)((double)SampleCount / (double)samplesPerSec) + 1; k++)
            {
                if (n2 >= 0 && n2 < panelSOUND.Width)
                {
                    DX.DrawString((int)(n2 + panelSOUND.Location.X), panelSOUND.Location.Y - 20 + nYOfs2, $"{xs1 + k}s", DX.GetColor(128, 128, 128));
                    DX.DrawBox((int)(n2 + panelSOUND.Location.X), panelSOUND.Location.Y, (int)(n2 + panelSOUND.Location.X) + 2, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(128, 128, 128), DX.TRUE);
                }
                n2 += (double)samplesPerSec / dDataSkipPitch; // 1秒分のピクセル数を加算
            }

            if (dBPM > 0.0)
            {
                // 開始位置を補正した秒 
                xs1 = (int)(((double)currentSamplePos - dStartSamplePosUsingBPM) / (double)samplesPerSec); // 何秒目か
                xs2 = (int)(((double)currentSamplePos - dStartSamplePosUsingBPM) % (double)samplesPerSec); // 何秒目かの余り
                n2 = -((double)xs2 / dDataSkipPitch); // 画面の左端からのピクセル位置
                for (int k = 0; k < (int)((double)SampleCount / (double)samplesPerSec) + 1; k++)
                {
                    if (n2 >= 0 && n2 < panelSOUND.Width)
                    {
                        DX.DrawString((int)(n2 + panelSOUND.Location.X + 4), panelSOUND.Location.Y - 20 + 30 + nYOfs2, $"{xs1 + k}s", DX.GetColor(255, 255, 255));
                        DX.DrawBox((int)(n2 + panelSOUND.Location.X), panelSOUND.Location.Y, (int)(n2 + panelSOUND.Location.X) + 2, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(255, 255, 255), DX.TRUE);
                    }
                    n2 += (double)samplesPerSec / dDataSkipPitch; // 1秒分のピクセル数を加算
                }
            }

            // 波形描画
            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                if (nSFSound[i] != -1)
                {
                    unsafe
                    {
                        short* ptr = (short*)DX.GetSoftSoundDataImage(nSFSound[i]).ToPointer();
                        if (ptr != null)
                        {
                            double startYL = 0;
                            double prevXL = 0;
                            double prevYL = startYL;
                            double startYR = 0;
                            double prevXR = 0;
                            double prevYR = startYR;
                            int v = 0;
                            DX.COLOR_U8 color = DX.GetColorU8(255, 255, 0, 255);
                            for (int u = 0; u < SampleCount; u += (int)j)
                            {
                                int n = (int)((currentSamplePos + u) / j) * (int)j; // 同じ位置を毎回指定するための正規化
                                if (currentSamplePos + u >= total_samples)
                                {
                                    break;
                                }

                                double x = (double)u / dDataSkipPitch;
                                // short値 (-32768 ～ 32767) を画面の高さに合わせてスケーリング
                                double yL, yR;
                                yL = startYL + ((ptr[(n) * 2 + 0]) / 500);
                                yR = startYR + ((ptr[(n) * 2 + 1]) / 500);

                                // 前の点から現在の点まで線を引く
                                vertices[v].pos.x = (float)(prevXL + panelSOUND.Location.X);
                                vertices[v].pos.y = yOsf + (float)((prevYL + prevYR) / 2 + panelSOUND.Location.Y);
                                vertices[v].pos.z = 0.0f;
                                vertices[v].rhw = 1.0f;
                                vertices[v].dif = color;
                                v++;
                                if (v >= nWAVE_SIZE) break;

                                prevXL = x;
                                prevYL = yL;
                                prevXR = x;
                                prevYR = yR;
                            }
                            DX.DrawPrimitive2D(vertices, v, DX.DX_PRIMTYPE_LINESTRIP, DX.DX_NONE_GRAPH, DX.TRUE);
                            DX.DrawString(4, yOsf + 4, $"{i.ToString("00")}:{sFilenames[i]}", DX.GetColor(255, 255, 255));
                        }
                    }
                }
                DX.DrawLine(0, yOsf, ClientSize.Width, yOsf, DX.GetColor(0, 0, 0));
                yOsf += 150;
            }

            if (nPushedL > -100)
            {
                DX.DrawBox(nPushedL, panelSOUND.Location.Y, nPushedL + 2, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(0, 255, 255), DX.TRUE);
            }
            if (nPushedR > -100)
            {
                DX.DrawBox(nPushedR, panelSOUND.Location.Y, nPushedR + 2, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(0, 255, 255), DX.TRUE);
            }
            if (nPushedL > -100 && nPushedR > -100)
            {
                int x1 = Math.Min(nPushedL, nPushedR);
                int x2 = Math.Max(nPushedL, nPushedR);
                int nDotsAmount = x2 - x1; // 何ドットの間隔か
                // x[sec] = ドット数×1ドット当たりのサンプル数 / samplesPerSec
                // BPM = 60 / x[sec] * 4
                double d = 4.0 * 60.0 / ((double)nDotsAmount * (double)((double)SampleCount / (double)panelSOUND.Width) / (double)samplesPerSec);
                label5.Text = $"推測BPM={d.ToString("F3")}";

                for (int x = nPushedL - nDotsAmount; x > -50; x -= nDotsAmount)
                {
                    DX.DrawBox(x, panelSOUND.Location.Y, x + 2, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(0, 192, 192), DX.TRUE);
                }
                for (int x = nPushedR + nDotsAmount; x <= panelSOUND.Width + nDotsAmount; x += nDotsAmount)
                {
                    DX.DrawBox(x, panelSOUND.Location.Y, x + 2, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(0, 192, 192), DX.TRUE);
                }
            }

            DX.DrawBox(panelSOUND.Location.X, panelSOUND.Location.Y, panelSOUND.Location.X + 3, panelSOUND.Location.Y + panelSOUND.Size.Height, DX.GetColor(255, 0, 0), DX.TRUE); // 赤

            // 裏画面の内容を表画面に反映
            DX.ScreenFlip();
        }

        // フォームが閉じられようとするときにループを止める
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // UserDataをJSON形式で保存
            userData.dBPM = double.Parse(textBoxBPM.Text);
            userData.nBunshi = int.Parse(textBoxBunshi.Text);
            userData.nBunbo = int.Parse(textBoxBunbo.Text);
            userData.nStartBar = int.Parse(textBoxStartBar.Text);
            userData.nMAX_MIN = int.Parse(textBoxMAX_MIN.Text);
            userData.nAVE_NUM = int.Parse(textBoxAVE_NUM.Text);
            // userData.sWaveFolder;
            // userData.sSoundFile;
            try
            {
                // オブジェクトをJSON文字列に変換
                string sFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string jsonString = JsonSerializer.Serialize(userData);
                // JSON文字列をファイルに書き込み
                File.WriteAllText(sFolder + JSON_FILENAME, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(JSON_FILENAME + " の書き込みに失敗しました: " + ex.Message);
            }

            if (isRunning)
            {
                isRunning = false;
                e.Cancel = true;

                if (DX.DxLib_IsInit() == DX.TRUE)
                {
                    for (int i = 0; i < nSOUND_QUANTITY; i++)
                    {
                        if (nSound[i] != -1)
                        {
                            DX.DeleteSoundMem(nSound[i]);
                            DX.DeleteSoundMem(nSFSound[i]);
                        }
                    }
                    DX.DxLib_End();
                }
                Close();
            }
        }

        // 再生かポーズか
        private void button1_Click(object sender, EventArgs e)
        {
            if (isRunning == false) return;

            try
            {
                userData.nBunshi = int.Parse(textBoxBunshi.Text);
                userData.nBunbo = int.Parse(textBoxBunbo.Text);
                userData.nStartBar = int.Parse(textBoxStartBar.Text);
                userData.dBPM = double.Parse(textBoxBPM.Text);
            }
            catch
            {
                MessageBox.Show("BPMの値が不正です。");
                return;
            }
            if (userData.nBunshi < 0 || userData.nBunbo <= 0 || userData.nStartBar < 0 || userData.dBPM < 0 || userData.nBunshi >= userData.nBunbo)
            {
                MessageBox.Show("BPM関連の値が不正です。");
                return;
            }

            bool bPlaying = false;
            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                if (DX.CheckSoundMem(nSound[i]) == DX.TRUE)
                {
                    bPlaying = true;
                    break;
                }
            }

            if (bPlaying)
            {
                for (int i = 0; i < nSOUND_QUANTITY; i++)
                {
                    DX.StopSoundMem(nSound[i]);
                }
            }
            else
            {
                for (int i = 0; i < nSOUND_QUANTITY; i++)
                {
                    DX.PlaySoundMem(nSound[i], DX.DX_PLAYTYPE_BACK, DX.FALSE);
                }
            }
        }

        // 止めてスクロール
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (isRunning == false) return;

            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                DX.StopSoundMem(nSound[i]);
                DX.SetCurrentPositionSoundMem(hScrollBar1.Value, nSound[i]);
            }
            MainLoop_Sub();
        }

        // 縮小
        private void button2_Click(object sender, EventArgs e)
        {
            if (isRunning == false) return;

            nPushedL = nPushedR = -100; label5.Text = "";
            if (dZoomRate > 0.125)
            {
                dZoomRate /= 2.0;
            }
            else
                dZoomRate = 0.125;
            labelZoomVal.Text = $"Zoom={(1.0 / dZoomRate * 2.0).ToString("F2")}";
        }

        // 拡大
        private void button3_Click(object sender, EventArgs e)
        {
            if (isRunning == false) return;

            nPushedL = nPushedR = -100; label5.Text = "";
            if (dZoomRate < 8)
            {
                dZoomRate *= 2.0;
            }
            else
                dZoomRate = 8;
            labelZoomVal.Text = $"Zoom={(1.0 / dZoomRate * 2.0).ToString("F2")}";
        }

        // Mute
        private void checkBoxMute0_CheckedChanged(object sender, EventArgs e)
        {
            if (isRunning == false) return;
            if (checkBoxMute0.Checked)
                DX.ChangeVolumeSoundMem(0, nSound[0]);
            else
                DX.ChangeVolumeSoundMem(255, nSound[0]);
        }

        private void checkBoxMute1_CheckedChanged(object sender, EventArgs e)
        {
            if (isRunning == false) return;
            if (checkBoxMute1.Checked)
                DX.ChangeVolumeSoundMem(0, nSound[1]);
            else
                DX.ChangeVolumeSoundMem(255, nSound[1]);
        }

        private void checkBoxMute2_CheckedChanged(object sender, EventArgs e)
        {
            if (isRunning == false) return;
            if (checkBoxMute2.Checked)
                DX.ChangeVolumeSoundMem(0, nSound[2]);
            else
                DX.ChangeVolumeSoundMem(255, nSound[2]);
        }

        private void checkBoxMute3_CheckedChanged(object sender, EventArgs e)
        {
            if (isRunning == false) return;
            if (checkBoxMute3.Checked)
                DX.ChangeVolumeSoundMem(0, nSound[3]);
            else
                DX.ChangeVolumeSoundMem(255, nSound[3]);
        }

        private void checkBoxMute4_CheckedChanged(object sender, EventArgs e)
        {
            if (isRunning == false) return;
            if (checkBoxMute4.Checked)
                DX.ChangeVolumeSoundMem(0, nSound[4]);
            else
                DX.ChangeVolumeSoundMem(255, nSound[4]);
        }

        private void checkBoxMute5_CheckedChanged(object sender, EventArgs e)
        {
            if (isRunning == false) return;
            if (checkBoxMute5.Checked)
                DX.ChangeVolumeSoundMem(0, nSound[5]);
            else
                DX.ChangeVolumeSoundMem(255, nSound[5]);
        }

        private void checkBoxMute6_CheckedChanged(object sender, EventArgs e)
        {
            if (isRunning == false) return;
            if (checkBoxMute6.Checked)
                DX.ChangeVolumeSoundMem(0, nSound[6]);
            else
                DX.ChangeVolumeSoundMem(255, nSound[6]);
        }

        // 音量が指定値以上になる最初のサンプル位置を探す
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (isRunning == false) return;

            long posNow = 0, posExpire = 999999999;
            double v = double.Parse(textBoxMAX_MIN.Text);
            int chNum = -1;
            int nAVGSample = int.Parse(textBoxAVE_NUM.Text);
            if (nAVGSample <= 0)
                return;

            unsafe
            {
                while (posNow < total_samples) // posNow
                {
                    long l = posNow;
                    for (int i = 3; i < 4; i++)
                    //for (int i = 0; i < nSOUND_QUANTITY; i++)
                    {
                        short* ptr = (short*)DX.GetSoftSoundDataImage(nSFSound[i]).ToPointer();
                        short maxL = -30000, maxR = -30000;
                        short minL = 30000, minR = 30000;

                        for (int j = 0; j < nAVGSample; j++)
                        {
                            short sampleL = ptr[l + j * 2 + 0];
                            short sampleR = ptr[l + j * 2 + 1];
                            if (sampleL > maxL) maxL = sampleL;
                            if (sampleR > maxR) maxR = sampleR;
                            if (sampleL < minL) minL = sampleL;
                            if (sampleR < minR) minR = sampleR;
                        }

                        double vL = (double)(maxL - minL);
                        double vR = (double)(maxR - minR);
                        if (vL >= v || vR >= v)
                        {
                            if (l < posExpire)
                            {
                                posExpire = l / 2;
                                chNum = i;
                                break;
                            }
                        }
                        l += 2; // 2chなので2サンプルずつ進める
                    }
                    if (chNum != -1) break; // 見つかったらループを抜ける)
                    posNow += nAVGSample * 2; // 2chなので2サンプルずつ進める
                }
            }
            MessageBox.Show($"音量差が{v}以上になる最初のサンプル群({nAVGSample}平均)位置は {posExpire}(Ch={chNum}) です。");
        }

        // 更新
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (isRunning == false) return;

            try
            {
                userData.nBunshi = int.Parse(textBoxBunshi.Text);
                userData.nBunbo = int.Parse(textBoxBunbo.Text);
                userData.nStartBar = int.Parse(textBoxStartBar.Text);
                userData.dBPM = double.Parse(textBoxBPM.Text);
            }
            catch
            {
                MessageBox.Show("BPMの値が不正です。");
                return;
            }
            if (userData.nBunshi < 0 || userData.nBunbo <= 0 || userData.nStartBar < 0 || userData.dBPM < 0 || userData.nBunshi >= userData.nBunbo)
            {
                MessageBox.Show("BPM関連の値が不正です。");
                return;
            }
        }

        private void buttonLoadWaves_Click(object sender, EventArgs e)
        {
            WavesLoad();
        }

        /*
         * BMSの先頭部分に使えるデータを書き出します。UTF-8
         * WAVは2番から (アプリ上では0番からなので+2しています
         * 小節は001から (アプリ上では0小節からなので+1しています
         * BMPも01をCh04用にダミーで出力（動画用）
         * 
         * 必要なものだけコピペしてBMSに貼り付けてご使用ください（※バックアップは必ず取っておいてください）
         */
        private void buttonMakeHeaderData_Click(object sender, EventArgs e)
        {
            if (isRunning == false) return;

            List<string> lines = new List<string>();
            lines.Add("#BPM " + userData.dBPM);
            lines.Add("");
            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                string sFile = Path.GetFileName(userData.sSoundFile[i]);
                // sFileの拡張子を".wav"に変更
                sFile = Path.ChangeExtension(sFile, ".wav");
                lines.Add($"#WAV{(i + 2):00} {sFile}"); // WAVは2番から
            }
            lines.Add("");
            lines.Add("#BMP01 bga.bmp"); // BGAもダミーで（不要なら消してください
            lines.Add("");

            int nNote = int.Parse(textBoxStartBar.Text) + 1; // 1小節以降にする
            int nMax = int.Parse(textBoxBunbo.Text);
            int nMin = int.Parse(textBoxBunshi.Text);
            nMin = nMax - nMin;
            string s;
            for (int i = 0; i < nSOUND_QUANTITY; i++)
            {
                s = $"#{nNote.ToString("000")}01:";
                for (int j = 0; j < nMax; j++)
                {
                    if (j == nMin)
                        s += $"{(i + 2).ToString("00")}";
                    else
                        s += "00";
                }
                lines.Add(s);
            }

            s = $"#{nNote.ToString("000")}04:";
            for (int j = 0; j < nMax; j++)
            {
                if (j == nMin)
                    s += $"{(1).ToString("00")}";
                else
                    s += "00";
            }
            lines.Add(s);

            // 実行ファイルと同じフォルダに "output.bms" という名前で保存
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.bms.txt");
            File.WriteAllLines(outputPath, lines);
            // explorerで開く
            System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{outputPath}\"");
        }

        // BPM推測用資料として線を表示する（ための座標指定）
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            label5.Text = $"(x, y)=({e.X}, {e.Y})";
            if (e.Button == MouseButtons.Left)
            {
                nPushedL = e.X;
            }
            else if (e.Button == MouseButtons.Right)
            {
                nPushedR = e.X;
            }
        }

    }
}