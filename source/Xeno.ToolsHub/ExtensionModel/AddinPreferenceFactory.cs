/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    AddinPreferenceFactory.cs
 * Description:
 *
 */

using System.Windows.Forms;

namespace Xeno.ToolsHub.ExtensionModel
{
  public abstract class AddinPreferenceFactory
  {
    public abstract Panel CreatePreferenceWidget();
  }
}
