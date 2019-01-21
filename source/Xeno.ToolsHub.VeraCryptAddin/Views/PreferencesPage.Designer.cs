namespace Xeno.ToolsHub.VeraCryptAddin.views
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
            this.chkDismountSignout = new System.Windows.Forms.CheckBox();
            this.chkDismountShutdown = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDismountSignout);
            this.groupBox1.Controls.Add(this.chkDismountShutdown);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 96);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "VeraCrypt Preferences";
            // 
            // chkDismountSignout
            // 
            this.chkDismountSignout.AutoSize = true;
            this.chkDismountSignout.Location = new System.Drawing.Point(6, 42);
            this.chkDismountSignout.Name = "chkDismountSignout";
            this.chkDismountSignout.Size = new System.Drawing.Size(160, 17);
            this.chkDismountSignout.TabIndex = 1;
            this.chkDismountSignout.Text = "Auto-dismount all on Signout";
            this.chkDismountSignout.UseVisualStyleBackColor = true;
            // 
            // chkDismountShutdown
            // 
            this.chkDismountShutdown.AutoSize = true;
            this.chkDismountShutdown.Location = new System.Drawing.Point(6, 19);
            this.chkDismountShutdown.Name = "chkDismountShutdown";
            this.chkDismountShutdown.Size = new System.Drawing.Size(172, 17);
            this.chkDismountShutdown.TabIndex = 0;
            this.chkDismountShutdown.Text = "Auto-dismount all on Shutdown";
            this.chkDismountShutdown.UseVisualStyleBackColor = true;
            // 
            // PreferencesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 142);
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
    private System.Windows.Forms.CheckBox chkDismountSignout;
    private System.Windows.Forms.CheckBox chkDismountShutdown;
  }
}