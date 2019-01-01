/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PreferencePage.Designer.cs
 * Description:
 *  
 */

namespace PomodoroAddin.Views
{
  partial class PreferencePage
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.TxtDuration = new System.Windows.Forms.TextBox();
      this.TxtBreakShort = new System.Windows.Forms.TextBox();
      this.TxtBreakLong = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(89, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Duration (def=25)";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(63, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Short Break";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 67);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(62, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Long Break";
      // 
      // TxtDuration
      // 
      this.TxtDuration.Location = new System.Drawing.Point(107, 12);
      this.TxtDuration.Name = "TxtDuration";
      this.TxtDuration.Size = new System.Drawing.Size(53, 20);
      this.TxtDuration.TabIndex = 3;
      this.TxtDuration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // TxtBreakShort
      // 
      this.TxtBreakShort.Location = new System.Drawing.Point(107, 38);
      this.TxtBreakShort.Name = "TxtBreakShort";
      this.TxtBreakShort.Size = new System.Drawing.Size(53, 20);
      this.TxtBreakShort.TabIndex = 4;
      this.TxtBreakShort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // TxtBreakLong
      // 
      this.TxtBreakLong.Location = new System.Drawing.Point(107, 64);
      this.TxtBreakLong.Name = "TxtBreakLong";
      this.TxtBreakLong.Size = new System.Drawing.Size(53, 20);
      this.TxtBreakLong.TabIndex = 5;
      this.TxtBreakLong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // PreferencePage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 211);
      this.Controls.Add(this.TxtBreakLong);
      this.Controls.Add(this.TxtBreakShort);
      this.Controls.Add(this.TxtDuration);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "PreferencePage";
      this.Text = "Pomodoro Timer";
      this.Load += new System.EventHandler(this.PreferencePage_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox TxtDuration;
    private System.Windows.Forms.TextBox TxtBreakShort;
    private System.Windows.Forms.TextBox TxtBreakLong;
  }
}