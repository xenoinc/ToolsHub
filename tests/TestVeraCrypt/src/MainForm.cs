// Author: Damian Suess
// Date: 2018-08-14

using System;
using System.Windows.Forms;

namespace TestVeraCrypt
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void btnMount_Click(object sender, EventArgs e)
    {
      // var cmd = "VeraCrypt.exe -ArugmentList '/l [DriveLetter]', '/v [pathName]', '/q', '/m ts'";
      string drive = txtDriveLetter.Text;
      string vcPath = @"C:\Program Files\VeraCrypt\VeraCrypt.exe";
      string hcPath = @"";
      string password = "";

      VeraCrypt.Drive mnt = new VeraCrypt.Drive(vcPath, drive, hcPath, password);
      mnt.IsSilent = false;

      int ret = mnt.Mount();
      MessageBox.Show("Ret: " + ret);
    }

    private void btnDismount_Click(object sender, EventArgs e)
    {
      string drive = txtDriveLetter.Text;
      string vcPath = @"C:\Program Files\VeraCrypt\VeraCrypt.exe";

      VeraCrypt.Drive mnt = new VeraCrypt.Drive(vcPath, drive);
      mnt.IsSilent = false;
      mnt.IsBeep = true;

      int ret = mnt.Dismount(force: (chkForce.Checked ? true : false));
      MessageBox.Show("Ret: " + ret);
    }

    private async void btnRefreshMounts_Click(object sender, EventArgs e)
    {
      var list = await VeraCrypt.Utils.GetMountList();

      //this.Text = $"Found {list.Count} mounts";
    }

    private void btnFindInstall_Click(object sender, EventArgs e)
    {
    }
  }
}
