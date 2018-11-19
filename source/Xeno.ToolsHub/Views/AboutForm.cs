/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-19
 * Author:  Damian Suess
 * File:    AboutForm.cs
 * Description:
 *  Cheap-ass about box
 */

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Xeno.ToolsHub.Views
{
  public partial class AboutForm : Form
  {
    public AboutForm()
    {
      InitializeComponent();
    }

    private void AboutForm_Load(object sender, EventArgs e)
    {
      linkGitHub.Links.Add(new LinkLabel.Link { LinkData = "https://github.com/DamianSuess/ToolsHub" });
      linkXenoInc.Links.Add(new LinkLabel.Link { LinkData = "https://xenoinc.com" });
    }

    private void LinkedLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(e.Link.LinkData as string);
    }
  }
}
