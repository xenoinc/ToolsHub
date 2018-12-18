/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    SampleAssmPreferencesPage.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.SampleAssmAddin
{
  partial class SampleAssmPreferencesPage
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
            this.BtnTest2 = new System.Windows.Forms.Button();
            this.BtnTest1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Random label";
            // 
            // BtnTest2
            // 
            this.BtnTest2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnTest2.Location = new System.Drawing.Point(184, 143);
            this.BtnTest2.Name = "BtnTest2";
            this.BtnTest2.Size = new System.Drawing.Size(108, 23);
            this.BtnTest2.TabIndex = 4;
            this.BtnTest2.Text = "Bottom Right";
            this.BtnTest2.UseVisualStyleBackColor = true;
            // 
            // BtnTest1
            // 
            this.BtnTest1.Location = new System.Drawing.Point(12, 12);
            this.BtnTest1.Name = "BtnTest1";
            this.BtnTest1.Size = new System.Drawing.Size(75, 23);
            this.BtnTest1.TabIndex = 3;
            this.BtnTest1.Text = "Top Left";
            this.BtnTest1.UseVisualStyleBackColor = true;
            // 
            // SampleAssmPreferencesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 178);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnTest2);
            this.Controls.Add(this.BtnTest1);
            this.Name = "SampleAssmPreferencesPage";
            this.Text = "Sample Add-in (External Assm)";
            this.Load += new System.EventHandler(this.SampleAssmPreferencesPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button BtnTest2;
    private System.Windows.Forms.Button BtnTest1;
  }
}