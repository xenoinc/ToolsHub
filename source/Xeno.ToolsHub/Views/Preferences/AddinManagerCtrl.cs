/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-24
 * Author:  Damian Suess
 * File:    AddinManagerCtrl.cs
 * Description:
 *
 */

using System.Windows.Forms;
using Xeno.ToolsHub.Managers;

namespace Xeno.ToolsHub.Views.Preferences
{
  public partial class AddinManagerCtrl : UserControl
  {
    private readonly AddinsManager _addinManager;

    public AddinManagerCtrl(AddinsManager manager)
    {
      InitializeComponent();

      _addinManager = manager;

      GetAddins();
    }

    private void AddinManagerCtrl_Load(object sender, System.EventArgs e)
    {
    }

    private void GetAddins()
    {
      ListAddins.Clear();

      foreach (Mono.Addins.Addin addin in _addinManager.GetAllAddins())
      {
        ListAddins.Items.Add(addin.Name);
        // Mono.Addins.Setup.SetupService.GetAddinHeader(addin),
        // addin
        // addin.Enabled
        // addin.IsUserAddin
      }
    }
  }
}
