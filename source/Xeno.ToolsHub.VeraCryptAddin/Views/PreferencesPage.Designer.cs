namespace Xeno.ToolsHub.VeraCryptAddin.Views
{
  partial class PreferencesPage
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
      flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      groupBox3 = new System.Windows.Forms.GroupBox();
      lblInstallFound = new System.Windows.Forms.Label();
      lblCipherTest = new System.Windows.Forms.Label();
      BtnTestCipher = new System.Windows.Forms.Button();
      BtnInstallAutoFind = new System.Windows.Forms.Button();
      BtnInstallManualFind = new System.Windows.Forms.Button();
      TxtInstallPath = new System.Windows.Forms.TextBox();
      label4 = new System.Windows.Forms.Label();
      ChkOnSignoutDismount = new System.Windows.Forms.CheckBox();
      ChkForceDismounts = new System.Windows.Forms.CheckBox();
      ChkOnShutdownDismount = new System.Windows.Forms.CheckBox();
      ChkOnStartMount = new System.Windows.Forms.CheckBox();
      ChkOnExitDismount = new System.Windows.Forms.CheckBox();
      groupBox2 = new System.Windows.Forms.GroupBox();
      CmboDrives = new System.Windows.Forms.ComboBox();
      BtnHcPath = new System.Windows.Forms.Button();
      TxtHcPath = new System.Windows.Forms.TextBox();
      label1 = new System.Windows.Forms.Label();
      TxtHcPass = new System.Windows.Forms.TextBox();
      label2 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      flowLayoutPanel1.SuspendLayout();
      groupBox3.SuspendLayout();
      groupBox2.SuspendLayout();
      SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.Controls.Add(groupBox3);
      flowLayoutPanel1.Controls.Add(groupBox2);
      flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new System.Drawing.Size(456, 368);
      flowLayoutPanel1.TabIndex = 2;
      // 
      // groupBox3
      // 
      groupBox3.Controls.Add(lblInstallFound);
      groupBox3.Controls.Add(lblCipherTest);
      groupBox3.Controls.Add(BtnTestCipher);
      groupBox3.Controls.Add(BtnInstallAutoFind);
      groupBox3.Controls.Add(BtnInstallManualFind);
      groupBox3.Controls.Add(TxtInstallPath);
      groupBox3.Controls.Add(label4);
      groupBox3.Controls.Add(ChkOnSignoutDismount);
      groupBox3.Controls.Add(ChkForceDismounts);
      groupBox3.Controls.Add(ChkOnShutdownDismount);
      groupBox3.Controls.Add(ChkOnStartMount);
      groupBox3.Controls.Add(ChkOnExitDismount);
      groupBox3.Location = new System.Drawing.Point(4, 3);
      groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox3.Name = "groupBox3";
      groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox3.Size = new System.Drawing.Size(439, 198);
      groupBox3.TabIndex = 1;
      groupBox3.TabStop = false;
      groupBox3.Text = "VeraCrypt Settings";
      // 
      // lblInstallFound
      // 
      lblInstallFound.AutoSize = true;
      lblInstallFound.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
      lblInstallFound.ForeColor = System.Drawing.SystemColors.Highlight;
      lblInstallFound.Location = new System.Drawing.Point(117, 171);
      lblInstallFound.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      lblInstallFound.Name = "lblInstallFound";
      lblInstallFound.Size = new System.Drawing.Size(136, 15);
      lblInstallFound.TabIndex = 18;
      lblInstallFound.Text = "VeraCrypt Install Found!";
      lblInstallFound.Visible = false;
      // 
      // lblCipherTest
      // 
      lblCipherTest.AutoSize = true;
      lblCipherTest.Location = new System.Drawing.Point(299, 105);
      lblCipherTest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      lblCipherTest.Name = "lblCipherTest";
      lblCipherTest.Size = new System.Drawing.Size(87, 15);
      lblCipherTest.TabIndex = 17;
      lblCipherTest.Text = "Test Status: n/a";
      // 
      // BtnTestCipher
      // 
      BtnTestCipher.Location = new System.Drawing.Point(301, 75);
      BtnTestCipher.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      BtnTestCipher.Name = "BtnTestCipher";
      BtnTestCipher.Size = new System.Drawing.Size(88, 27);
      BtnTestCipher.TabIndex = 16;
      BtnTestCipher.Text = "Test Cipher";
      BtnTestCipher.UseVisualStyleBackColor = true;
      BtnTestCipher.Click += BtnTestCipher_Click;
      // 
      // BtnInstallAutoFind
      // 
      BtnInstallAutoFind.Location = new System.Drawing.Point(10, 165);
      BtnInstallAutoFind.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      BtnInstallAutoFind.Name = "BtnInstallAutoFind";
      BtnInstallAutoFind.Size = new System.Drawing.Size(99, 27);
      BtnInstallAutoFind.TabIndex = 15;
      BtnInstallAutoFind.Text = "Auto-discover";
      BtnInstallAutoFind.UseVisualStyleBackColor = true;
      BtnInstallAutoFind.Click += BtnInstallAutoFind_Click;
      // 
      // BtnInstallManualFind
      // 
      BtnInstallManualFind.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
      BtnInstallManualFind.Location = new System.Drawing.Point(396, 136);
      BtnInstallManualFind.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      BtnInstallManualFind.Name = "BtnInstallManualFind";
      BtnInstallManualFind.Size = new System.Drawing.Size(36, 23);
      BtnInstallManualFind.TabIndex = 14;
      BtnInstallManualFind.Text = "...";
      BtnInstallManualFind.UseVisualStyleBackColor = true;
      BtnInstallManualFind.Click += BtnInstallManualFind_Click;
      // 
      // TxtInstallPath
      // 
      TxtInstallPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      TxtInstallPath.Location = new System.Drawing.Point(10, 136);
      TxtInstallPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TxtInstallPath.Name = "TxtInstallPath";
      TxtInstallPath.Size = new System.Drawing.Size(377, 23);
      TxtInstallPath.TabIndex = 13;
      TxtInstallPath.TextChanged += OnTextChanged;
      TxtInstallPath.KeyPress += OnKeyPress;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point(4, 113);
      label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(122, 15);
      label4.TabIndex = 12;
      label4.Text = "VeraCrypt Install Path:";
      // 
      // ChkOnSignoutDismount
      // 
      ChkOnSignoutDismount.AutoSize = true;
      ChkOnSignoutDismount.Enabled = false;
      ChkOnSignoutDismount.Location = new System.Drawing.Point(139, 48);
      ChkOnSignoutDismount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      ChkOnSignoutDismount.Name = "ChkOnSignoutDismount";
      ChkOnSignoutDismount.Size = new System.Drawing.Size(184, 19);
      ChkOnSignoutDismount.TabIndex = 1;
      ChkOnSignoutDismount.Text = "Auto-dismount all on Signout";
      ChkOnSignoutDismount.UseVisualStyleBackColor = true;
      ChkOnSignoutDismount.CheckedChanged += Check_CheckedChanged;
      // 
      // ChkForceDismounts
      // 
      ChkForceDismounts.AutoSize = true;
      ChkForceDismounts.Location = new System.Drawing.Point(7, 75);
      ChkForceDismounts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      ChkForceDismounts.Name = "ChkForceDismounts";
      ChkForceDismounts.Size = new System.Drawing.Size(115, 19);
      ChkForceDismounts.TabIndex = 11;
      ChkForceDismounts.Text = "Force Dismounts";
      ChkForceDismounts.UseVisualStyleBackColor = true;
      ChkForceDismounts.CheckedChanged += Check_CheckedChanged;
      // 
      // ChkOnShutdownDismount
      // 
      ChkOnShutdownDismount.AutoSize = true;
      ChkOnShutdownDismount.Enabled = false;
      ChkOnShutdownDismount.Location = new System.Drawing.Point(139, 22);
      ChkOnShutdownDismount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      ChkOnShutdownDismount.Name = "ChkOnShutdownDismount";
      ChkOnShutdownDismount.Size = new System.Drawing.Size(197, 19);
      ChkOnShutdownDismount.TabIndex = 0;
      ChkOnShutdownDismount.Text = "Auto-dismount all on Shutdown";
      ChkOnShutdownDismount.UseVisualStyleBackColor = true;
      ChkOnShutdownDismount.CheckedChanged += Check_CheckedChanged;
      // 
      // ChkOnStartMount
      // 
      ChkOnStartMount.AutoSize = true;
      ChkOnStartMount.Enabled = false;
      ChkOnStartMount.Location = new System.Drawing.Point(7, 22);
      ChkOnStartMount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      ChkOnStartMount.Name = "ChkOnStartMount";
      ChkOnStartMount.Size = new System.Drawing.Size(105, 19);
      ChkOnStartMount.TabIndex = 8;
      ChkOnStartMount.Text = "OnStart Mount";
      ChkOnStartMount.UseVisualStyleBackColor = true;
      ChkOnStartMount.CheckedChanged += Check_CheckedChanged;
      // 
      // ChkOnExitDismount
      // 
      ChkOnExitDismount.AutoSize = true;
      ChkOnExitDismount.Enabled = false;
      ChkOnExitDismount.Location = new System.Drawing.Point(7, 48);
      ChkOnExitDismount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      ChkOnExitDismount.Name = "ChkOnExitDismount";
      ChkOnExitDismount.Size = new System.Drawing.Size(115, 19);
      ChkOnExitDismount.TabIndex = 9;
      ChkOnExitDismount.Text = "OnExit Dismount";
      ChkOnExitDismount.UseVisualStyleBackColor = true;
      ChkOnExitDismount.CheckedChanged += Check_CheckedChanged;
      // 
      // groupBox2
      // 
      groupBox2.Controls.Add(CmboDrives);
      groupBox2.Controls.Add(BtnHcPath);
      groupBox2.Controls.Add(TxtHcPath);
      groupBox2.Controls.Add(label1);
      groupBox2.Controls.Add(TxtHcPass);
      groupBox2.Controls.Add(label2);
      groupBox2.Controls.Add(label3);
      groupBox2.Location = new System.Drawing.Point(4, 207);
      groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox2.Name = "groupBox2";
      groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      groupBox2.Size = new System.Drawing.Size(439, 149);
      groupBox2.TabIndex = 0;
      groupBox2.TabStop = false;
      groupBox2.Text = "Available Volumes";
      // 
      // CmboDrives
      // 
      CmboDrives.FormattingEnabled = true;
      CmboDrives.Location = new System.Drawing.Point(7, 36);
      CmboDrives.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      CmboDrives.MaxLength = 1;
      CmboDrives.Name = "CmboDrives";
      CmboDrives.Size = new System.Drawing.Size(58, 23);
      CmboDrives.TabIndex = 16;
      CmboDrives.TextChanged += OnTextChanged;
      // 
      // BtnHcPath
      // 
      BtnHcPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
      BtnHcPath.Location = new System.Drawing.Point(396, 36);
      BtnHcPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      BtnHcPath.Name = "BtnHcPath";
      BtnHcPath.Size = new System.Drawing.Size(36, 23);
      BtnHcPath.TabIndex = 15;
      BtnHcPath.Text = "...";
      BtnHcPath.UseVisualStyleBackColor = true;
      BtnHcPath.Click += BtnHcPath_Click;
      // 
      // TxtHcPath
      // 
      TxtHcPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      TxtHcPath.Location = new System.Drawing.Point(72, 37);
      TxtHcPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TxtHcPath.Name = "TxtHcPath";
      TxtHcPath.Size = new System.Drawing.Size(316, 23);
      TxtHcPath.TabIndex = 10;
      TxtHcPath.TextChanged += OnTextChanged;
      TxtHcPath.KeyPress += OnKeyPress;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(7, 18);
      label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(37, 15);
      label1.TabIndex = 4;
      label1.Text = "Drive:";
      // 
      // TxtHcPass
      // 
      TxtHcPass.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      TxtHcPass.Location = new System.Drawing.Point(7, 82);
      TxtHcPass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TxtHcPass.Name = "TxtHcPass";
      TxtHcPass.PasswordChar = '*';
      TxtHcPass.Size = new System.Drawing.Size(424, 23);
      TxtHcPass.TabIndex = 7;
      TxtHcPass.TextChanged += OnTextChanged;
      TxtHcPass.KeyPress += OnKeyPress;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(75, 18);
      label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(54, 15);
      label2.TabIndex = 5;
      label2.Text = "HC Path:";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(7, 63);
      label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(100, 15);
      label3.TabIndex = 6;
      label3.Text = "Volume Password";
      // 
      // PreferencesPage
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(456, 368);
      Controls.Add(flowLayoutPanel1);
      Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      Name = "PreferencesPage";
      Text = "PreferencesPage";
      Load += PreferencesPage_Load;
      flowLayoutPanel1.ResumeLayout(false);
      groupBox3.ResumeLayout(false);
      groupBox3.PerformLayout();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox ChkOnShutdownDismount;
    private System.Windows.Forms.CheckBox ChkOnSignoutDismount;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox TxtHcPass;
    private System.Windows.Forms.CheckBox ChkOnStartMount;
    private System.Windows.Forms.CheckBox ChkOnExitDismount;
    private System.Windows.Forms.TextBox TxtHcPath;
    private System.Windows.Forms.CheckBox ChkForceDismounts;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox TxtInstallPath;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button BtnInstallManualFind;
    private System.Windows.Forms.Button BtnInstallAutoFind;
    private System.Windows.Forms.Button BtnHcPath;
    private System.Windows.Forms.ComboBox CmboDrives;
    private System.Windows.Forms.Button BtnTestCipher;
    private System.Windows.Forms.Label lblCipherTest;
    private System.Windows.Forms.Label lblInstallFound;
  }
}