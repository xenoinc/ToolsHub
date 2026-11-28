/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-19
 * Author:  Damian Suess
 * File:    AboutForm.cs
 * Description:
 *  Cheap-ass about box
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Xeno.Dockzi.Services.Logging;

namespace Xeno.Dockzi.Views
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

      label1.Text = $"Dockzi v{ver}";
    }

    private void AboutForm_Load(object sender, EventArgs e)
    {
      linkGitHub.Links.Add(new LinkLabel.Link { LinkData = "https://github.com/xenoinc/ToolsHub" });
      linkXenoInc.Links.Add(new LinkLabel.Link { LinkData = "https://xenoinc.com" });
    }

    private void LinkedLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      if (e.Link?.LinkData is not string target ||
          !Uri.TryCreate(target, UriKind.Absolute, out var uri) ||
          (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
        return;

      try
      {
        using var process = Process.Start(new ProcessStartInfo(uri.AbsoluteUri)
        {
          UseShellExecute = true,
        });
        e.Link.Visited = true;
      }
      catch (Exception ex) when (ex is Win32Exception or InvalidOperationException)
      {
        Log.Warn($"Unable to open About link '{uri.AbsoluteUri}': {ex.Message}");
        MessageBox.Show(
          this,
          $"Unable to open your browser. You can visit:\n{uri.AbsoluteUri}",
          "Dockzi",
          MessageBoxButtons.OK,
          MessageBoxIcon.Warning);
      }
    }
  }
}
