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
      this.TabListEditor = new System.Windows.Forms.TabPage();
      this.GroupShortcuts = new System.Windows.Forms.GroupBox();
      this.LvShortcuts = new System.Windows.Forms.ListView();
      this.LvShortcutsId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.LvShortcutsTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.LvShortcutsPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.LvShortcutsParameters = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.GroupProperties = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.TxtTitle = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.TxtPathArgs = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.TxtPath = new System.Windows.Forms.TextBox();
      this.TabRawEditor = new System.Windows.Forms.TabPage();
      this.LblModified = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.TxtRawFile = new System.Windows.Forms.TextBox();
      this.BtnRemove = new System.Windows.Forms.Button();
      this.BtnAdd = new System.Windows.Forms.Button();
      this.tabControl1.SuspendLayout();
      this.TabListEditor.SuspendLayout();
      this.GroupShortcuts.SuspendLayout();
      this.GroupProperties.SuspendLayout();
      this.TabRawEditor.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControl1
      // 
      this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl1.Controls.Add(this.TabListEditor);
      this.tabControl1.Controls.Add(this.TabRawEditor);
      this.tabControl1.Location = new System.Drawing.Point(12, 12);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(451, 397);
      this.tabControl1.TabIndex = 7;
      // 
      // TabListEditor
      // 
      this.TabListEditor.Controls.Add(this.GroupShortcuts);
      this.TabListEditor.Controls.Add(this.GroupProperties);
      this.TabListEditor.Location = new System.Drawing.Point(4, 22);
      this.TabListEditor.Name = "TabListEditor";
      this.TabListEditor.Padding = new System.Windows.Forms.Padding(3);
      this.TabListEditor.Size = new System.Drawing.Size(443, 371);
      this.TabListEditor.TabIndex = 0;
      this.TabListEditor.Text = "List of Shortcuts";
      this.TabListEditor.UseVisualStyleBackColor = true;
      // 
      // GroupShortcuts
      // 
      this.GroupShortcuts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GroupShortcuts.Controls.Add(this.LvShortcuts);
      this.GroupShortcuts.Location = new System.Drawing.Point(6, 6);
      this.GroupShortcuts.Name = "GroupShortcuts";
      this.GroupShortcuts.Size = new System.Drawing.Size(431, 222);
      this.GroupShortcuts.TabIndex = 23;
      this.GroupShortcuts.TabStop = false;
      this.GroupShortcuts.Text = "Shortcuts";
      // 
      // LvShortcuts
      // 
      this.LvShortcuts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LvShortcuts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LvShortcutsId,
            this.LvShortcutsTitle,
            this.LvShortcutsPath,
            this.LvShortcutsParameters});
      this.LvShortcuts.FullRowSelect = true;
      this.LvShortcuts.GridLines = true;
      this.LvShortcuts.Location = new System.Drawing.Point(6, 19);
      this.LvShortcuts.MultiSelect = false;
      this.LvShortcuts.Name = "LvShortcuts";
      this.LvShortcuts.Size = new System.Drawing.Size(419, 197);
      this.LvShortcuts.TabIndex = 12;
      this.LvShortcuts.UseCompatibleStateImageBehavior = false;
      this.LvShortcuts.View = System.Windows.Forms.View.Details;
      this.LvShortcuts.SelectedIndexChanged += new System.EventHandler(this.LvShortcuts_SelectedIndexChanged);
      // 
      // LvShortcutsId
      // 
      this.LvShortcutsId.Text = "Id";
      this.LvShortcutsId.Width = 26;
      // 
      // LvShortcutsTitle
      // 
      this.LvShortcutsTitle.Text = "Title";
      this.LvShortcutsTitle.Width = 126;
      // 
      // LvShortcutsPath
      // 
      this.LvShortcutsPath.Text = "Path";
      this.LvShortcutsPath.Width = 261;
      // 
      // LvShortcutsParameters
      // 
      this.LvShortcutsParameters.Text = "Parameters";
      this.LvShortcutsParameters.Width = 8;
      // 
      // GroupProperties
      // 
      this.GroupProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GroupProperties.Controls.Add(this.label3);
      this.GroupProperties.Controls.Add(this.BtnAdd);
      this.GroupProperties.Controls.Add(this.label5);
      this.GroupProperties.Controls.Add(this.BtnRemove);
      this.GroupProperties.Controls.Add(this.TxtTitle);
      this.GroupProperties.Controls.Add(this.label4);
      this.GroupProperties.Controls.Add(this.TxtPathArgs);
      this.GroupProperties.Controls.Add(this.button1);
      this.GroupProperties.Controls.Add(this.TxtPath);
      this.GroupProperties.Location = new System.Drawing.Point(6, 234);
      this.GroupProperties.Name = "GroupProperties";
      this.GroupProperties.Size = new System.Drawing.Size(431, 131);
      this.GroupProperties.TabIndex = 22;
      this.GroupProperties.TabStop = false;
      this.GroupProperties.Text = "Properties";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(28, 22);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(30, 13);
      this.label3.TabIndex = 13;
      this.label3.Text = "Title:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 75);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(60, 13);
      this.label5.TabIndex = 21;
      this.label5.Text = "Arguments:";
      // 
      // TxtTitle
      // 
      this.TxtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtTitle.Location = new System.Drawing.Point(72, 19);
      this.TxtTitle.Name = "TxtTitle";
      this.TxtTitle.Size = new System.Drawing.Size(353, 20);
      this.TxtTitle.TabIndex = 17;
      this.TxtTitle.TextChanged += new System.EventHandler(this.TxtTitle_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(26, 49);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(32, 13);
      this.label4.TabIndex = 16;
      this.label4.Text = "Path:";
      // 
      // TxtPathArgs
      // 
      this.TxtPathArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtPathArgs.Enabled = false;
      this.TxtPathArgs.Location = new System.Drawing.Point(72, 72);
      this.TxtPathArgs.Name = "TxtPathArgs";
      this.TxtPathArgs.Size = new System.Drawing.Size(353, 20);
      this.TxtPathArgs.TabIndex = 20;
      this.TxtPathArgs.TextChanged += new System.EventHandler(this.TxtPathArgs_TextChanged);
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.Enabled = false;
      this.button1.Location = new System.Drawing.Point(391, 45);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(34, 21);
      this.button1.TabIndex = 19;
      this.button1.Text = "...";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // TxtPath
      // 
      this.TxtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtPath.Location = new System.Drawing.Point(72, 46);
      this.TxtPath.Name = "TxtPath";
      this.TxtPath.Size = new System.Drawing.Size(313, 20);
      this.TxtPath.TabIndex = 18;
      this.TxtPath.TextChanged += new System.EventHandler(this.TxtPath_TextChanged);
      // 
      // TabRawEditor
      // 
      this.TabRawEditor.Controls.Add(this.LblModified);
      this.TabRawEditor.Controls.Add(this.label2);
      this.TabRawEditor.Controls.Add(this.TxtRawFile);
      this.TabRawEditor.Location = new System.Drawing.Point(4, 22);
      this.TabRawEditor.Name = "TabRawEditor";
      this.TabRawEditor.Padding = new System.Windows.Forms.Padding(3);
      this.TabRawEditor.Size = new System.Drawing.Size(443, 310);
      this.TabRawEditor.TabIndex = 1;
      this.TabRawEditor.Text = "Raw File";
      this.TabRawEditor.UseVisualStyleBackColor = true;
      // 
      // LblModified
      // 
      this.LblModified.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.LblModified.AutoSize = true;
      this.LblModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LblModified.ForeColor = System.Drawing.SystemColors.HotTrack;
      this.LblModified.Location = new System.Drawing.Point(457, 3);
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
      this.TxtRawFile.Size = new System.Drawing.Size(512, 350);
      this.TxtRawFile.TabIndex = 0;
      this.TxtRawFile.TextChanged += new System.EventHandler(this.TxtRawFile_TextChanged);
      // 
      // BtnRemove
      // 
      this.BtnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnRemove.Location = new System.Drawing.Point(184, 98);
      this.BtnRemove.Name = "BtnRemove";
      this.BtnRemove.Size = new System.Drawing.Size(71, 23);
      this.BtnRemove.TabIndex = 15;
      this.BtnRemove.Text = "- Remove";
      this.BtnRemove.UseVisualStyleBackColor = true;
      this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
      // 
      // BtnAdd
      // 
      this.BtnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnAdd.Location = new System.Drawing.Point(72, 98);
      this.BtnAdd.Name = "BtnAdd";
      this.BtnAdd.Size = new System.Drawing.Size(48, 23);
      this.BtnAdd.TabIndex = 14;
      this.BtnAdd.Text = "+ New";
      this.BtnAdd.UseVisualStyleBackColor = true;
      this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
      // 
      // ShortcutsPreferences
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(475, 421);
      this.Controls.Add(this.tabControl1);
      this.Name = "ShortcutsPreferences";
      this.Text = "Shortcuts Manager";
      this.Load += new System.EventHandler(this.ShortcutsPreferences_Load);
      this.tabControl1.ResumeLayout(false);
      this.TabListEditor.ResumeLayout(false);
      this.GroupShortcuts.ResumeLayout(false);
      this.GroupProperties.ResumeLayout(false);
      this.GroupProperties.PerformLayout();
      this.TabRawEditor.ResumeLayout(false);
      this.TabRawEditor.PerformLayout();
      this.ResumeLayout(false);

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