/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-8-13
 * Author:  Damian Suess
 * File:    VeraCryptPreferencesView.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.VeraCryptAddin
{
  partial class VeraCryptPreferencesView
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

    #region Component Designer generated code

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
      this.groupBox1.Location = new System.Drawing.Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(313, 126);
      this.groupBox1.TabIndex = 0;
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
      this.chkDismountSignout.CheckedChanged += new System.EventHandler(this.chkDismountSignout_CheckedChanged);
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
      this.chkDismountShutdown.CheckedChanged += new System.EventHandler(this.chkDismountShutdown_CheckedChanged);
      // 
      // VeraCryptPreferencesView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "VeraCryptPreferencesView";
      this.Size = new System.Drawing.Size(335, 181);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox chkDismountShutdown;
    private System.Windows.Forms.CheckBox chkDismountSignout;
  }
}
