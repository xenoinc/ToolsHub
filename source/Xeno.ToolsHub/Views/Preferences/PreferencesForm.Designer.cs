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
      this.BtnOk = new System.Windows.Forms.Button();
      this.BtnCancel = new System.Windows.Forms.Button();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.TreeAddinList = new System.Windows.Forms.TreeView();
      this.PanelAddinPrefsView = new System.Windows.Forms.Panel();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.SuspendLayout();
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
      // tabControl1
      // 
      this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Location = new System.Drawing.Point(12, 12);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(557, 275);
      this.tabControl1.TabIndex = 4;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.TreeAddinList);
      this.tabPage1.Controls.Add(this.PanelAddinPrefsView);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(549, 249);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Preferences";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // TreeAddinList
      // 
      this.TreeAddinList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.TreeAddinList.Location = new System.Drawing.Point(6, 6);
      this.TreeAddinList.Name = "TreeAddinList";
      this.TreeAddinList.Size = new System.Drawing.Size(161, 237);
      this.TreeAddinList.TabIndex = 5;
      // 
      // PanelAddinPrefsView
      // 
      this.PanelAddinPrefsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PanelAddinPrefsView.Location = new System.Drawing.Point(173, 6);
      this.PanelAddinPrefsView.Name = "PanelAddinPrefsView";
      this.PanelAddinPrefsView.Size = new System.Drawing.Size(373, 237);
      this.PanelAddinPrefsView.TabIndex = 4;
      // 
      // tabPage2
      // 
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(549, 249);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Add-in Manager";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // PreferencesForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(581, 328);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.BtnCancel);
      this.Controls.Add(this.BtnOk);
      this.Name = "PreferencesForm";
      this.Text = "OptionsForm";
      this.Load += new System.EventHandler(this.PreferencesForm_Load);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button BtnOk;
    private System.Windows.Forms.Button BtnCancel;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TreeView TreeAddinList;
    private System.Windows.Forms.Panel PanelAddinPrefsView;
    private System.Windows.Forms.TabPage tabPage2;
  }
}