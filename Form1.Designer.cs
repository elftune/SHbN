namespace DxLibCSTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonPlay = new Button();
            hScrollBar1 = new HScrollBar();
            buttonCheckStartPos = new Button();
            textBoxMAX_MIN = new TextBox();
            buttonZoomUp = new Button();
            buttonZoomDown = new Button();
            labelZoomVal = new Label();
            textBoxAVE_NUM = new TextBox();
            label7 = new Label();
            label8 = new Label();
            panel2 = new Panel();
            panel1 = new Panel();
            label5 = new Label();
            buttonLoadWaves = new Button();
            buttonMakeHeaderData = new Button();
            label6 = new Label();
            buttonUpdate = new Button();
            checkBoxMute0 = new CheckBox();
            checkBoxMute1 = new CheckBox();
            checkBoxMute5 = new CheckBox();
            checkBoxMute4 = new CheckBox();
            checkBoxMute3 = new CheckBox();
            checkBoxMute6 = new CheckBox();
            checkBoxMute2 = new CheckBox();
            textBoxStartBar = new TextBox();
            label1 = new Label();
            textBoxBunshi = new TextBox();
            textBoxBPM = new TextBox();
            label4 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBoxBunbo = new TextBox();
            panelSOUND = new Panel();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonPlay
            // 
            buttonPlay.Location = new Point(559, 74);
            buttonPlay.Margin = new Padding(3, 4, 3, 4);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(542, 44);
            buttonPlay.TabIndex = 0;
            buttonPlay.Text = "PLAY / PAUSE";
            buttonPlay.UseVisualStyleBackColor = true;
            buttonPlay.Click += button1_Click;
            // 
            // hScrollBar1
            // 
            hScrollBar1.Location = new Point(3, 0);
            hScrollBar1.Name = "hScrollBar1";
            hScrollBar1.Size = new Size(1958, 30);
            hScrollBar1.TabIndex = 2;
            hScrollBar1.Scroll += hScrollBar1_Scroll;
            // 
            // buttonCheckStartPos
            // 
            buttonCheckStartPos.Location = new Point(1198, 70);
            buttonCheckStartPos.Margin = new Padding(3, 4, 3, 4);
            buttonCheckStartPos.Name = "buttonCheckStartPos";
            buttonCheckStartPos.Size = new Size(183, 42);
            buttonCheckStartPos.TabIndex = 3;
            buttonCheckStartPos.Text = "開始位置検証";
            buttonCheckStartPos.UseVisualStyleBackColor = true;
            buttonCheckStartPos.Click += button1_Click_1;
            // 
            // textBoxMAX_MIN
            // 
            textBoxMAX_MIN.Location = new Point(1538, 70);
            textBoxMAX_MIN.Margin = new Padding(3, 4, 3, 4);
            textBoxMAX_MIN.Name = "textBoxMAX_MIN";
            textBoxMAX_MIN.Size = new Size(151, 35);
            textBoxMAX_MIN.TabIndex = 4;
            textBoxMAX_MIN.TextAlign = HorizontalAlignment.Center;
            // 
            // buttonZoomUp
            // 
            buttonZoomUp.Location = new Point(286, 78);
            buttonZoomUp.Margin = new Padding(3, 4, 3, 4);
            buttonZoomUp.Name = "buttonZoomUp";
            buttonZoomUp.Size = new Size(77, 40);
            buttonZoomUp.TabIndex = 1;
            buttonZoomUp.Text = "拡大";
            buttonZoomUp.UseVisualStyleBackColor = true;
            buttonZoomUp.Click += button2_Click;
            // 
            // buttonZoomDown
            // 
            buttonZoomDown.Location = new Point(379, 78);
            buttonZoomDown.Margin = new Padding(3, 4, 3, 4);
            buttonZoomDown.Name = "buttonZoomDown";
            buttonZoomDown.Size = new Size(77, 40);
            buttonZoomDown.TabIndex = 2;
            buttonZoomDown.Text = "縮小";
            buttonZoomDown.UseVisualStyleBackColor = true;
            buttonZoomDown.Click += button3_Click;
            // 
            // labelZoomVal
            // 
            labelZoomVal.AutoSize = true;
            labelZoomVal.BackColor = Color.FromArgb(64, 64, 64);
            labelZoomVal.ForeColor = Color.White;
            labelZoomVal.Location = new Point(154, 76);
            labelZoomVal.Name = "labelZoomVal";
            labelZoomVal.Size = new Size(119, 30);
            labelZoomVal.TabIndex = 21;
            labelZoomVal.Text = "Zoom=1.00";
            // 
            // textBoxAVE_NUM
            // 
            textBoxAVE_NUM.Location = new Point(1819, 70);
            textBoxAVE_NUM.Margin = new Padding(3, 4, 3, 4);
            textBoxAVE_NUM.Name = "textBoxAVE_NUM";
            textBoxAVE_NUM.Size = new Size(114, 35);
            textBoxAVE_NUM.TabIndex = 5;
            textBoxAVE_NUM.TextAlign = HorizontalAlignment.Center;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(64, 64, 64);
            label7.ForeColor = Color.White;
            label7.Location = new Point(1716, 70);
            label7.Name = "label7";
            label7.Size = new Size(97, 30);
            label7.TabIndex = 31;
            label7.Text = "個数平均";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.FromArgb(64, 64, 64);
            label8.ForeColor = Color.White;
            label8.Location = new Point(1407, 70);
            label8.Name = "label8";
            label8.Size = new Size(118, 30);
            label8.TabIndex = 32;
            label8.Text = "最大最小差";
            // 
            // panel2
            // 
            panel2.Controls.Add(hScrollBar1);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(labelZoomVal);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(buttonPlay);
            panel2.Controls.Add(textBoxAVE_NUM);
            panel2.Controls.Add(buttonZoomUp);
            panel2.Controls.Add(textBoxMAX_MIN);
            panel2.Controls.Add(buttonZoomDown);
            panel2.Controls.Add(buttonCheckStartPos);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 1982);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(1961, 142);
            panel2.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Controls.Add(label5);
            panel1.Controls.Add(buttonLoadWaves);
            panel1.Controls.Add(buttonMakeHeaderData);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(buttonUpdate);
            panel1.Controls.Add(checkBoxMute0);
            panel1.Controls.Add(checkBoxMute1);
            panel1.Controls.Add(checkBoxMute5);
            panel1.Controls.Add(checkBoxMute4);
            panel1.Controls.Add(checkBoxMute3);
            panel1.Controls.Add(checkBoxMute6);
            panel1.Controls.Add(checkBoxMute2);
            panel1.Controls.Add(textBoxStartBar);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBoxBunshi);
            panel1.Controls.Add(textBoxBPM);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(textBoxBunbo);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1961, 62);
            panel1.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(29, 19);
            label5.Name = "label5";
            label5.Size = new Size(0, 30);
            label5.TabIndex = 0;
            // 
            // buttonLoadWaves
            // 
            buttonLoadWaves.Location = new Point(281, 4);
            buttonLoadWaves.Margin = new Padding(3, 4, 3, 4);
            buttonLoadWaves.Name = "buttonLoadWaves";
            buttonLoadWaves.Size = new Size(101, 48);
            buttonLoadWaves.TabIndex = 34;
            buttonLoadWaves.Text = "読込";
            buttonLoadWaves.UseVisualStyleBackColor = true;
            buttonLoadWaves.Click += buttonLoadWaves_Click;
            // 
            // buttonMakeHeaderData
            // 
            buttonMakeHeaderData.Location = new Point(1550, 2);
            buttonMakeHeaderData.Margin = new Padding(3, 4, 3, 4);
            buttonMakeHeaderData.Name = "buttonMakeHeaderData";
            buttonMakeHeaderData.Size = new Size(129, 48);
            buttonMakeHeaderData.TabIndex = 33;
            buttonMakeHeaderData.Text = "資料作成";
            buttonMakeHeaderData.UseVisualStyleBackColor = true;
            buttonMakeHeaderData.Click += buttonMakeHeaderData_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(410, 12);
            label6.Name = "label6";
            label6.Size = new Size(62, 30);
            label6.TabIndex = 29;
            label6.Text = "Mute";
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(1370, 0);
            buttonUpdate.Margin = new Padding(3, 4, 3, 4);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(156, 48);
            buttonUpdate.TabIndex = 9;
            buttonUpdate.Text = "更新";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += buttonUpdate_Click;
            // 
            // checkBoxMute0
            // 
            checkBoxMute0.AutoSize = true;
            checkBoxMute0.Location = new Point(477, 6);
            checkBoxMute0.Margin = new Padding(3, 4, 3, 4);
            checkBoxMute0.Name = "checkBoxMute0";
            checkBoxMute0.Size = new Size(50, 34);
            checkBoxMute0.TabIndex = 11;
            checkBoxMute0.Text = "0";
            checkBoxMute0.UseVisualStyleBackColor = true;
            checkBoxMute0.CheckedChanged += checkBoxMute0_CheckedChanged;
            // 
            // checkBoxMute1
            // 
            checkBoxMute1.AutoSize = true;
            checkBoxMute1.Location = new Point(533, 6);
            checkBoxMute1.Margin = new Padding(3, 4, 3, 4);
            checkBoxMute1.Name = "checkBoxMute1";
            checkBoxMute1.Size = new Size(50, 34);
            checkBoxMute1.TabIndex = 12;
            checkBoxMute1.Text = "1";
            checkBoxMute1.UseVisualStyleBackColor = true;
            checkBoxMute1.CheckedChanged += checkBoxMute1_CheckedChanged;
            // 
            // checkBoxMute5
            // 
            checkBoxMute5.AutoSize = true;
            checkBoxMute5.Location = new Point(763, 6);
            checkBoxMute5.Margin = new Padding(3, 4, 3, 4);
            checkBoxMute5.Name = "checkBoxMute5";
            checkBoxMute5.Size = new Size(50, 34);
            checkBoxMute5.TabIndex = 16;
            checkBoxMute5.Text = "5";
            checkBoxMute5.UseVisualStyleBackColor = true;
            checkBoxMute5.CheckedChanged += checkBoxMute5_CheckedChanged;
            // 
            // checkBoxMute4
            // 
            checkBoxMute4.AutoSize = true;
            checkBoxMute4.Location = new Point(706, 6);
            checkBoxMute4.Margin = new Padding(3, 4, 3, 4);
            checkBoxMute4.Name = "checkBoxMute4";
            checkBoxMute4.Size = new Size(50, 34);
            checkBoxMute4.TabIndex = 15;
            checkBoxMute4.Text = "4";
            checkBoxMute4.UseVisualStyleBackColor = true;
            checkBoxMute4.CheckedChanged += checkBoxMute4_CheckedChanged;
            // 
            // checkBoxMute3
            // 
            checkBoxMute3.AutoSize = true;
            checkBoxMute3.Location = new Point(651, 6);
            checkBoxMute3.Margin = new Padding(3, 4, 3, 4);
            checkBoxMute3.Name = "checkBoxMute3";
            checkBoxMute3.Size = new Size(50, 34);
            checkBoxMute3.TabIndex = 14;
            checkBoxMute3.Text = "3";
            checkBoxMute3.UseVisualStyleBackColor = true;
            checkBoxMute3.CheckedChanged += checkBoxMute3_CheckedChanged;
            // 
            // checkBoxMute6
            // 
            checkBoxMute6.AutoSize = true;
            checkBoxMute6.Location = new Point(819, 6);
            checkBoxMute6.Margin = new Padding(3, 4, 3, 4);
            checkBoxMute6.Name = "checkBoxMute6";
            checkBoxMute6.Size = new Size(50, 34);
            checkBoxMute6.TabIndex = 17;
            checkBoxMute6.Text = "6";
            checkBoxMute6.UseVisualStyleBackColor = true;
            checkBoxMute6.CheckedChanged += checkBoxMute6_CheckedChanged;
            // 
            // checkBoxMute2
            // 
            checkBoxMute2.AutoSize = true;
            checkBoxMute2.Location = new Point(595, 6);
            checkBoxMute2.Margin = new Padding(3, 4, 3, 4);
            checkBoxMute2.Name = "checkBoxMute2";
            checkBoxMute2.Size = new Size(50, 34);
            checkBoxMute2.TabIndex = 13;
            checkBoxMute2.Text = "2";
            checkBoxMute2.UseVisualStyleBackColor = true;
            checkBoxMute2.CheckedChanged += checkBoxMute2_CheckedChanged;
            // 
            // textBoxStartBar
            // 
            textBoxStartBar.Enabled = false;
            textBoxStartBar.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            textBoxStartBar.Location = new Point(960, 4);
            textBoxStartBar.Margin = new Padding(3, 4, 3, 4);
            textBoxStartBar.Name = "textBoxStartBar";
            textBoxStartBar.Size = new Size(73, 45);
            textBoxStartBar.TabIndex = 6;
            textBoxStartBar.TextAlign = HorizontalAlignment.Right;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1714, 14);
            label1.Name = "label1";
            label1.Size = new Size(56, 30);
            label1.TabIndex = 10;
            label1.Text = "BPM";
            // 
            // textBoxBunshi
            // 
            textBoxBunshi.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            textBoxBunshi.Location = new Point(1116, 4);
            textBoxBunshi.Margin = new Padding(3, 4, 3, 4);
            textBoxBunshi.Name = "textBoxBunshi";
            textBoxBunshi.Size = new Size(97, 45);
            textBoxBunshi.TabIndex = 7;
            textBoxBunshi.TextAlign = HorizontalAlignment.Right;
            // 
            // textBoxBPM
            // 
            textBoxBPM.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            textBoxBPM.Location = new Point(1790, 6);
            textBoxBPM.Margin = new Padding(3, 4, 3, 4);
            textBoxBPM.Name = "textBoxBPM";
            textBoxBPM.Size = new Size(150, 45);
            textBoxBPM.TabIndex = 10;
            textBoxBPM.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1037, 4);
            label4.Name = "label4";
            label4.Size = new Size(72, 30);
            label4.TabIndex = 16;
            label4.Text = "小節の";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(898, 4);
            label2.Name = "label2";
            label2.Size = new Size(55, 30);
            label2.TabIndex = 12;
            label2.Text = "開始";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1217, 4);
            label3.Name = "label3";
            label3.Size = new Size(21, 30);
            label3.TabIndex = 13;
            label3.Text = "/";
            // 
            // textBoxBunbo
            // 
            textBoxBunbo.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            textBoxBunbo.Location = new Point(1245, 4);
            textBoxBunbo.Margin = new Padding(3, 4, 3, 4);
            textBoxBunbo.Name = "textBoxBunbo";
            textBoxBunbo.Size = new Size(98, 45);
            textBoxBunbo.TabIndex = 8;
            textBoxBunbo.TextAlign = HorizontalAlignment.Right;
            // 
            // panelSOUND
            // 
            panelSOUND.BackColor = Color.FromArgb(192, 255, 255);
            panelSOUND.Location = new Point(0, 62);
            panelSOUND.Margin = new Padding(3, 4, 3, 4);
            panelSOUND.Name = "panelSOUND";
            panelSOUND.Size = new Size(1961, 2366);
            panelSOUND.TabIndex = 4;
            panelSOUND.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1961, 2124);
            Controls.Add(panel2);
            Controls.Add(panelSOUND);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SHbN - Stemを並べて発声開始位置とかBPMとかの調査をなんちゃって支援";
            MouseClick += Form1_MouseClick;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonPlay;
        private HScrollBar hScrollBar1;
        private Button buttonCheckStartPos;
        private TextBox textBoxMAX_MIN;
        private Button buttonZoomUp;
        private Button buttonZoomDown;
        private Label labelZoomVal;
        private TextBox textBoxAVE_NUM;
        private Label label7;
        private Label label8;
        private Panel panel2;
        private Panel panel1;
        private Button buttonLoadWaves;
        private Button buttonMakeHeaderData;
        private Label label6;
        private Button buttonUpdate;
        private CheckBox checkBoxMute0;
        private CheckBox checkBoxMute1;
        private CheckBox checkBoxMute5;
        private CheckBox checkBoxMute4;
        private CheckBox checkBoxMute3;
        private CheckBox checkBoxMute6;
        private CheckBox checkBoxMute2;
        private TextBox textBoxStartBar;
        private Label label1;
        private TextBox textBoxBunshi;
        private TextBox textBoxBPM;
        private Label label4;
        private Label label2;
        private Label label3;
        private TextBox textBoxBunbo;
        private Panel panelSOUND;
        private Label label5;
    }
}
