/* Copyright Xeno Innovations, Inc. 2020
 * Date:    2020-3-13
 * Author:  Damian Suess
 * File:    DockItem.cs
 * Description:
 *  Dock item
 */

namespace Xeno.ToolsHub.SidebarAddin.Models
{
  public struct ItemImage
  {
    public int X;
    public int Y;
    public int Width;
    public int Height;

    public string Target;

    /// <summary>Icon is fixed into position</summary>
    public bool LockedItem;

    public System.Drawing.Bitmap Image;
    public string ImagePath;
  }
}
