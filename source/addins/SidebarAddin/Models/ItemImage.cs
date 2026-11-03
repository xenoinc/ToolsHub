/* Copyright Xeno Innovations, Inc. 2020
 * Date:    2020-3-13
 * Author:  Damian Suess
 * File:    DockItem.cs
 * Description:
 *  Dock item
 */

namespace Xeno.ToolsHub.SidebarAddin.Models;

public struct ItemImage
{
  public int Height;
  public System.Drawing.Bitmap Image;
  public string ImagePath;

  /// <summary>Icon is fixed into position</summary>
  public bool LockedItem;

  public string Target;
  public int Width;
  public int X;
  public int Y;
}
