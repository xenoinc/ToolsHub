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
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.AddinTree = new System.Windows.Forms.TreeView();
      this.PanelAddinPrefsView = new System.Windows.Forms.Panel();
      this.LblPageTitle = new System.Windows.Forms.Label();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // BtnOk
      // 
      this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnOk.Location = new System.Drawing.Point(525, 395);
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
      this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.BtnCancel.Location = new System.Drawing.Point(606, 395);
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
      this.tabControl1.Size = new System.Drawing.Size(669, 377);
      this.tabControl1.TabIndex = 4;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.splitContainer1);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(661, 351);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Preferences";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainer1.Location = new System.Drawing.Point(6, 6);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.AddinTree);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
      this.splitContainer1.Panel2.Controls.Add(this.PanelAddinPrefsView);
      this.splitContainer1.Panel2.Controls.Add(this.LblPageTitle);
      this.splitContainer1.Size = new System.Drawing.Size(649, 339);
      this.splitContainer1.SplitterDistance = 176;
      this.splitContainer1.TabIndex = 6;
      // 
      // AddinTree
      // 
      this.AddinTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.AddinTree.Location = new System.Drawing.Point(3, 3);
      this.AddinTree.Name = "AddinTree";
      this.AddinTree.Size = new System.Drawing.Size(170, 333);
      this.AddinTree.TabIndex = 6;
      this.AddinTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AddinTree_AfterSelect);
      // 
      // PanelAddinPrefsView
      // 
      this.PanelAddinPrefsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PanelAddinPrefsView.Location = new System.Drawing.Point(3, 26);
      this.PanelAddinPrefsView.Name = "PanelAddinPrefsView";
      this.PanelAddinPrefsView.Size = new System.Drawing.Size(463, 310);
      this.PanelAddinPrefsView.TabIndex = 6;
      // 
      // LblPageTitle
      // 
      this.LblPageTitle.AutoSize = true;
      this.LblPageTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LblPageTitle.Location = new System.Drawing.Point(3, 3);
      this.LblPageTitle.Name = "LblPageTitle";
      this.LblPageTitle.Size = new System.Drawing.Size(282, 20);
      this.LblPageTitle.TabIndex = 5;
      this.LblPageTitle.Text = "Select a preference page to begin";
      // 
      // tabPage2
      // 
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(661, 351);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Add-in Manager";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // PreferencesForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.BtnCancel;
      this.ClientSize = new System.Drawing.Size(693, 430);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.BtnCancel);
      this.Controls.Add(this.BtnOk);
      this.Name = "PreferencesForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "Preferences";
      this.Load += new System.EventHandler(this.PreferencesForm_Load);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button BtnOk;
    private System.Windows.Forms.Button BtnCancel;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TreeView AddinTree;
    private System.Windows.Forms.Label LblPageTitle;
    private System.Windows.Forms.Panel PanelAddinPrefsView;
  }
}