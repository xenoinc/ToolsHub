/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-21
 * Author:  Damian Suess
 * File:    LinkIcon.cs
 * Description:
 *  Shortcut link icon details
 */

using System;

namespace Xeno.ToolsHub.SidebarAddin.Models
{
  public class DockItem
  {
    private readonly string _text;
    private readonly string _image;
    private readonly Action _clickAction;

    public DockItem(string text, string image, Action clickAction)
    {
      _text = text;
      _image = image;
      _clickAction = clickAction;

      DockItemImage = new ItemImage();
    }

    public ItemImage DockItemImage { get; set; }

    public string Text => _text;

    public string Image => DockItemImage.ImagePath;

    public Action OnClick
    {
      get { return _clickAction; }
    }
  }
}
