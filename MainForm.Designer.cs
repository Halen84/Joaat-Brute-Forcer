namespace JoaatBruteForcer
{
	partial class MainForm
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
			components = new System.ComponentModel.Container();
			bruteForceprogressBar = new ProgressBar();
			btnCancel = new Button();
			btnStartPauseResume = new Button();
			toolTip1 = new ToolTip(components);
			rbHexAndDec = new RadioButton();
			rbDec = new RadioButton();
			rbHex = new RadioButton();
			rbStringOnly = new RadioButton();
			tbFormat = new TextBox();
			cbStringHashMode = new CheckBox();
			rbCustom = new RadioButton();
			label1 = new Label();
			gbHashOptions = new GroupBox();
			gbOutput = new GroupBox();
			tbCustomOutput = new TextBox();
			openFileDialog1 = new OpenFileDialog();
			btnSaveToFile = new Button();
			lblPercent = new Label();
			splitContainer1 = new SplitContainer();
			tbHashList = new TextBox();
			tbOutput = new TextBox();
			lblOutput = new Label();
			lblHashList = new Label();
			lblTimeRemaining = new Label();
			cbUnsigned = new CheckBox();
			gbHashOptions.SuspendLayout();
			gbOutput.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			SuspendLayout();
			// 
			// bruteForceprogressBar
			// 
			bruteForceprogressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			bruteForceprogressBar.BackColor = SystemColors.Control;
			bruteForceprogressBar.Location = new Point(12, 676);
			bruteForceprogressBar.Name = "bruteForceprogressBar";
			bruteForceprogressBar.Size = new Size(558, 23);
			bruteForceprogressBar.Step = 1;
			bruteForceprogressBar.TabIndex = 0;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnCancel.Location = new Point(657, 676);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(75, 23);
			btnCancel.TabIndex = 1;
			btnCancel.Text = "Cancel";
			toolTip1.SetToolTip(btnCancel, "Cancel/Abort the brute force");
			btnCancel.UseVisualStyleBackColor = true;
			btnCancel.Click += btnCancel_Click;
			// 
			// btnStartPauseResume
			// 
			btnStartPauseResume.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnStartPauseResume.Location = new Point(576, 676);
			btnStartPauseResume.Name = "btnStartPauseResume";
			btnStartPauseResume.Size = new Size(75, 23);
			btnStartPauseResume.TabIndex = 2;
			btnStartPauseResume.Text = "Start";
			toolTip1.SetToolTip(btnStartPauseResume, "Start/Pause/Resume the brute force");
			btnStartPauseResume.UseVisualStyleBackColor = true;
			btnStartPauseResume.Click += btnStartPauseResume_Click;
			// 
			// rbHexAndDec
			// 
			rbHexAndDec.AutoSize = true;
			rbHexAndDec.Location = new Point(6, 97);
			rbHexAndDec.Name = "rbHexAndDec";
			rbHexAndDec.Size = new Size(163, 19);
			rbHexAndDec.TabIndex = 4;
			rbHexAndDec.TabStop = true;
			rbHexAndDec.Text = "Hexadecimal and Decimal";
			toolTip1.SetToolTip(rbHexAndDec, "Output the matched string in both hexadecimal and decimal format\r\nBrute Force Mode: example_hash = 0x2C9A0702 (748291842)\r\nString Hash Mode: 0x2C9A0702 (748291842)\r\n\r\n");
			rbHexAndDec.UseVisualStyleBackColor = true;
			rbHexAndDec.CheckedChanged += rbHexAndDec_CheckedChanged;
			// 
			// rbDec
			// 
			rbDec.AutoSize = true;
			rbDec.Location = new Point(6, 72);
			rbDec.Name = "rbDec";
			rbDec.Size = new Size(68, 19);
			rbDec.TabIndex = 3;
			rbDec.TabStop = true;
			rbDec.Text = "Decimal";
			toolTip1.SetToolTip(rbDec, "Output the matched string in decimal format\r\nBrute Force Mode: example_hash = 748291842\r\nString Hash Mode: 748291842\r\n");
			rbDec.UseVisualStyleBackColor = true;
			rbDec.CheckedChanged += rbDec_CheckedChanged;
			// 
			// rbHex
			// 
			rbHex.AutoSize = true;
			rbHex.Location = new Point(6, 47);
			rbHex.Name = "rbHex";
			rbHex.Size = new Size(94, 19);
			rbHex.TabIndex = 2;
			rbHex.TabStop = true;
			rbHex.Text = "Hexadecimal";
			toolTip1.SetToolTip(rbHex, "Output the matched string in hexadecimal format\r\nBrute Force Mode: example_hash = 0x2C9A0702\r\nString Hash Mode: 0x2C9A0702");
			rbHex.UseVisualStyleBackColor = true;
			rbHex.CheckedChanged += rbHex_CheckedChanged;
			// 
			// rbStringOnly
			// 
			rbStringOnly.AutoSize = true;
			rbStringOnly.Location = new Point(6, 22);
			rbStringOnly.Name = "rbStringOnly";
			rbStringOnly.Size = new Size(84, 19);
			rbStringOnly.TabIndex = 1;
			rbStringOnly.TabStop = true;
			rbStringOnly.Text = "String Only";
			toolTip1.SetToolTip(rbStringOnly, "Only output the matched string\r\nBrute Force Mode: example_hash\r\nString Hash Mode: (DISABLED)");
			rbStringOnly.UseVisualStyleBackColor = true;
			rbStringOnly.CheckedChanged += rbStringOnly_CheckedChanged;
			// 
			// tbFormat
			// 
			tbFormat.Location = new Point(12, 225);
			tbFormat.Name = "tbFormat";
			tbFormat.ScrollBars = ScrollBars.Horizontal;
			tbFormat.Size = new Size(352, 23);
			tbFormat.TabIndex = 8;
			toolTip1.SetToolTip(tbFormat, "The format to use to brute force");
			tbFormat.TextChanged += tbFormat_TextChanged;
			// 
			// cbStringHashMode
			// 
			cbStringHashMode.AutoSize = true;
			cbStringHashMode.Location = new Point(6, 22);
			cbStringHashMode.Name = "cbStringHashMode";
			cbStringHashMode.Size = new Size(121, 19);
			cbStringHashMode.TabIndex = 5;
			cbStringHashMode.Text = "String Hash Mode";
			toolTip1.SetToolTip(cbStringHashMode, "Generate JOAAT hashes from strings in the hash list");
			cbStringHashMode.UseVisualStyleBackColor = true;
			cbStringHashMode.CheckedChanged += cbStringHashMode_CheckedChanged;
			// 
			// rbCustom
			// 
			rbCustom.AutoSize = true;
			rbCustom.Location = new Point(6, 122);
			rbCustom.Name = "rbCustom";
			rbCustom.Size = new Size(67, 19);
			rbCustom.TabIndex = 5;
			rbCustom.TabStop = true;
			rbCustom.Text = "Custom";
			toolTip1.SetToolTip(rbCustom, "Create a custom output\r\n\r\n{hash}: The matched string\r\n{hex}: The JOAAT hash as hexadecimal format\r\n{dec}: The JOAAT hash as decimal format");
			rbCustom.UseVisualStyleBackColor = true;
			rbCustom.CheckedChanged += rbCustom_CheckedChanged;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(12, 207);
			label1.Name = "label1";
			label1.Size = new Size(108, 15);
			label1.TabIndex = 16;
			label1.Text = "Brute Force Format";
			toolTip1.SetToolTip(label1, "The format to use to brute force");
			// 
			// gbHashOptions
			// 
			gbHashOptions.Anchor = AnchorStyles.Top;
			gbHashOptions.AutoSize = true;
			gbHashOptions.Controls.Add(cbUnsigned);
			gbHashOptions.Controls.Add(cbStringHashMode);
			gbHashOptions.Location = new Point(135, 12);
			gbHashOptions.Name = "gbHashOptions";
			gbHashOptions.Size = new Size(227, 192);
			gbHashOptions.TabIndex = 4;
			gbHashOptions.TabStop = false;
			gbHashOptions.Text = "Options";
			// 
			// gbOutput
			// 
			gbOutput.Anchor = AnchorStyles.Top;
			gbOutput.AutoSize = true;
			gbOutput.Controls.Add(tbCustomOutput);
			gbOutput.Controls.Add(rbCustom);
			gbOutput.Controls.Add(rbHexAndDec);
			gbOutput.Controls.Add(rbDec);
			gbOutput.Controls.Add(rbHex);
			gbOutput.Controls.Add(rbStringOnly);
			gbOutput.Location = new Point(382, 12);
			gbOutput.Name = "gbOutput";
			gbOutput.Size = new Size(227, 192);
			gbOutput.TabIndex = 6;
			gbOutput.TabStop = false;
			gbOutput.Text = "Output";
			// 
			// tbCustomOutput
			// 
			tbCustomOutput.Enabled = false;
			tbCustomOutput.Location = new Point(27, 147);
			tbCustomOutput.Name = "tbCustomOutput";
			tbCustomOutput.PlaceholderText = "{hash} = {hex} ({dec})";
			tbCustomOutput.Size = new Size(170, 23);
			tbCustomOutput.TabIndex = 6;
			tbCustomOutput.WordWrap = false;
			// 
			// openFileDialog1
			// 
			openFileDialog1.FileName = "openFileDialog1";
			// 
			// btnSaveToFile
			// 
			btnSaveToFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnSaveToFile.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			btnSaveToFile.Location = new Point(612, 251);
			btnSaveToFile.Name = "btnSaveToFile";
			btnSaveToFile.Size = new Size(120, 25);
			btnSaveToFile.TabIndex = 11;
			btnSaveToFile.Text = "Save Output To File";
			btnSaveToFile.UseVisualStyleBackColor = true;
			btnSaveToFile.Click += btnSaveToFile_Click;
			// 
			// lblPercent
			// 
			lblPercent.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			lblPercent.AutoSize = true;
			lblPercent.BackColor = Color.FromArgb(232, 228, 228);
			lblPercent.CausesValidation = false;
			lblPercent.Location = new Point(16, 680);
			lblPercent.Name = "lblPercent";
			lblPercent.Size = new Size(38, 15);
			lblPercent.TabIndex = 12;
			lblPercent.Text = "0.00%";
			// 
			// splitContainer1
			// 
			splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			splitContainer1.Location = new Point(12, 282);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(tbHashList);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(tbOutput);
			splitContainer1.Size = new Size(720, 388);
			splitContainer1.SplitterDistance = 356;
			splitContainer1.TabIndex = 13;
			// 
			// tbHashList
			// 
			tbHashList.Dock = DockStyle.Fill;
			tbHashList.Location = new Point(0, 0);
			tbHashList.MaxLength = 0;
			tbHashList.Multiline = true;
			tbHashList.Name = "tbHashList";
			tbHashList.ScrollBars = ScrollBars.Both;
			tbHashList.Size = new Size(356, 388);
			tbHashList.TabIndex = 0;
			tbHashList.WordWrap = false;
			tbHashList.TextChanged += tbHashList_TextChanged;
			// 
			// tbOutput
			// 
			tbOutput.BackColor = SystemColors.Window;
			tbOutput.Dock = DockStyle.Fill;
			tbOutput.Location = new Point(0, 0);
			tbOutput.MaxLength = 0;
			tbOutput.Multiline = true;
			tbOutput.Name = "tbOutput";
			tbOutput.ReadOnly = true;
			tbOutput.ScrollBars = ScrollBars.Both;
			tbOutput.Size = new Size(360, 388);
			tbOutput.TabIndex = 1;
			tbOutput.WordWrap = false;
			// 
			// lblOutput
			// 
			lblOutput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			lblOutput.AutoSize = true;
			lblOutput.Location = new Point(378, 264);
			lblOutput.Name = "lblOutput";
			lblOutput.Size = new Size(45, 15);
			lblOutput.TabIndex = 15;
			lblOutput.Text = "Output";
			// 
			// lblHashList
			// 
			lblHashList.AutoSize = true;
			lblHashList.Location = new Point(9, 264);
			lblHashList.Name = "lblHashList";
			lblHashList.Size = new Size(72, 15);
			lblHashList.TabIndex = 14;
			lblHashList.Text = "Hash List (1)";
			// 
			// lblTimeRemaining
			// 
			lblTimeRemaining.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			lblTimeRemaining.AutoSize = true;
			lblTimeRemaining.Location = new Point(9, 702);
			lblTimeRemaining.Name = "lblTimeRemaining";
			lblTimeRemaining.Size = new Size(162, 15);
			lblTimeRemaining.TabIndex = 0;
			lblTimeRemaining.Text = "EST Time Remaining: 00:00:00";
			// 
			// cbUnsigned
			// 
			cbUnsigned.AutoSize = true;
			cbUnsigned.Location = new Point(6, 47);
			cbUnsigned.Name = "cbUnsigned";
			cbUnsigned.Size = new Size(163, 19);
			cbUnsigned.TabIndex = 6;
			cbUnsigned.Text = "Unsigned Integers (TODO)";
			cbUnsigned.UseVisualStyleBackColor = true;
			cbUnsigned.Visible = false;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(744, 721);
			Controls.Add(lblTimeRemaining);
			Controls.Add(label1);
			Controls.Add(lblOutput);
			Controls.Add(lblPercent);
			Controls.Add(lblHashList);
			Controls.Add(splitContainer1);
			Controls.Add(btnSaveToFile);
			Controls.Add(tbFormat);
			Controls.Add(gbOutput);
			Controls.Add(gbHashOptions);
			Controls.Add(btnStartPauseResume);
			Controls.Add(btnCancel);
			Controls.Add(bruteForceprogressBar);
			MinimumSize = new Size(760, 760);
			Name = "MainForm";
			Text = "JOAAT Brute Forcer by Tuffy";
			FormClosing += MainForm_FormClosing;
			gbHashOptions.ResumeLayout(false);
			gbHashOptions.PerformLayout();
			gbOutput.ResumeLayout(false);
			gbOutput.PerformLayout();
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel1.PerformLayout();
			splitContainer1.Panel2.ResumeLayout(false);
			splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private GroupBox gbHashOptions;
		private Button btnCancel;
		private ToolTip toolTip1;
		private Button btnStartPauseResume;
		private GroupBox gbOutput;
		private RadioButton rbHex;
		private RadioButton rbStringOnly;
		private RadioButton rbHexAndDec;
		private RadioButton rbDec;
		private TextBox tbFormat;
		private OpenFileDialog openFileDialog1;
		private CheckBox cbStringHashMode;
		private Button btnSaveToFile;
		public ProgressBar bruteForceprogressBar;
		public Label lblPercent;
		private SplitContainer splitContainer1;
		private TextBox tbHashList;
		public TextBox tbOutput;
		private TextBox tbCustomOutput;
		private RadioButton rbCustom;
		private Label label1;
		private Label lblHashList;
		private Label lblOutput;
		public Label lblTimeRemaining;
		private CheckBox cbUnsigned;
	}
}