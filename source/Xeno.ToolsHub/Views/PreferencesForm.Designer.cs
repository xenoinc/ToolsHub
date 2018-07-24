/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    OptionsForm.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.Views
{
  partial class PreferencesForm
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
      this.panel1 = new System.Windows.Forms.Panel();
      this.BtnOk = new System.Windows.Forms.Button();
      this.BtnCancel = new System.Windows.Forms.Button();
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.Location = new System.Drawing.Point(173, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(396, 275);
      this.panel1.TabIndex = 0;
      // 
      // BtnOk
      // 
      this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnOk.Location = new System.Drawing.Point(413, 293);
      this.BtnOk.Name = "BtnOk";
      this.BtnOk.Size = new System.Drawing.Size(75, 23);
      this.BtnOk.TabIndex = 1;
      this.BtnOk.Text = "OK";
      this.BtnOk.UseVisualStyleBackColor = true;
      this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
      // 
      // BtnCancel
      // 
      this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnCancel.Location = new System.Drawing.Point(494, 293);
      this.BtnCancel.Name = "BtnCancel";
      this.BtnCancel.Size = new System.Drawing.Size(75, 23);
      this.BtnCancel.TabIndex = 2;
      this.BtnCancel.Text = "Cancel";
      this.BtnCancel.UseVisualStyleBackColor = true;
      this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
      // 
      // treeView1
      // 
      this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.treeView1.Location = new System.Drawing.Point(6, 12);
      this.treeView1.Name = "treeView1";
      this.treeView1.Size = new System.Drawing.Size(161, 275);
      this.treeView1.TabIndex = 3;
      // 
      // PreferencesForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(581, 328);
      this.Controls.Add(this.treeView1);
      this.Controls.Add(this.BtnCancel);
      this.Controls.Add(this.BtnOk);
      this.Controls.Add(this.panel1);
      this.Name = "PreferencesForm";
      this.Text = "OptionsForm";
      this.Load += new System.EventHandler(this.OptionsForm_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button BtnOk;
    private System.Windows.Forms.Button BtnCancel;
    private System.Windows.Forms.TreeView treeView1;
  }
}