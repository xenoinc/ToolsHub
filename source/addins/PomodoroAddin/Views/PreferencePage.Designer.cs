/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PreferencePage.Designer.cs
 * Description:
 *  
 */

namespace PomodoroAddin.Views
{
  partial class PreferencePage
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
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.TxtDuration = new System.Windows.Forms.TextBox();
      this.TxtBreakShort = new System.Windows.Forms.TextBox();
      this.TxtBreakLong = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.ChkSysTrayDurations = new System.Windows.Forms.CheckBox();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.ChkSysTrayBubbles = new System.Windows.Forms.CheckBox();
      this.ChkFlashScreenEvents = new System.Windows.Forms.CheckBox();
      this.BtnTestTrayIconUpdate = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(21, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(89, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Duration (def=25)";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(21, 51);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(63, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Short Break";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(21, 77);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(62, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Long Break";
      // 
      // TxtDuration
      // 
      this.TxtDuration.Location = new System.Drawing.Point(146, 22);
      this.TxtDuration.Name = "TxtDuration";
      this.TxtDuration.Size = new System.Drawing.Size(53, 20);
      this.TxtDuration.TabIndex = 3;
      this.TxtDuration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // TxtBreakShort
      // 
      this.TxtBreakShort.Location = new System.Drawing.Point(146, 48);
      this.TxtBreakShort.Name = "TxtBreakShort";
      this.TxtBreakShort.Size = new System.Drawing.Size(53, 20);
      this.TxtBreakShort.TabIndex = 4;
      this.TxtBreakShort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // TxtBreakLong
      // 
      this.TxtBreakLong.Location = new System.Drawing.Point(146, 74);
      this.TxtBreakLong.Name = "TxtBreakLong";
      this.TxtBreakLong.Size = new System.Drawing.Size(53, 20);
      this.TxtBreakLong.TabIndex = 5;
      this.TxtBreakLong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.TxtBreakLong);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.TxtBreakShort);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.TxtDuration);
      this.groupBox1.Location = new System.Drawing.Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(210, 111);
      this.groupBox1.TabIndex = 6;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Timer Durations";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.BtnTestTrayIconUpdate);
      this.groupBox2.Controls.Add(this.ChkSysTrayDurations);
      this.groupBox2.Location = new System.Drawing.Point(219, 3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(210, 111);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Tray Icons";
      // 
      // ChkSysTrayDurations
      // 
      this.ChkSysTrayDurations.AutoSize = true;
      this.ChkSysTrayDurations.Location = new System.Drawing.Point(6, 24);
      this.ChkSysTrayDurations.Name = "ChkSysTrayDurations";
      this.ChkSysTrayDurations.Size = new System.Drawing.Size(193, 17);
      this.ChkSysTrayDurations.TabIndex = 0;
      this.ChkSysTrayDurations.Text = "Override SysTray icon with duration";
      this.ChkSysTrayDurations.UseVisualStyleBackColor = true;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.groupBox1);
      this.flowLayoutPanel1.Controls.Add(this.groupBox2);
      this.flowLayoutPanel1.Controls.Add(this.groupBox3);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(458, 253);
      this.flowLayoutPanel1.TabIndex = 8;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.ChkSysTrayBubbles);
      this.groupBox3.Controls.Add(this.ChkFlashScreenEvents);
      this.groupBox3.Location = new System.Drawing.Point(3, 120);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(210, 111);
      this.groupBox3.TabIndex = 8;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Notifications";
      // 
      // ChkSysTrayBubbles
      // 
      this.ChkSysTrayBubbles.AutoSize = true;
      this.ChkSysTrayBubbles.Location = new System.Drawing.Point(9, 42);
      this.ChkSysTrayBubbles.Name = "ChkSysTrayBubbles";
      this.ChkSysTrayBubbles.Size = new System.Drawing.Size(123, 17);
      this.ChkSysTrayBubbles.TabIndex = 1;
      this.ChkSysTrayBubbles.Text = "SysTray notifications";
      this.ChkSysTrayBubbles.UseVisualStyleBackColor = true;
      // 
      // ChkFlashScreenEvents
      // 
      this.ChkFlashScreenEvents.AutoSize = true;
      this.ChkFlashScreenEvents.Location = new System.Drawing.Point(9, 19);
      this.ChkFlashScreenEvents.Name = "ChkFlashScreenEvents";
      this.ChkFlashScreenEvents.Size = new System.Drawing.Size(136, 17);
      this.ChkFlashScreenEvents.TabIndex = 0;
      this.ChkFlashScreenEvents.Text = "Flash on-screen events";
      this.ChkFlashScreenEvents.UseVisualStyleBackColor = true;
      // 
      // BtnTestTrayIconUpdate
      // 
      this.BtnTestTrayIconUpdate.Location = new System.Drawing.Point(6, 51);
      this.BtnTestTrayIconUpdate.Name = "BtnTestTrayIconUpdate";
      this.BtnTestTrayIconUpdate.Size = new System.Drawing.Size(111, 23);
      this.BtnTestTrayIconUpdate.TabIndex = 1;
      this.BtnTestTrayIconUpdate.Text = "Test Icon Updates";
      this.BtnTestTrayIconUpdate.UseVisualStyleBackColor = true;
      this.BtnTestTrayIconUpdate.Click += new System.EventHandler(this.BtnTestTrayIconUpdate_Click);
      // 
      // PreferencePage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(458, 253);
      this.Controls.Add(this.flowLayoutPanel1);
      this.Name = "PreferencePage";
      this.Text = "Pomodoro Timer";
      this.Load += new System.EventHandler(this.PreferencePage_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox TxtDuration;
    private System.Windows.Forms.TextBox TxtBreakShort;
    private System.Windows.Forms.TextBox TxtBreakLong;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.CheckBox ChkSysTrayDurations;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.CheckBox ChkSysTrayBubbles;
    private System.Windows.Forms.CheckBox ChkFlashScreenEvents;
    private System.Windows.Forms.Button BtnTestTrayIconUpdate;
  }
}