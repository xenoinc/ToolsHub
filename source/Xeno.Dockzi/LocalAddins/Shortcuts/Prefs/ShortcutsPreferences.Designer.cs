/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    ShortcutsPreferences.Designer.cs
 * Description:
 *  
 */

namespace Xeno.Dockzi.LocalAddins.Shortcuts.Prefs
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
      tabControl1 = new System.Windows.Forms.TabControl();
      TabListEditor = new System.Windows.Forms.TabPage();
      GroupShortcuts = new System.Windows.Forms.GroupBox();
      LvShortcuts = new System.Windows.Forms.ListView();
      LvShortcutsId = new System.Windows.Forms.ColumnHeader();
      LvShortcutsTitle = new System.Windows.Forms.ColumnHeader();
      LvShortcutsPath = new System.Windows.Forms.ColumnHeader();
      LvShortcutsParameters = new System.Windows.Forms.ColumnHeader();
      GroupProperties = new System.Windows.Forms.GroupBox();
      label3 = new System.Windows.Forms.Label();
      BtnAdd = new System.Windows.Forms.Button();
      label5 = new System.Windows.Forms.Label();
      BtnRemove = new System.Windows.Forms.Button();
      TxtTitle = new System.Windows.Forms.TextBox();
      label4 = new System.Windows.Forms.Label();
      TxtPathArgs = new System.Windows.Forms.TextBox();
      button1 = new System.Windows.Forms.Button();
      TxtPath = new System.Windows.Forms.TextBox();
      TabRawEditor = new System.Windows.Forms.TabPage();
      LblModified = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      TxtRawFile = new System.Windows.Forms.TextBox();
      tabControl1.SuspendLayout();
      TabListEditor.SuspendLayout();
      GroupShortcuts.SuspendLayout();
      GroupProperties.SuspendLayout();
      TabRawEditor.SuspendLayout();
      SuspendLayout();
      // 
      // tabControl1
      // 
      tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      tabControl1.Controls.Add(TabListEditor);
      tabControl1.Controls.Add(TabRawEditor);
      tabControl1.Location = new System.Drawing.Point(14, 14);
      tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      tabControl1.Name = "tabControl1";
      tabControl1.SelectedIndex = 0;
      tabControl1.Size = new System.Drawing.Size(526, 458);
      tabControl1.TabIndex = 7;
      // 
      // TabListEditor
      // 
      TabListEditor.Controls.Add(GroupShortcuts);
      TabListEditor.Controls.Add(GroupProperties);
      TabListEditor.Location = new System.Drawing.Point(4, 24);
      TabListEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TabListEditor.Name = "TabListEditor";
      TabListEditor.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TabListEditor.Size = new System.Drawing.Size(518, 430);
      TabListEditor.TabIndex = 0;
      TabListEditor.Text = "List of Shortcuts";
      TabListEditor.UseVisualStyleBackColor = true;
      // 
      // GroupShortcuts
      // 
      GroupShortcuts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      GroupShortcuts.Controls.Add(LvShortcuts);
      GroupShortcuts.Location = new System.Drawing.Point(7, 7);
      GroupShortcuts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      GroupShortcuts.Name = "GroupShortcuts";
      GroupShortcuts.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      GroupShortcuts.Size = new System.Drawing.Size(503, 256);
      GroupShortcuts.TabIndex = 23;
      GroupShortcuts.TabStop = false;
      GroupShortcuts.Text = "Shortcuts";
      // 
      // LvShortcuts
      // 
      LvShortcuts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      LvShortcuts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { LvShortcutsId, LvShortcutsTitle, LvShortcutsPath, LvShortcutsParameters });
      LvShortcuts.FullRowSelect = true;
      LvShortcuts.GridLines = true;
      LvShortcuts.Location = new System.Drawing.Point(7, 22);
      LvShortcuts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      LvShortcuts.MultiSelect = false;
      LvShortcuts.Name = "LvShortcuts";
      LvShortcuts.Size = new System.Drawing.Size(488, 227);
      LvShortcuts.TabIndex = 12;
      LvShortcuts.UseCompatibleStateImageBehavior = false;
      LvShortcuts.View = System.Windows.Forms.View.Details;
      LvShortcuts.SelectedIndexChanged += LvShortcuts_SelectedIndexChanged;
      // 
      // LvShortcutsId
      // 
      LvShortcutsId.Text = "Id";
      LvShortcutsId.Width = 26;
      // 
      // LvShortcutsTitle
      // 
      LvShortcutsTitle.Text = "Title";
      LvShortcutsTitle.Width = 126;
      // 
      // LvShortcutsPath
      // 
      LvShortcutsPath.Text = "Path";
      LvShortcutsPath.Width = 261;
      // 
      // LvShortcutsParameters
      // 
      LvShortcutsParameters.Text = "Parameters";
      LvShortcutsParameters.Width = 8;
      // 
      // GroupProperties
      // 
      GroupProperties.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      GroupProperties.Controls.Add(label3);
      GroupProperties.Controls.Add(BtnAdd);
      GroupProperties.Controls.Add(label5);
      GroupProperties.Controls.Add(BtnRemove);
      GroupProperties.Controls.Add(TxtTitle);
      GroupProperties.Controls.Add(label4);
      GroupProperties.Controls.Add(TxtPathArgs);
      GroupProperties.Controls.Add(button1);
      GroupProperties.Controls.Add(TxtPath);
      GroupProperties.Location = new System.Drawing.Point(7, 270);
      GroupProperties.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      GroupProperties.Name = "GroupProperties";
      GroupProperties.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      GroupProperties.Size = new System.Drawing.Size(503, 151);
      GroupProperties.TabIndex = 22;
      GroupProperties.TabStop = false;
      GroupProperties.Text = "Properties";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(33, 25);
      label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(33, 15);
      label3.TabIndex = 13;
      label3.Text = "Title:";
      // 
      // BtnAdd
      // 
      BtnAdd.Location = new System.Drawing.Point(84, 113);
      BtnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      BtnAdd.Name = "BtnAdd";
      BtnAdd.Size = new System.Drawing.Size(56, 27);
      BtnAdd.TabIndex = 14;
      BtnAdd.Text = "+ New";
      BtnAdd.UseVisualStyleBackColor = true;
      BtnAdd.Click += BtnAdd_Click;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point(7, 87);
      label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size(69, 15);
      label5.TabIndex = 21;
      label5.Text = "Arguments:";
      // 
      // BtnRemove
      // 
      BtnRemove.Location = new System.Drawing.Point(148, 113);
      BtnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      BtnRemove.Name = "BtnRemove";
      BtnRemove.Size = new System.Drawing.Size(83, 27);
      BtnRemove.TabIndex = 15;
      BtnRemove.Text = "- Remove";
      BtnRemove.UseVisualStyleBackColor = true;
      BtnRemove.Click += BtnRemove_Click;
      // 
      // TxtTitle
      // 
      TxtTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      TxtTitle.Location = new System.Drawing.Point(84, 22);
      TxtTitle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TxtTitle.Name = "TxtTitle";
      TxtTitle.Size = new System.Drawing.Size(411, 23);
      TxtTitle.TabIndex = 17;
      TxtTitle.TextChanged += TxtTitle_TextChanged;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point(30, 57);
      label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(34, 15);
      label4.TabIndex = 16;
      label4.Text = "Path:";
      // 
      // TxtPathArgs
      // 
      TxtPathArgs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      TxtPathArgs.Enabled = false;
      TxtPathArgs.Location = new System.Drawing.Point(84, 83);
      TxtPathArgs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TxtPathArgs.Name = "TxtPathArgs";
      TxtPathArgs.Size = new System.Drawing.Size(411, 23);
      TxtPathArgs.TabIndex = 20;
      TxtPathArgs.TextChanged += TxtPathArgs_TextChanged;
      // 
      // button1
      // 
      button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
      button1.Enabled = false;
      button1.Location = new System.Drawing.Point(456, 52);
      button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      button1.Name = "button1";
      button1.Size = new System.Drawing.Size(40, 24);
      button1.TabIndex = 19;
      button1.Text = "...";
      button1.UseVisualStyleBackColor = true;
      // 
      // TxtPath
      // 
      TxtPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      TxtPath.Location = new System.Drawing.Point(84, 53);
      TxtPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TxtPath.Name = "TxtPath";
      TxtPath.Size = new System.Drawing.Size(364, 23);
      TxtPath.TabIndex = 18;
      TxtPath.TextChanged += TxtPath_TextChanged;
      // 
      // TabRawEditor
      // 
      TabRawEditor.Controls.Add(LblModified);
      TabRawEditor.Controls.Add(label2);
      TabRawEditor.Controls.Add(TxtRawFile);
      TabRawEditor.Location = new System.Drawing.Point(4, 24);
      TabRawEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TabRawEditor.Name = "TabRawEditor";
      TabRawEditor.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TabRawEditor.Size = new System.Drawing.Size(518, 430);
      TabRawEditor.TabIndex = 1;
      TabRawEditor.Text = "Raw File";
      TabRawEditor.UseVisualStyleBackColor = true;
      // 
      // LblModified
      // 
      LblModified.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
      LblModified.AutoSize = true;
      LblModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
      LblModified.ForeColor = System.Drawing.SystemColors.HotTrack;
      LblModified.Location = new System.Drawing.Point(533, 3);
      LblModified.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      LblModified.Name = "LblModified";
      LblModified.Size = new System.Drawing.Size(47, 13);
      LblModified.TabIndex = 2;
      LblModified.Text = "Modified";
      LblModified.Visible = false;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(7, 3);
      label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(112, 15);
      label2.TabIndex = 1;
      label2.Text = "Shortcuts JSON File:";
      // 
      // TxtRawFile
      // 
      TxtRawFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      TxtRawFile.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
      TxtRawFile.Location = new System.Drawing.Point(7, 22);
      TxtRawFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      TxtRawFile.Multiline = true;
      TxtRawFile.Name = "TxtRawFile";
      TxtRawFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      TxtRawFile.Size = new System.Drawing.Size(597, 403);
      TxtRawFile.TabIndex = 0;
      TxtRawFile.TextChanged += TxtRawFile_TextChanged;
      // 
      // ShortcutsPreferences
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(554, 486);
      Controls.Add(tabControl1);
      Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      Name = "ShortcutsPreferences";
      Text = "Shortcuts Manager";
      Load += ShortcutsPreferences_Load;
      tabControl1.ResumeLayout(false);
      TabListEditor.ResumeLayout(false);
      GroupShortcuts.ResumeLayout(false);
      GroupProperties.ResumeLayout(false);
      GroupProperties.PerformLayout();
      TabRawEditor.ResumeLayout(false);
      TabRawEditor.PerformLayout();
      ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage TabRawEditor;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox TxtRawFile;
    private System.Windows.Forms.TabPage TabListEditor;
    private System.Windows.Forms.Label LblModified;
    private System.Windows.Forms.TextBox TxtPath;
    private System.Windows.Forms.TextBox TxtTitle;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ListView LvShortcuts;
    private System.Windows.Forms.ColumnHeader LvShortcutsId;
    private System.Windows.Forms.ColumnHeader LvShortcutsTitle;
    private System.Windows.Forms.ColumnHeader LvShortcutsPath;
    private System.Windows.Forms.ColumnHeader LvShortcutsParameters;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox TxtPathArgs;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.GroupBox GroupProperties;
    private System.Windows.Forms.GroupBox GroupShortcuts;
    private System.Windows.Forms.Button BtnAdd;
    private System.Windows.Forms.Button BtnRemove;
  }
}