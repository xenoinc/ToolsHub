/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    ShortcutsPreferences.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs
{
  partial class ShortcutsPreferences
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
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.LblModified = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.TxtRawFile = new System.Windows.Forms.TextBox();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.BtnMoveIn = new System.Windows.Forms.Button();
      this.BtnMoveOut = new System.Windows.Forms.Button();
      this.BtnMoveDown = new System.Windows.Forms.Button();
      this.BtnMoveUp = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
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
      this.tabControl1.Size = new System.Drawing.Size(377, 270);
      this.tabControl1.TabIndex = 7;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.LblModified);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Controls.Add(this.TxtRawFile);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(369, 244);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Raw File";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // LblModified
      // 
      this.LblModified.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.LblModified.AutoSize = true;
      this.LblModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LblModified.ForeColor = System.Drawing.SystemColors.HotTrack;
      this.LblModified.Location = new System.Drawing.Point(302, 3);
      this.LblModified.Name = "LblModified";
      this.LblModified.Size = new System.Drawing.Size(47, 13);
      this.LblModified.TabIndex = 2;
      this.LblModified.Text = "Modified";
      this.LblModified.Visible = false;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 3);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(105, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Shortcuts JSON File:";
      // 
      // TxtRawFile
      // 
      this.TxtRawFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtRawFile.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtRawFile.Location = new System.Drawing.Point(6, 19);
      this.TxtRawFile.Multiline = true;
      this.TxtRawFile.Name = "TxtRawFile";
      this.TxtRawFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtRawFile.Size = new System.Drawing.Size(357, 219);
      this.TxtRawFile.TabIndex = 0;
      this.TxtRawFile.TextChanged += new System.EventHandler(this.TxtRawFile_TextChanged);
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.BtnMoveIn);
      this.tabPage2.Controls.Add(this.BtnMoveOut);
      this.tabPage2.Controls.Add(this.BtnMoveDown);
      this.tabPage2.Controls.Add(this.BtnMoveUp);
      this.tabPage2.Controls.Add(this.label1);
      this.tabPage2.Controls.Add(this.treeView1);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(369, 244);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Mock Test";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // BtnMoveIn
      // 
      this.BtnMoveIn.Location = new System.Drawing.Point(191, 106);
      this.BtnMoveIn.Name = "BtnMoveIn";
      this.BtnMoveIn.Size = new System.Drawing.Size(47, 23);
      this.BtnMoveIn.TabIndex = 11;
      this.BtnMoveIn.Text = "<<";
      this.BtnMoveIn.UseVisualStyleBackColor = true;
      // 
      // BtnMoveOut
      // 
      this.BtnMoveOut.Location = new System.Drawing.Point(191, 77);
      this.BtnMoveOut.Name = "BtnMoveOut";
      this.BtnMoveOut.Size = new System.Drawing.Size(47, 23);
      this.BtnMoveOut.TabIndex = 10;
      this.BtnMoveOut.Text = ">>";
      this.BtnMoveOut.UseVisualStyleBackColor = true;
      // 
      // BtnMoveDown
      // 
      this.BtnMoveDown.Location = new System.Drawing.Point(191, 48);
      this.BtnMoveDown.Name = "BtnMoveDown";
      this.BtnMoveDown.Size = new System.Drawing.Size(47, 23);
      this.BtnMoveDown.TabIndex = 9;
      this.BtnMoveDown.Text = "Down";
      this.BtnMoveDown.UseVisualStyleBackColor = true;
      // 
      // BtnMoveUp
      // 
      this.BtnMoveUp.Location = new System.Drawing.Point(191, 19);
      this.BtnMoveUp.Name = "BtnMoveUp";
      this.BtnMoveUp.Size = new System.Drawing.Size(47, 23);
      this.BtnMoveUp.TabIndex = 8;
      this.BtnMoveUp.Text = "Up";
      this.BtnMoveUp.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 3);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(55, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Shortcuts:";
      // 
      // treeView1
      // 
      this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.treeView1.Location = new System.Drawing.Point(6, 19);
      this.treeView1.Name = "treeView1";
      this.treeView1.Size = new System.Drawing.Size(179, 219);
      this.treeView1.TabIndex = 6;
      // 
      // ShortcutsPreferences
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(401, 294);
      this.Controls.Add(this.tabControl1);
      this.Name = "ShortcutsPreferences";
      this.Text = "Shortcuts Manager";
      this.Load += new System.EventHandler(this.ShortcutsPreferences_Load);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox TxtRawFile;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.Button BtnMoveIn;
    private System.Windows.Forms.Button BtnMoveOut;
    private System.Windows.Forms.Button BtnMoveDown;
    private System.Windows.Forms.Button BtnMoveUp;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TreeView treeView1;
    private System.Windows.Forms.Label LblModified;
  }
}