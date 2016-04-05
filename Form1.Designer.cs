namespace TestScreenshot
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.btnInject = new System.Windows.Forms.Button();
      this.btnCapture = new System.Windows.Forms.Button();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.btnLoadTest = new System.Windows.Forms.Button();
      this.txtNumber = new System.Windows.Forms.TextBox();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.txtCaptureX = new System.Windows.Forms.TextBox();
      this.txtCaptureY = new System.Windows.Forms.TextBox();
      this.txtCaptureWidth = new System.Windows.Forms.TextBox();
      this.txtCaptureHeight = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txtDebugLog = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.cbAutoGAC = new System.Windows.Forms.CheckBox();
      this.rbDirect3D9 = new System.Windows.Forms.RadioButton();
      this.rbDirect3D10 = new System.Windows.Forms.RadioButton();
      this.rbDirect3D11 = new System.Windows.Forms.RadioButton();
      this.rbAutodetect = new System.Windows.Forms.RadioButton();
      this.rbDirect3D10_1 = new System.Windows.Forms.RadioButton();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.btnStartHyperionMonitor = new System.Windows.Forms.Button();
      this.lblHyperionIP = new System.Windows.Forms.Label();
      this.lblHyperionProtoPort = new System.Windows.Forms.Label();
      this.tbHyperionInterval = new System.Windows.Forms.TextBox();
      this.tbHyperionProtoPort = new System.Windows.Forms.TextBox();
      this.tbHyperionIP = new System.Windows.Forms.TextBox();
      this.cmbFormat = new System.Windows.Forms.ComboBox();
      this.txtResizeHeight = new System.Windows.Forms.TextBox();
      this.txtResizeWidth = new System.Windows.Forms.TextBox();
      this.cbDrawOverlay = new System.Windows.Forms.CheckBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.lblHyperionInterval = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // btnInject
      // 
      this.btnInject.Location = new System.Drawing.Point(305, 20);
      this.btnInject.Name = "btnInject";
      this.btnInject.Size = new System.Drawing.Size(74, 23);
      this.btnInject.TabIndex = 0;
      this.btnInject.Text = "Inject";
      this.btnInject.UseVisualStyleBackColor = true;
      this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
      // 
      // btnCapture
      // 
      this.btnCapture.Enabled = false;
      this.btnCapture.Location = new System.Drawing.Point(832, 320);
      this.btnCapture.Name = "btnCapture";
      this.btnCapture.Size = new System.Drawing.Size(75, 41);
      this.btnCapture.TabIndex = 1;
      this.btnCapture.Text = "Request Capture";
      this.btnCapture.UseVisualStyleBackColor = true;
      this.btnCapture.Visible = false;
      this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.Location = new System.Drawing.Point(385, 20);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(614, 294);
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      // 
      // btnLoadTest
      // 
      this.btnLoadTest.Enabled = false;
      this.btnLoadTest.Location = new System.Drawing.Point(832, 472);
      this.btnLoadTest.Name = "btnLoadTest";
      this.btnLoadTest.Size = new System.Drawing.Size(75, 23);
      this.btnLoadTest.TabIndex = 3;
      this.btnLoadTest.Text = "Load Test";
      this.btnLoadTest.UseVisualStyleBackColor = true;
      this.btnLoadTest.Visible = false;
      this.btnLoadTest.Click += new System.EventHandler(this.btnLoadTest_Click);
      // 
      // txtNumber
      // 
      this.txtNumber.Location = new System.Drawing.Point(916, 341);
      this.txtNumber.Name = "txtNumber";
      this.txtNumber.Size = new System.Drawing.Size(83, 20);
      this.txtNumber.TabIndex = 4;
      this.txtNumber.Text = "100";
      this.txtNumber.Visible = false;
      // 
      // progressBar1
      // 
      this.progressBar1.Location = new System.Drawing.Point(832, 372);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(167, 23);
      this.progressBar1.TabIndex = 5;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 236);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(14, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "X";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(124, 200);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(14, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Y";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(11, 226);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(35, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Width";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(100, 226);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(38, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Height";
      // 
      // txtCaptureX
      // 
      this.txtCaptureX.Location = new System.Drawing.Point(47, 197);
      this.txtCaptureX.Name = "txtCaptureX";
      this.txtCaptureX.Size = new System.Drawing.Size(47, 20);
      this.txtCaptureX.TabIndex = 11;
      this.txtCaptureX.Text = "0";
      // 
      // txtCaptureY
      // 
      this.txtCaptureY.Location = new System.Drawing.Point(144, 197);
      this.txtCaptureY.Name = "txtCaptureY";
      this.txtCaptureY.Size = new System.Drawing.Size(47, 20);
      this.txtCaptureY.TabIndex = 12;
      this.txtCaptureY.Text = "0";
      // 
      // txtCaptureWidth
      // 
      this.txtCaptureWidth.Location = new System.Drawing.Point(47, 223);
      this.txtCaptureWidth.Name = "txtCaptureWidth";
      this.txtCaptureWidth.Size = new System.Drawing.Size(47, 20);
      this.txtCaptureWidth.TabIndex = 13;
      this.txtCaptureWidth.Text = "0";
      // 
      // txtCaptureHeight
      // 
      this.txtCaptureHeight.Location = new System.Drawing.Point(144, 223);
      this.txtCaptureHeight.Name = "txtCaptureHeight";
      this.txtCaptureHeight.Size = new System.Drawing.Size(47, 20);
      this.txtCaptureHeight.TabIndex = 14;
      this.txtCaptureHeight.Text = "0";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(194, 204);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(184, 13);
      this.label5.TabIndex = 15;
      this.label5.Text = "Width of 0 means capture full window";
      // 
      // txtDebugLog
      // 
      this.txtDebugLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDebugLog.Location = new System.Drawing.Point(5, 320);
      this.txtDebugLog.Multiline = true;
      this.txtDebugLog.Name = "txtDebugLog";
      this.txtDebugLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtDebugLog.Size = new System.Drawing.Size(808, 194);
      this.txtDebugLog.TabIndex = 16;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(13, 7);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(174, 13);
      this.label6.TabIndex = 17;
      this.label6.Text = "EXE Name of Direct3D Application:";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(913, 320);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(86, 13);
      this.label7.TabIndex = 18;
      this.label7.Text = "Capture Multiple:";
      this.label7.Visible = false;
      // 
      // cbAutoGAC
      // 
      this.cbAutoGAC.AutoSize = true;
      this.cbAutoGAC.Location = new System.Drawing.Point(820, 426);
      this.cbAutoGAC.Name = "cbAutoGAC";
      this.cbAutoGAC.Size = new System.Drawing.Size(179, 17);
      this.cbAutoGAC.TabIndex = 25;
      this.cbAutoGAC.Text = "Auto register GAC (run as admin)";
      this.cbAutoGAC.UseVisualStyleBackColor = true;
      this.cbAutoGAC.Visible = false;
      // 
      // rbDirect3D9
      // 
      this.rbDirect3D9.AutoSize = true;
      this.rbDirect3D9.Location = new System.Drawing.Point(284, 59);
      this.rbDirect3D9.Name = "rbDirect3D9";
      this.rbDirect3D9.Size = new System.Drawing.Size(76, 17);
      this.rbDirect3D9.TabIndex = 21;
      this.rbDirect3D9.Text = "Direct3D 9";
      this.rbDirect3D9.UseVisualStyleBackColor = true;
      // 
      // rbDirect3D10
      // 
      this.rbDirect3D10.AutoSize = true;
      this.rbDirect3D10.Location = new System.Drawing.Point(201, 82);
      this.rbDirect3D10.Name = "rbDirect3D10";
      this.rbDirect3D10.Size = new System.Drawing.Size(82, 17);
      this.rbDirect3D10.TabIndex = 22;
      this.rbDirect3D10.Text = "Direct3D 10";
      this.rbDirect3D10.UseVisualStyleBackColor = true;
      // 
      // rbDirect3D11
      // 
      this.rbDirect3D11.AutoSize = true;
      this.rbDirect3D11.Location = new System.Drawing.Point(201, 105);
      this.rbDirect3D11.Name = "rbDirect3D11";
      this.rbDirect3D11.Size = new System.Drawing.Size(82, 17);
      this.rbDirect3D11.TabIndex = 24;
      this.rbDirect3D11.Text = "Direct3D 11";
      this.rbDirect3D11.UseVisualStyleBackColor = true;
      // 
      // rbAutodetect
      // 
      this.rbAutodetect.AutoSize = true;
      this.rbAutodetect.Checked = true;
      this.rbAutodetect.Location = new System.Drawing.Point(201, 59);
      this.rbAutodetect.Name = "rbAutodetect";
      this.rbAutodetect.Size = new System.Drawing.Size(77, 17);
      this.rbAutodetect.TabIndex = 20;
      this.rbAutodetect.TabStop = true;
      this.rbAutodetect.Text = "Autodetect";
      this.rbAutodetect.UseVisualStyleBackColor = true;
      // 
      // rbDirect3D10_1
      // 
      this.rbDirect3D10_1.AutoSize = true;
      this.rbDirect3D10_1.Location = new System.Drawing.Point(284, 82);
      this.rbDirect3D10_1.Name = "rbDirect3D10_1";
      this.rbDirect3D10_1.Size = new System.Drawing.Size(91, 17);
      this.rbDirect3D10_1.TabIndex = 23;
      this.rbDirect3D10_1.Text = "Direct3D 10.1";
      this.rbDirect3D10_1.UseVisualStyleBackColor = true;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(11, 181);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(84, 13);
      this.label8.TabIndex = 27;
      this.label8.Text = "SCAN REGION:";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(198, 181);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(49, 13);
      this.label9.TabIndex = 28;
      this.label9.Text = "RESIZE:";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(287, 226);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(38, 13);
      this.label10.TabIndex = 30;
      this.label10.Text = "Height";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(198, 226);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(35, 13);
      this.label11.TabIndex = 29;
      this.label11.Text = "Width";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(7, 290);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(55, 13);
      this.label12.TabIndex = 33;
      this.label12.Text = "FORMAT:";
      // 
      // btnStartHyperionMonitor
      // 
      this.btnStartHyperionMonitor.Location = new System.Drawing.Point(16, 134);
      this.btnStartHyperionMonitor.Name = "btnStartHyperionMonitor";
      this.btnStartHyperionMonitor.Size = new System.Drawing.Size(163, 23);
      this.btnStartHyperionMonitor.TabIndex = 35;
      this.btnStartHyperionMonitor.Text = "Start hyperion forwarding";
      this.btnStartHyperionMonitor.UseVisualStyleBackColor = true;
      this.btnStartHyperionMonitor.Click += new System.EventHandler(this.btnStartHyperionMonitor_Click);
      // 
      // lblHyperionIP
      // 
      this.lblHyperionIP.AutoSize = true;
      this.lblHyperionIP.Location = new System.Drawing.Point(13, 59);
      this.lblHyperionIP.Name = "lblHyperionIP";
      this.lblHyperionIP.Size = new System.Drawing.Size(65, 13);
      this.lblHyperionIP.TabIndex = 38;
      this.lblHyperionIP.Text = "Hyperion IP:";
      // 
      // lblHyperionProtoPort
      // 
      this.lblHyperionProtoPort.AutoSize = true;
      this.lblHyperionProtoPort.Location = new System.Drawing.Point(13, 84);
      this.lblHyperionProtoPort.Name = "lblHyperionProtoPort";
      this.lblHyperionProtoPort.Size = new System.Drawing.Size(101, 13);
      this.lblHyperionProtoPort.TabIndex = 39;
      this.lblHyperionProtoPort.Text = "Hyperion Proto port:";
      // 
      // tbHyperionInterval
      // 
      this.tbHyperionInterval.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TestScreenshot.Properties.Settings.Default, "hyperionInterval", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.tbHyperionInterval.Location = new System.Drawing.Point(120, 108);
      this.tbHyperionInterval.Name = "tbHyperionInterval";
      this.tbHyperionInterval.Size = new System.Drawing.Size(59, 20);
      this.tbHyperionInterval.TabIndex = 40;
      this.tbHyperionInterval.Text = global::TestScreenshot.Properties.Settings.Default.hyperionInterval;
      this.tbHyperionInterval.Validating += new System.ComponentModel.CancelEventHandler(this.tbHyperionInterval_Validating);
      // 
      // tbHyperionProtoPort
      // 
      this.tbHyperionProtoPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TestScreenshot.Properties.Settings.Default, "hyperionProtoPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.tbHyperionProtoPort.Location = new System.Drawing.Point(120, 81);
      this.tbHyperionProtoPort.Name = "tbHyperionProtoPort";
      this.tbHyperionProtoPort.Size = new System.Drawing.Size(58, 20);
      this.tbHyperionProtoPort.TabIndex = 37;
      this.tbHyperionProtoPort.Text = global::TestScreenshot.Properties.Settings.Default.hyperionProtoPort;
      this.tbHyperionProtoPort.Validating += new System.ComponentModel.CancelEventHandler(this.tbHyperionProtoPort_Validating);
      // 
      // tbHyperionIP
      // 
      this.tbHyperionIP.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TestScreenshot.Properties.Settings.Default, "hyperionIP", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.tbHyperionIP.Location = new System.Drawing.Point(120, 56);
      this.tbHyperionIP.Name = "tbHyperionIP";
      this.tbHyperionIP.Size = new System.Drawing.Size(58, 20);
      this.tbHyperionIP.TabIndex = 36;
      this.tbHyperionIP.Text = global::TestScreenshot.Properties.Settings.Default.hyperionIP;
      this.tbHyperionIP.Validating += new System.ComponentModel.CancelEventHandler(this.tbHyperionIP_Validating);
      // 
      // cmbFormat
      // 
      this.cmbFormat.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TestScreenshot.Properties.Settings.Default, "screenshotOutputFormat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cmbFormat.FormattingEnabled = true;
      this.cmbFormat.Items.AddRange(new object[] {
            "Bitmap",
            "Jpeg",
            "Png",
            "PixelData"});
      this.cmbFormat.Location = new System.Drawing.Point(69, 288);
      this.cmbFormat.Name = "cmbFormat";
      this.cmbFormat.Size = new System.Drawing.Size(101, 21);
      this.cmbFormat.TabIndex = 34;
      this.cmbFormat.Text = global::TestScreenshot.Properties.Settings.Default.screenshotOutputFormat;
      // 
      // txtResizeHeight
      // 
      this.txtResizeHeight.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TestScreenshot.Properties.Settings.Default, "resizeHeight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.txtResizeHeight.Location = new System.Drawing.Point(331, 223);
      this.txtResizeHeight.Name = "txtResizeHeight";
      this.txtResizeHeight.Size = new System.Drawing.Size(47, 20);
      this.txtResizeHeight.TabIndex = 32;
      this.txtResizeHeight.Text = global::TestScreenshot.Properties.Settings.Default.resizeHeight;
      // 
      // txtResizeWidth
      // 
      this.txtResizeWidth.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TestScreenshot.Properties.Settings.Default, "resizeWidth", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.txtResizeWidth.Location = new System.Drawing.Point(234, 223);
      this.txtResizeWidth.Name = "txtResizeWidth";
      this.txtResizeWidth.Size = new System.Drawing.Size(47, 20);
      this.txtResizeWidth.TabIndex = 31;
      this.txtResizeWidth.Text = global::TestScreenshot.Properties.Settings.Default.resizeWidth;
      // 
      // cbDrawOverlay
      // 
      this.cbDrawOverlay.AutoSize = true;
      this.cbDrawOverlay.Checked = global::TestScreenshot.Properties.Settings.Default.overlayEnabled;
      this.cbDrawOverlay.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbDrawOverlay.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TestScreenshot.Properties.Settings.Default, "overlayEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbDrawOverlay.Location = new System.Drawing.Point(10, 265);
      this.cbDrawOverlay.Name = "cbDrawOverlay";
      this.cbDrawOverlay.Size = new System.Drawing.Size(90, 17);
      this.cbDrawOverlay.TabIndex = 26;
      this.cbDrawOverlay.Text = "Draw Overlay";
      this.cbDrawOverlay.UseVisualStyleBackColor = true;
      // 
      // textBox1
      // 
      this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TestScreenshot.Properties.Settings.Default, "exeName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.textBox1.Location = new System.Drawing.Point(15, 23);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(284, 20);
      this.textBox1.TabIndex = 6;
      this.textBox1.Text = global::TestScreenshot.Properties.Settings.Default.exeName;
      this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
      // 
      // lblHyperionInterval
      // 
      this.lblHyperionInterval.AutoSize = true;
      this.lblHyperionInterval.Location = new System.Drawing.Point(13, 111);
      this.lblHyperionInterval.Name = "lblHyperionInterval";
      this.lblHyperionInterval.Size = new System.Drawing.Size(89, 13);
      this.lblHyperionInterval.TabIndex = 41;
      this.lblHyperionInterval.Text = "Hyperion interval:";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1002, 526);
      this.Controls.Add(this.lblHyperionInterval);
      this.Controls.Add(this.tbHyperionInterval);
      this.Controls.Add(this.lblHyperionProtoPort);
      this.Controls.Add(this.lblHyperionIP);
      this.Controls.Add(this.tbHyperionProtoPort);
      this.Controls.Add(this.tbHyperionIP);
      this.Controls.Add(this.btnStartHyperionMonitor);
      this.Controls.Add(this.cmbFormat);
      this.Controls.Add(this.label12);
      this.Controls.Add(this.txtResizeHeight);
      this.Controls.Add(this.txtResizeWidth);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.label11);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.rbDirect3D10_1);
      this.Controls.Add(this.rbAutodetect);
      this.Controls.Add(this.cbDrawOverlay);
      this.Controls.Add(this.rbDirect3D11);
      this.Controls.Add(this.rbDirect3D10);
      this.Controls.Add(this.rbDirect3D9);
      this.Controls.Add(this.cbAutoGAC);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.txtDebugLog);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.txtCaptureHeight);
      this.Controls.Add(this.txtCaptureWidth);
      this.Controls.Add(this.txtCaptureY);
      this.Controls.Add(this.txtCaptureX);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.progressBar1);
      this.Controls.Add(this.txtNumber);
      this.Controls.Add(this.btnLoadTest);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.btnCapture);
      this.Controls.Add(this.btnInject);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Form1";
      this.Text = "HyperionDX - Direct3D API Hook";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInject;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnLoadTest;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCaptureX;
        private System.Windows.Forms.TextBox txtCaptureY;
        private System.Windows.Forms.TextBox txtCaptureWidth;
        private System.Windows.Forms.TextBox txtCaptureHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDebugLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbAutoGAC;
        private System.Windows.Forms.RadioButton rbDirect3D9;
        private System.Windows.Forms.RadioButton rbDirect3D10;
        private System.Windows.Forms.RadioButton rbDirect3D11;
        private System.Windows.Forms.CheckBox cbDrawOverlay;
        private System.Windows.Forms.RadioButton rbAutodetect;
        private System.Windows.Forms.RadioButton rbDirect3D10_1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtResizeHeight;
        private System.Windows.Forms.TextBox txtResizeWidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmbFormat;
    private System.Windows.Forms.Button btnStartHyperionMonitor;
    private System.Windows.Forms.TextBox tbHyperionIP;
    private System.Windows.Forms.TextBox tbHyperionProtoPort;
    private System.Windows.Forms.Label lblHyperionIP;
    private System.Windows.Forms.Label lblHyperionProtoPort;
    private System.Windows.Forms.TextBox tbHyperionInterval;
    private System.Windows.Forms.Label lblHyperionInterval;
  }
}

