/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-19
 * Author:  Damian Suess
 * File:    AboutForm.cs
 * Description:
 *  Cheap-ass about box
 */

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace Xeno.ToolsHub.Views
{
  public partial class AboutForm : Form // AnimateForm
  {
    public AboutForm() // : base(true) // Used by AnimateForm.. but it looks ugly on the latest Win10
    {
      InitializeComponent();

      System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
      var ver = AssemblyName.GetAssemblyName(assembly.Location).Version.ToString();

      ////string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      ////string assemblyVersion = Assembly.LoadFile('your assembly file').GetName().Version.ToString();
      ////string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
      ////string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

      label1.Text = $"ToolsHub v{ver}";
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
