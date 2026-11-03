/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-21
 * Author:  Damian Suess
 * File:    LinkIcon.cs
 * Description:
 *  Shortcut link icon details
 */

using System;

namespace Xeno.ToolsHub.SidebarAddin.Models;

public class DockItem
{
  private readonly Action _clickAction;
  private readonly string _image;
  private readonly string _text;

  public DockItem(string text, string image, Action clickAction)
  {
    _text = text;
    _image = image;
    _clickAction = clickAction;

    DockItemImage = new ItemImage();
  }

  public ItemImage DockItemImage { get; set; }

  public string Image => DockItemImage.ImagePath;

  public Action OnClick => _clickAction;

  public string Text => _text;
}
