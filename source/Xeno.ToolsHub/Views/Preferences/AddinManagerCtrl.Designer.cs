/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-24
 * Author:  Damian Suess
 * File:    AddinManagerCtrl.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.Views.Preferences
{
  partial class AddinManagerCtrl
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
      this.ListAddins = new System.Windows.Forms.ListView();
      this.label1 = new System.Windows.Forms.Label();
      this.BtnEnable = new System.Windows.Forms.Button();
      this.BtnDisable = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.ListAddinId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ListAddinName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ListAddinEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.ListAddinIsUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // ListAddins
      // 
      this.ListAddins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ListAddins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ListAddinId,
            this.ListAddinName,
            this.ListAddinEnabled,
            this.ListAddinIsUser});
      this.ListAddins.FullRowSelect = true;
      this.ListAddins.GridLines = true;
      this.ListAddins.Location = new System.Drawing.Point(6, 47);
      this.ListAddins.MultiSelect = false;
      this.ListAddins.Name = "ListAddins";
      this.ListAddins.Size = new System.Drawing.Size(466, 193);
      this.ListAddins.TabIndex = 0;
      this.ListAddins.UseCompatibleStateImageBehavior = false;
      this.ListAddins.View = System.Windows.Forms.View.Details;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(87, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Enabled Add-ins:";
      // 
      // BtnEnable
      // 
      this.BtnEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnEnable.Location = new System.Drawing.Point(307, 3);
      this.BtnEnable.Name = "BtnEnable";
      this.BtnEnable.Size = new System.Drawing.Size(84, 38);
      this.BtnEnable.TabIndex = 2;
      this.BtnEnable.Text = "&Enable";
      this.BtnEnable.UseVisualStyleBackColor = true;
      this.BtnEnable.Click += new System.EventHandler(this.BtnEnable_Click);
      // 
      // BtnDisable
      // 
      this.BtnDisable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnDisable.Location = new System.Drawing.Point(397, 3);
      this.BtnDisable.Name = "BtnDisable";
      this.BtnDisable.Size = new System.Drawing.Size(78, 38);
      this.BtnDisable.TabIndex = 3;
      this.BtnDisable.Text = "&Disable";
      this.BtnDisable.UseVisualStyleBackColor = true;
      this.BtnDisable.Click += new System.EventHandler(this.BtnDisable_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 20);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(30, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Title:";
      this.label2.Visible = false;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 35);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(45, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Version:";
      this.label3.Visible = false;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 67);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(63, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Description:";
      this.label4.Visible = false;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 52);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(52, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Author(s):";
      this.label5.Visible = false;
      // 
      // ListAddinId
      // 
      this.ListAddinId.Text = "Id";
      this.ListAddinId.Width = 74;
      // 
      // ListAddinName
      // 
      this.ListAddinName.Text = "Name";
      this.ListAddinName.Width = 222;
      // 
      // ListAddinEnabled
      // 
      this.ListAddinEnabled.Text = "Enabled";
      this.ListAddinEnabled.Width = 98;
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Location = new System.Drawing.Point(6, 246);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(466, 90);
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Addin Information";
      // 
      // ListAddinIsUser
      // 
      this.ListAddinIsUser.Text = "User Addin";
      // 
      // AddinManagerCtrl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.BtnDisable);
      this.Controls.Add(this.BtnEnable);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ListAddins);
      this.Name = "AddinManagerCtrl";
      this.Size = new System.Drawing.Size(475, 339);
      this.Load += new System.EventHandler(this.AddinManagerCtrl_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView ListAddins;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button BtnEnable;
    private System.Windows.Forms.Button BtnDisable;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.ColumnHeader ListAddinId;
    private System.Windows.Forms.ColumnHeader ListAddinName;
    private System.Windows.Forms.ColumnHeader ListAddinEnabled;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ColumnHeader ListAddinIsUser;
  }
}
