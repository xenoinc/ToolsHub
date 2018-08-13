/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCryptPreferences.cs
 * Description:
 *
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.VeraCryptAddin
{
  public class VeraCryptPreferences : AddinPreferenceFactory
  {
    public override Panel CreatePreferenceWidget()
    {
      return new VeraCryptPreferencesPanel();
    }
  }

  public class VeraCryptPreferencesPanel : Panel
  {
    public VeraCryptPreferencesPanel() : base()
    {
    }
  }
}
