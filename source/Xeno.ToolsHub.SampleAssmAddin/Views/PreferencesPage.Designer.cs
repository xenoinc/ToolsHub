/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    SampleAssmPreferencesPage.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.SampleAssmAddin.Views
{
  partial class PreferencesPage
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
      this.BtnRefreshSysTray = new System.Windows.Forms.Button();
      this.BtnSysTrayNotify = new System.Windows.Forms.Button();
      this.TxtSysTrayMessage = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(15, 57);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(127, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Test SysTray Commands:";
      // 
      // BtnTest2
      // 
      this.BtnTest2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnTest2.Location = new System.Drawing.Point(242, 143);
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
      // BtnRefreshSysTray
      // 
      this.BtnRefreshSysTray.Location = new System.Drawing.Point(12, 73);
      this.BtnRefreshSysTray.Name = "BtnRefreshSysTray";
      this.BtnRefreshSysTray.Size = new System.Drawing.Size(117, 23);
      this.BtnRefreshSysTray.TabIndex = 6;
      this.BtnRefreshSysTray.Text = "Refresh SysTray";
      this.BtnRefreshSysTray.UseVisualStyleBackColor = true;
      this.BtnRefreshSysTray.Click += new System.EventHandler(this.BtnRefreshSysTray_Click);
      // 
      // BtnSysTrayNotify
      // 
      this.BtnSysTrayNotify.Location = new System.Drawing.Point(12, 102);
      this.BtnSysTrayNotify.Name = "BtnSysTrayNotify";
      this.BtnSysTrayNotify.Size = new System.Drawing.Size(117, 23);
      this.BtnSysTrayNotify.TabIndex = 7;
      this.BtnSysTrayNotify.Text = "Notify message:";
      this.BtnSysTrayNotify.UseVisualStyleBackColor = true;
      this.BtnSysTrayNotify.Click += new System.EventHandler(this.BtnSysTrayNotify_Click);
      // 
      // TxtSysTrayMessage
      // 
      this.TxtSysTrayMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtSysTrayMessage.Location = new System.Drawing.Point(135, 104);
      this.TxtSysTrayMessage.Name = "TxtSysTrayMessage";
      this.TxtSysTrayMessage.Size = new System.Drawing.Size(215, 20);
      this.TxtSysTrayMessage.TabIndex = 8;
      this.TxtSysTrayMessage.Text = "Sample notification message";
      // 
      // PreferencesPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(362, 178);
      this.Controls.Add(this.TxtSysTrayMessage);
      this.Controls.Add(this.BtnSysTrayNotify);
      this.Controls.Add(this.BtnRefreshSysTray);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.BtnTest2);
      this.Controls.Add(this.BtnTest1);
      this.Name = "PreferencesPage";
      this.Text = "Sample Add-in (External Assm)";
      this.Load += new System.EventHandler(this.SampleAssmPreferencesPage_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button BtnTest2;
    private System.Windows.Forms.Button BtnTest1;
    private System.Windows.Forms.Button BtnRefreshSysTray;
    private System.Windows.Forms.Button BtnSysTrayNotify;
    private System.Windows.Forms.TextBox TxtSysTrayMessage;
  }
}