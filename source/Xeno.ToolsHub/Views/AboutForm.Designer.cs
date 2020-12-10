/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-19
 * Author:  Damian Suess
 * File:    AboutForm.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.Views
{
  partial class AboutForm
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
      this.BtnOk = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.linkGitHub = new System.Windows.Forms.LinkLabel();
      this.label3 = new System.Windows.Forms.Label();
      this.linkXenoInc = new System.Windows.Forms.LinkLabel();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // BtnOk
      // 
      this.BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.BtnOk.Location = new System.Drawing.Point(192, 148);
      this.BtnOk.Name = "BtnOk";
      this.BtnOk.Size = new System.Drawing.Size(75, 23);
      this.BtnOk.TabIndex = 0;
      this.BtnOk.Text = "OK";
      this.BtnOk.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(164, 26);
      this.label1.TabIndex = 1;
      this.label1.Text = "ToolsHub v0.1";
      // 
      // linkGitHub
      // 
      this.linkGitHub.AutoSize = true;
      this.linkGitHub.Location = new System.Drawing.Point(16, 112);
      this.linkGitHub.Name = "linkGitHub";
      this.linkGitHub.Size = new System.Drawing.Size(188, 13);
      this.linkGitHub.TabIndex = 2;
      this.linkGitHub.TabStop = true;
      this.linkGitHub.Text = "https://github.com/xenoinc/ToolsHub";
      this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkedLabel_LinkClicked);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 99);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(73, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Open Source:";
      // 
      // linkXenoInc
      // 
      this.linkXenoInc.AutoSize = true;
      this.linkXenoInc.Location = new System.Drawing.Point(16, 64);
      this.linkXenoInc.Name = "linkXenoInc";
      this.linkXenoInc.Size = new System.Drawing.Size(90, 13);
      this.linkXenoInc.TabIndex = 7;
      this.linkXenoInc.TabStop = true;
      this.linkXenoInc.Text = "Xeno Innovations";
      this.linkXenoInc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkedLabel_LinkClicked);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(10, 51);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(96, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Made with love by:";
      // 
      // AboutForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(279, 183);
      this.Controls.Add(this.linkXenoInc);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.linkGitHub);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.BtnOk);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutForm";
      this.Text = "About ToolsHub";
      this.Load += new System.EventHandler(this.AboutForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button BtnOk;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.LinkLabel linkGitHub;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.LinkLabel linkXenoInc;
    private System.Windows.Forms.Label label2;
  }
}