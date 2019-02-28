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
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.BtnInstallAutoFind = new System.Windows.Forms.Button();
      this.BtnInstallManualFind = new System.Windows.Forms.Button();
      this.TxtInstallPath = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.ChkOnSignoutDismount = new System.Windows.Forms.CheckBox();
      this.ChkForceDismounts = new System.Windows.Forms.CheckBox();
      this.ChkOnShutdownDismount = new System.Windows.Forms.CheckBox();
      this.ChkOnStartMount = new System.Windows.Forms.CheckBox();
      this.ChkOnExitDismount = new System.Windows.Forms.CheckBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.BtnHcPath = new System.Windows.Forms.Button();
      this.TxtHcDrive = new System.Windows.Forms.TextBox();
      this.TxtHcPath = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.TxtHcPass = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.flowLayoutPanel1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.groupBox3);
      this.flowLayoutPanel1.Controls.Add(this.groupBox2);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(391, 319);
      this.flowLayoutPanel1.TabIndex = 2;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.BtnInstallAutoFind);
      this.groupBox3.Controls.Add(this.BtnInstallManualFind);
      this.groupBox3.Controls.Add(this.TxtInstallPath);
      this.groupBox3.Controls.Add(this.label4);
      this.groupBox3.Controls.Add(this.ChkOnSignoutDismount);
      this.groupBox3.Controls.Add(this.ChkForceDismounts);
      this.groupBox3.Controls.Add(this.ChkOnShutdownDismount);
      this.groupBox3.Controls.Add(this.ChkOnStartMount);
      this.groupBox3.Controls.Add(this.ChkOnExitDismount);
      this.groupBox3.Location = new System.Drawing.Point(3, 3);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(376, 172);
      this.groupBox3.TabIndex = 1;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "VeraCrypt Settings";
      // 
      // BtnInstallAutoFind
      // 
      this.BtnInstallAutoFind.Enabled = false;
      this.BtnInstallAutoFind.Location = new System.Drawing.Point(9, 143);
      this.BtnInstallAutoFind.Name = "BtnInstallAutoFind";
      this.BtnInstallAutoFind.Size = new System.Drawing.Size(85, 23);
      this.BtnInstallAutoFind.TabIndex = 15;
      this.BtnInstallAutoFind.Text = "Auto-discover";
      this.BtnInstallAutoFind.UseVisualStyleBackColor = true;
      this.BtnInstallAutoFind.Click += new System.EventHandler(this.BtnInstallAutoFind_Click);
      // 
      // BtnInstallManualFind
      // 
      this.BtnInstallManualFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnInstallManualFind.Enabled = false;
      this.BtnInstallManualFind.Location = new System.Drawing.Point(339, 118);
      this.BtnInstallManualFind.Name = "BtnInstallManualFind";
      this.BtnInstallManualFind.Size = new System.Drawing.Size(31, 20);
      this.BtnInstallManualFind.TabIndex = 14;
      this.BtnInstallManualFind.Text = "...";
      this.BtnInstallManualFind.UseVisualStyleBackColor = true;
      this.BtnInstallManualFind.Click += new System.EventHandler(this.BtnInstallManualFind_Click);
      // 
      // TxtInstallPath
      // 
      this.TxtInstallPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtInstallPath.Location = new System.Drawing.Point(9, 118);
      this.TxtInstallPath.Name = "TxtInstallPath";
      this.TxtInstallPath.Size = new System.Drawing.Size(324, 20);
      this.TxtInstallPath.TabIndex = 13;
      this.TxtInstallPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(3, 98);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(111, 13);
      this.label4.TabIndex = 12;
      this.label4.Text = "VeraCrypt Install Path:";
      // 
      // ChkOnSignoutDismount
      // 
      this.ChkOnSignoutDismount.AutoSize = true;
      this.ChkOnSignoutDismount.Enabled = false;
      this.ChkOnSignoutDismount.Location = new System.Drawing.Point(119, 42);
      this.ChkOnSignoutDismount.Name = "ChkOnSignoutDismount";
      this.ChkOnSignoutDismount.Size = new System.Drawing.Size(160, 17);
      this.ChkOnSignoutDismount.TabIndex = 1;
      this.ChkOnSignoutDismount.Text = "Auto-dismount all on Signout";
      this.ChkOnSignoutDismount.UseVisualStyleBackColor = true;
      this.ChkOnSignoutDismount.CheckedChanged += new System.EventHandler(this.Check_CheckedChanged);
      // 
      // ChkForceDismounts
      // 
      this.ChkForceDismounts.AutoSize = true;
      this.ChkForceDismounts.Location = new System.Drawing.Point(6, 65);
      this.ChkForceDismounts.Name = "ChkForceDismounts";
      this.ChkForceDismounts.Size = new System.Drawing.Size(105, 17);
      this.ChkForceDismounts.TabIndex = 11;
      this.ChkForceDismounts.Text = "Force Dismounts";
      this.ChkForceDismounts.UseVisualStyleBackColor = true;
      this.ChkForceDismounts.CheckedChanged += new System.EventHandler(this.Check_CheckedChanged);
      // 
      // ChkOnShutdownDismount
      // 
      this.ChkOnShutdownDismount.AutoSize = true;
      this.ChkOnShutdownDismount.Enabled = false;
      this.ChkOnShutdownDismount.Location = new System.Drawing.Point(119, 19);
      this.ChkOnShutdownDismount.Name = "ChkOnShutdownDismount";
      this.ChkOnShutdownDismount.Size = new System.Drawing.Size(172, 17);
      this.ChkOnShutdownDismount.TabIndex = 0;
      this.ChkOnShutdownDismount.Text = "Auto-dismount all on Shutdown";
      this.ChkOnShutdownDismount.UseVisualStyleBackColor = true;
      this.ChkOnShutdownDismount.CheckedChanged += new System.EventHandler(this.Check_CheckedChanged);
      // 
      // ChkOnStartMount
      // 
      this.ChkOnStartMount.AutoSize = true;
      this.ChkOnStartMount.Enabled = false;
      this.ChkOnStartMount.Location = new System.Drawing.Point(6, 19);
      this.ChkOnStartMount.Name = "ChkOnStartMount";
      this.ChkOnStartMount.Size = new System.Drawing.Size(95, 17);
      this.ChkOnStartMount.TabIndex = 8;
      this.ChkOnStartMount.Text = "OnStart Mount";
      this.ChkOnStartMount.UseVisualStyleBackColor = true;
      this.ChkOnStartMount.CheckedChanged += new System.EventHandler(this.Check_CheckedChanged);
      // 
      // ChkOnExitDismount
      // 
      this.ChkOnExitDismount.AutoSize = true;
      this.ChkOnExitDismount.Enabled = false;
      this.ChkOnExitDismount.Location = new System.Drawing.Point(6, 42);
      this.ChkOnExitDismount.Name = "ChkOnExitDismount";
      this.ChkOnExitDismount.Size = new System.Drawing.Size(104, 17);
      this.ChkOnExitDismount.TabIndex = 9;
      this.ChkOnExitDismount.Text = "OnExit Dismount";
      this.ChkOnExitDismount.UseVisualStyleBackColor = true;
      this.ChkOnExitDismount.CheckedChanged += new System.EventHandler(this.Check_CheckedChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.BtnHcPath);
      this.groupBox2.Controls.Add(this.TxtHcDrive);
      this.groupBox2.Controls.Add(this.TxtHcPath);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.TxtHcPass);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Location = new System.Drawing.Point(3, 181);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(376, 104);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Available Volumes";
      // 
      // BtnHcPath
      // 
      this.BtnHcPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnHcPath.Enabled = false;
      this.BtnHcPath.Location = new System.Drawing.Point(339, 31);
      this.BtnHcPath.Name = "BtnHcPath";
      this.BtnHcPath.Size = new System.Drawing.Size(31, 20);
      this.BtnHcPath.TabIndex = 15;
      this.BtnHcPath.Text = "...";
      this.BtnHcPath.UseVisualStyleBackColor = true;
      this.BtnHcPath.Click += new System.EventHandler(this.BtnHcPath_Click);
      // 
      // TxtHcDrive
      // 
      this.TxtHcDrive.Location = new System.Drawing.Point(6, 32);
      this.TxtHcDrive.MaxLength = 1;
      this.TxtHcDrive.Name = "TxtHcDrive";
      this.TxtHcDrive.Size = new System.Drawing.Size(35, 20);
      this.TxtHcDrive.TabIndex = 5;
      this.TxtHcDrive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // TxtHcPath
      // 
      this.TxtHcPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtHcPath.Location = new System.Drawing.Point(50, 32);
      this.TxtHcPath.Name = "TxtHcPath";
      this.TxtHcPath.Size = new System.Drawing.Size(283, 20);
      this.TxtHcPath.TabIndex = 10;
      this.TxtHcPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Drive:";
      // 
      // TxtHcPass
      // 
      this.TxtHcPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtHcPass.Location = new System.Drawing.Point(6, 71);
      this.TxtHcPass.Name = "TxtHcPass";
      this.TxtHcPass.PasswordChar = '*';
      this.TxtHcPass.Size = new System.Drawing.Size(364, 20);
      this.TxtHcPass.TabIndex = 7;
      this.TxtHcPass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(47, 16);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(50, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "HC Path:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 55);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(91, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Volume Password";
      // 
      // PreferencesPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(391, 319);
      this.Controls.Add(this.flowLayoutPanel1);
      this.Name = "PreferencesPage";
      this.Text = "PreferencesPage";
      this.Load += new System.EventHandler(this.PreferencesPage_Load);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

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
    private System.Windows.Forms.TextBox TxtHcDrive;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox TxtInstallPath;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button BtnInstallManualFind;
    private System.Windows.Forms.Button BtnInstallAutoFind;
    private System.Windows.Forms.Button BtnHcPath;
  }
}