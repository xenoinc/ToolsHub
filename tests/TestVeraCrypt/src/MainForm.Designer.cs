namespace TestVeraCrypt
{
  partial class MainForm
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
      this.btnMount = new System.Windows.Forms.Button();
      this.btnDismount = new System.Windows.Forms.Button();
      this.lstMounts = new System.Windows.Forms.ListBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnRefreshMounts = new System.Windows.Forms.Button();
      this.txtPath = new System.Windows.Forms.TextBox();
      this.btnFindInstall = new System.Windows.Forms.Button();
      this.txtDriveLetter = new System.Windows.Forms.TextBox();
      this.chkForce = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // btnMount
      // 
      this.btnMount.Location = new System.Drawing.Point(3, 0);
      this.btnMount.Name = "btnMount";
      this.btnMount.Size = new System.Drawing.Size(75, 23);
      this.btnMount.TabIndex = 0;
      this.btnMount.Text = "Mount";
      this.btnMount.UseVisualStyleBackColor = true;
      this.btnMount.Click += new System.EventHandler(this.btnMount_Click);
      // 
      // btnDismount
      // 
      this.btnDismount.Location = new System.Drawing.Point(84, 0);
      this.btnDismount.Name = "btnDismount";
      this.btnDismount.Size = new System.Drawing.Size(75, 23);
      this.btnDismount.TabIndex = 1;
      this.btnDismount.Text = "Dismount";
      this.btnDismount.UseVisualStyleBackColor = true;
      this.btnDismount.Click += new System.EventHandler(this.btnDismount_Click);
      // 
      // lstMounts
      // 
      this.lstMounts.FormattingEnabled = true;
      this.lstMounts.Location = new System.Drawing.Point(12, 81);
      this.lstMounts.Name = "lstMounts";
      this.lstMounts.Size = new System.Drawing.Size(279, 147);
      this.lstMounts.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 65);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(94, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "VeraCrypt Mounts:";
      // 
      // btnRefreshMounts
      // 
      this.btnRefreshMounts.Location = new System.Drawing.Point(216, 55);
      this.btnRefreshMounts.Name = "btnRefreshMounts";
      this.btnRefreshMounts.Size = new System.Drawing.Size(75, 23);
      this.btnRefreshMounts.TabIndex = 4;
      this.btnRefreshMounts.Text = "Refresh";
      this.btnRefreshMounts.UseVisualStyleBackColor = true;
      this.btnRefreshMounts.Click += new System.EventHandler(this.btnRefreshMounts_Click);
      // 
      // txtPath
      // 
      this.txtPath.Location = new System.Drawing.Point(3, 29);
      this.txtPath.Name = "txtPath";
      this.txtPath.Size = new System.Drawing.Size(245, 20);
      this.txtPath.TabIndex = 5;
      this.txtPath.Text = "C:\\Program Files\\VeraCrypt\\VeraCrypt.exe";
      // 
      // btnFindInstall
      // 
      this.btnFindInstall.Location = new System.Drawing.Point(254, 26);
      this.btnFindInstall.Name = "btnFindInstall";
      this.btnFindInstall.Size = new System.Drawing.Size(37, 23);
      this.btnFindInstall.TabIndex = 6;
      this.btnFindInstall.Text = "Find";
      this.btnFindInstall.UseVisualStyleBackColor = true;
      this.btnFindInstall.Click += new System.EventHandler(this.btnFindInstall_Click);
      // 
      // txtDriveLetter
      // 
      this.txtDriveLetter.Location = new System.Drawing.Point(165, 3);
      this.txtDriveLetter.Name = "txtDriveLetter";
      this.txtDriveLetter.Size = new System.Drawing.Size(32, 20);
      this.txtDriveLetter.TabIndex = 7;
      this.txtDriveLetter.Text = "X";
      // 
      // chkForce
      // 
      this.chkForce.AutoSize = true;
      this.chkForce.Location = new System.Drawing.Point(203, 6);
      this.chkForce.Name = "chkForce";
      this.chkForce.Size = new System.Drawing.Size(53, 17);
      this.chkForce.TabIndex = 8;
      this.chkForce.Text = "Force";
      this.chkForce.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(303, 240);
      this.Controls.Add(this.chkForce);
      this.Controls.Add(this.txtDriveLetter);
      this.Controls.Add(this.btnFindInstall);
      this.Controls.Add(this.txtPath);
      this.Controls.Add(this.btnRefreshMounts);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lstMounts);
      this.Controls.Add(this.btnDismount);
      this.Controls.Add(this.btnMount);
      this.Name = "MainForm";
      this.Text = "MainForm";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnMount;
    private System.Windows.Forms.Button btnDismount;
    private System.Windows.Forms.ListBox lstMounts;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnRefreshMounts;
    private System.Windows.Forms.TextBox txtPath;
    private System.Windows.Forms.Button btnFindInstall;
    private System.Windows.Forms.TextBox txtDriveLetter;
    private System.Windows.Forms.CheckBox chkForce;
  }
}

