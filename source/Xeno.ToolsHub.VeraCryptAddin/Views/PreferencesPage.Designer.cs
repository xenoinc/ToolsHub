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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.TxtHcPath = new System.Windows.Forms.TextBox();
      this.ChkOnExitDismount = new System.Windows.Forms.CheckBox();
      this.ChkOnStartMount = new System.Windows.Forms.CheckBox();
      this.TxtHcPass = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.TxtHcDrive = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.ChkOnSignoutDismount = new System.Windows.Forms.CheckBox();
      this.ChkOnShutdownDismount = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.TxtHcPath);
      this.groupBox1.Controls.Add(this.ChkOnExitDismount);
      this.groupBox1.Controls.Add(this.ChkOnStartMount);
      this.groupBox1.Controls.Add(this.TxtHcPass);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.TxtHcDrive);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.ChkOnSignoutDismount);
      this.groupBox1.Controls.Add(this.ChkOnShutdownDismount);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(424, 171);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "VeraCrypt Preferences";
      // 
      // TxtHcPath
      // 
      this.TxtHcPath.Location = new System.Drawing.Point(50, 92);
      this.TxtHcPath.Name = "TxtHcPath";
      this.TxtHcPath.Size = new System.Drawing.Size(368, 20);
      this.TxtHcPath.TabIndex = 10;
      this.TxtHcPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // ChkOnExitDismount
      // 
      this.ChkOnExitDismount.AutoSize = true;
      this.ChkOnExitDismount.Location = new System.Drawing.Point(6, 42);
      this.ChkOnExitDismount.Name = "ChkOnExitDismount";
      this.ChkOnExitDismount.Size = new System.Drawing.Size(104, 17);
      this.ChkOnExitDismount.TabIndex = 9;
      this.ChkOnExitDismount.Text = "OnExit Dismount";
      this.ChkOnExitDismount.UseVisualStyleBackColor = true;
      this.ChkOnExitDismount.CheckedChanged += new System.EventHandler(this.ChkOnExitDismount_CheckedChanged);
      // 
      // ChkOnStartMount
      // 
      this.ChkOnStartMount.AutoSize = true;
      this.ChkOnStartMount.Location = new System.Drawing.Point(6, 19);
      this.ChkOnStartMount.Name = "ChkOnStartMount";
      this.ChkOnStartMount.Size = new System.Drawing.Size(95, 17);
      this.ChkOnStartMount.TabIndex = 8;
      this.ChkOnStartMount.Text = "OnStart Mount";
      this.ChkOnStartMount.UseVisualStyleBackColor = true;
      this.ChkOnStartMount.CheckedChanged += new System.EventHandler(this.ChkOnStartMount_CheckedChanged);
      // 
      // TxtHcPass
      // 
      this.TxtHcPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtHcPass.Location = new System.Drawing.Point(6, 131);
      this.TxtHcPass.Name = "TxtHcPass";
      this.TxtHcPass.Size = new System.Drawing.Size(412, 20);
      this.TxtHcPass.TabIndex = 7;
      this.TxtHcPass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 115);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(91, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Volume Password";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(47, 76);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(50, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "HC Path:";
      // 
      // TxtHcDrive
      // 
      this.TxtHcDrive.Location = new System.Drawing.Point(6, 92);
      this.TxtHcDrive.MaxLength = 1;
      this.TxtHcDrive.Name = "TxtHcDrive";
      this.TxtHcDrive.Size = new System.Drawing.Size(35, 20);
      this.TxtHcDrive.TabIndex = 3;
      this.TxtHcDrive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 76);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Drive:";
      // 
      // ChkOnSignoutDismount
      // 
      this.ChkOnSignoutDismount.AutoSize = true;
      this.ChkOnSignoutDismount.Enabled = false;
      this.ChkOnSignoutDismount.Location = new System.Drawing.Point(148, 42);
      this.ChkOnSignoutDismount.Name = "ChkOnSignoutDismount";
      this.ChkOnSignoutDismount.Size = new System.Drawing.Size(160, 17);
      this.ChkOnSignoutDismount.TabIndex = 1;
      this.ChkOnSignoutDismount.Text = "Auto-dismount all on Signout";
      this.ChkOnSignoutDismount.UseVisualStyleBackColor = true;
      this.ChkOnSignoutDismount.CheckedChanged += new System.EventHandler(this.ChkOnSignoutDismount_CheckedChanged);
      // 
      // ChkOnShutdownDismount
      // 
      this.ChkOnShutdownDismount.AutoSize = true;
      this.ChkOnShutdownDismount.Enabled = false;
      this.ChkOnShutdownDismount.Location = new System.Drawing.Point(148, 19);
      this.ChkOnShutdownDismount.Name = "ChkOnShutdownDismount";
      this.ChkOnShutdownDismount.Size = new System.Drawing.Size(172, 17);
      this.ChkOnShutdownDismount.TabIndex = 0;
      this.ChkOnShutdownDismount.Text = "Auto-dismount all on Shutdown";
      this.ChkOnShutdownDismount.UseVisualStyleBackColor = true;
      this.ChkOnShutdownDismount.CheckedChanged += new System.EventHandler(this.ChkOnShutdownDismount_CheckedChanged);
      // 
      // PreferencesPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(448, 195);
      this.Controls.Add(this.groupBox1);
      this.Name = "PreferencesPage";
      this.Text = "PreferencesPage";
      this.Load += new System.EventHandler(this.PreferencesPage_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox ChkOnSignoutDismount;
    private System.Windows.Forms.CheckBox ChkOnShutdownDismount;
    private System.Windows.Forms.TextBox TxtHcPass;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox TxtHcDrive;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox ChkOnExitDismount;
    private System.Windows.Forms.CheckBox ChkOnStartMount;
    private System.Windows.Forms.TextBox TxtHcPath;
  }
}