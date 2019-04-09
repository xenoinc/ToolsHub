/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    PreferencesPage.Designer.cs
 * Description:
 *  
 */

namespace Xeno.ToolsHub.SidebarAddin.Views
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
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.ComboScreenOrientaiton = new System.Windows.Forms.ComboBox();
      this.ComboScreenArea = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.UpDownScreenNumber = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.ChkOnTop = new System.Windows.Forms.CheckBox();
      this.ChkShowOnStart = new System.Windows.Forms.CheckBox();
      this.ChkAutoHide = new System.Windows.Forms.CheckBox();
      this.Opacity = new System.Windows.Forms.TrackBar();
      this.label4 = new System.Windows.Forms.Label();
      this.flowLayoutPanel1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.UpDownScreenNumber)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Opacity)).BeginInit();
      this.SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.groupBox3);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(300, 285);
      this.flowLayoutPanel1.TabIndex = 3;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label4);
      this.groupBox3.Controls.Add(this.Opacity);
      this.groupBox3.Controls.Add(this.ComboScreenOrientaiton);
      this.groupBox3.Controls.Add(this.ComboScreenArea);
      this.groupBox3.Controls.Add(this.label3);
      this.groupBox3.Controls.Add(this.label2);
      this.groupBox3.Controls.Add(this.UpDownScreenNumber);
      this.groupBox3.Controls.Add(this.label1);
      this.groupBox3.Controls.Add(this.ChkOnTop);
      this.groupBox3.Controls.Add(this.ChkShowOnStart);
      this.groupBox3.Controls.Add(this.ChkAutoHide);
      this.groupBox3.Location = new System.Drawing.Point(3, 3);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(269, 249);
      this.groupBox3.TabIndex = 1;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Sidebar Settings";
      // 
      // ComboScreenOrientaiton
      // 
      this.ComboScreenOrientaiton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ComboScreenOrientaiton.Enabled = false;
      this.ComboScreenOrientaiton.FormattingEnabled = true;
      this.ComboScreenOrientaiton.Items.AddRange(new object[] {
            "Vertical",
            "Horizontal"});
      this.ComboScreenOrientaiton.Location = new System.Drawing.Point(119, 140);
      this.ComboScreenOrientaiton.Name = "ComboScreenOrientaiton";
      this.ComboScreenOrientaiton.Size = new System.Drawing.Size(121, 21);
      this.ComboScreenOrientaiton.TabIndex = 16;
      // 
      // ComboScreenArea
      // 
      this.ComboScreenArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ComboScreenArea.Enabled = false;
      this.ComboScreenArea.FormattingEnabled = true;
      this.ComboScreenArea.Items.AddRange(new object[] {
            "(none)",
            "Left",
            "Right",
            "Top",
            "Bottom"});
      this.ComboScreenArea.Location = new System.Drawing.Point(119, 113);
      this.ComboScreenArea.Name = "ComboScreenArea";
      this.ComboScreenArea.Size = new System.Drawing.Size(121, 21);
      this.ComboScreenArea.TabIndex = 15;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(9, 143);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(98, 13);
      this.label3.TabIndex = 14;
      this.label3.Text = "Screen Orientation:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(9, 116);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(69, 13);
      this.label2.TabIndex = 13;
      this.label2.Text = "Screen Area:";
      // 
      // UpDownScreenNumber
      // 
      this.UpDownScreenNumber.Enabled = false;
      this.UpDownScreenNumber.Location = new System.Drawing.Point(119, 87);
      this.UpDownScreenNumber.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.UpDownScreenNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.UpDownScreenNumber.Name = "UpDownScreenNumber";
      this.UpDownScreenNumber.Size = new System.Drawing.Size(53, 20);
      this.UpDownScreenNumber.TabIndex = 12;
      this.UpDownScreenNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 89);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(84, 13);
      this.label1.TabIndex = 11;
      this.label1.Text = "Screen Number:";
      // 
      // ChkOnTop
      // 
      this.ChkOnTop.AutoSize = true;
      this.ChkOnTop.Enabled = false;
      this.ChkOnTop.Location = new System.Drawing.Point(9, 65);
      this.ChkOnTop.Name = "ChkOnTop";
      this.ChkOnTop.Size = new System.Drawing.Size(104, 17);
      this.ChkOnTop.TabIndex = 10;
      this.ChkOnTop.Text = "Window On-Top";
      this.ChkOnTop.UseVisualStyleBackColor = true;
      // 
      // ChkShowOnStart
      // 
      this.ChkShowOnStart.AutoSize = true;
      this.ChkShowOnStart.Enabled = false;
      this.ChkShowOnStart.Location = new System.Drawing.Point(9, 19);
      this.ChkShowOnStart.Name = "ChkShowOnStart";
      this.ChkShowOnStart.Size = new System.Drawing.Size(153, 17);
      this.ChkShowOnStart.TabIndex = 8;
      this.ChkShowOnStart.Text = "Show Sidebar(s) on startup";
      this.ChkShowOnStart.UseVisualStyleBackColor = true;
      // 
      // ChkAutoHide
      // 
      this.ChkAutoHide.AutoSize = true;
      this.ChkAutoHide.Enabled = false;
      this.ChkAutoHide.Location = new System.Drawing.Point(9, 42);
      this.ChkAutoHide.Name = "ChkAutoHide";
      this.ChkAutoHide.Size = new System.Drawing.Size(71, 17);
      this.ChkAutoHide.TabIndex = 9;
      this.ChkAutoHide.Text = "Auto-hide";
      this.ChkAutoHide.UseVisualStyleBackColor = true;
      // 
      // Opacity
      // 
      this.Opacity.Location = new System.Drawing.Point(6, 198);
      this.Opacity.Maximum = 100;
      this.Opacity.Minimum = 20;
      this.Opacity.Name = "Opacity";
      this.Opacity.Size = new System.Drawing.Size(251, 45);
      this.Opacity.TabIndex = 17;
      this.Opacity.TickFrequency = 10;
      this.Opacity.Value = 20;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(9, 168);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(46, 13);
      this.label4.TabIndex = 18;
      this.label4.Text = "Opacity:";
      // 
      // PreferencesPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(300, 285);
      this.Controls.Add(this.flowLayoutPanel1);
      this.Name = "PreferencesPage";
      this.Text = "PreferencesPage";
      this.Load += new System.EventHandler(this.PreferencesPage_Load);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.UpDownScreenNumber)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Opacity)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.CheckBox ChkOnTop;
    private System.Windows.Forms.CheckBox ChkShowOnStart;
    private System.Windows.Forms.CheckBox ChkAutoHide;
    private System.Windows.Forms.NumericUpDown UpDownScreenNumber;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox ComboScreenOrientaiton;
    private System.Windows.Forms.ComboBox ComboScreenArea;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TrackBar Opacity;
  }
}