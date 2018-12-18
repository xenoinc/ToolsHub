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
      this.SuspendLayout();
      // 
      // ListAddins
      // 
      this.ListAddins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ListAddins.FullRowSelect = true;
      this.ListAddins.GridLines = true;
      this.ListAddins.Location = new System.Drawing.Point(0, 16);
      this.ListAddins.MultiSelect = false;
      this.ListAddins.Name = "ListAddins";
      this.ListAddins.Size = new System.Drawing.Size(243, 301);
      this.ListAddins.TabIndex = 0;
      this.ListAddins.UseCompatibleStateImageBehavior = false;
      this.ListAddins.View = System.Windows.Forms.View.List;
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
      this.BtnEnable.Location = new System.Drawing.Point(260, 26);
      this.BtnEnable.Name = "BtnEnable";
      this.BtnEnable.Size = new System.Drawing.Size(105, 23);
      this.BtnEnable.TabIndex = 2;
      this.BtnEnable.Text = "&Enable";
      this.BtnEnable.UseVisualStyleBackColor = true;
      // 
      // BtnDisable
      // 
      this.BtnDisable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnDisable.Location = new System.Drawing.Point(260, 55);
      this.BtnDisable.Name = "BtnDisable";
      this.BtnDisable.Size = new System.Drawing.Size(105, 23);
      this.BtnDisable.TabIndex = 3;
      this.BtnDisable.Text = "&Disable";
      this.BtnDisable.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(249, 100);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(30, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Title:";
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(249, 142);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(45, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Version:";
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(249, 241);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(63, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Description:";
      // 
      // label5
      // 
      this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(249, 191);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(52, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Author(s):";
      // 
      // AddinManagerCtrl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.BtnDisable);
      this.Controls.Add(this.BtnEnable);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ListAddins);
      this.Name = "AddinManagerCtrl";
      this.Size = new System.Drawing.Size(381, 320);
      this.Load += new System.EventHandler(this.AddinManagerCtrl_Load);
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
  }
}
