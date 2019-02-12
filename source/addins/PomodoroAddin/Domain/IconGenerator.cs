/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-1-22
 * Author:  Damian Suess
 * File:    IconGenerator.cs
 * Description:
 *  Generate an icon from text
 */

using System;
using System.Drawing;

namespace PomodoroAddin.Domain
{
  public class IconGenerator
  {
    private Brush _brushColor = new SolidBrush(Color.Red);
    private Brush _brushAccent = new SolidBrush(Color.Black);
    private Bitmap _bitmap = new Bitmap(16, 16);
    private Graphics _graphics;
    private Font _font = new Font(FontFamily.GenericSansSerif, 8);
    private Icon _icon;

    public IconGenerator(string text)
    {
      _graphics = Graphics.FromImage(_bitmap);
      _graphics.DrawString(text, _font, _brushAccent, 1, 1);
      _graphics.DrawString(text, _font, _brushColor, 0, 0);
      IntPtr hIcon = _bitmap.GetHicon();
      _icon = Icon.FromHandle(hIcon);
    }

    ~IconGenerator()
    {
      _icon = null;
      _font = null;
      _graphics = null;
      _bitmap = null;
      _brushAccent = null;
      _brushColor = null;
    }

    public Icon ToIcon()
    {
      return _icon;
    }
  }
}
