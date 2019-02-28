/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-2-28
 * Author:  Damian Suess
 * File:    VolumeInfo.cs
 * Description:
 *  Details about our encrypted drive
 */

namespace Xeno.ToolsHub.VeraCryptAddin.Domain
{
  public class VolumeInfo
  {
    /// <summary>Gets or sets the display text</summary>
    /// <value>The display text for Shortcuts add-in</value>
    public string Title { get; set; }

    /// <summary>Gets or sets index's id</summary>
    /// <value>Internal settings db index id (GUID)</value>
    public string IndexId { get; set; }

    /// <summary>Gets or sets file path</summary>
    /// <value>Path to VeraCrypt file</value>
    public string FilePath { get; set; }

    /// <summary>Gets or sets password</summary>
    /// <value>(optional) password of encrypted volume</value>
    public string Password { get; set; }
  }
}
