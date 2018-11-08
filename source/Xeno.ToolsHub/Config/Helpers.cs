/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-6
 * Author:  Damian Suess
 * File:    Helpers.cs
 * Description:
 *  Global helper methods. Code does not necessarily rely on other internal classes
 */

using System;
using System.IO;
using Newtonsoft.Json;

namespace Xeno.ToolsHub.Config
{
  public static class Helpers
  {
    /// <summary>
    ///   Execute on UI thread asynchronously (don't wait for completion)
    /// </summary>
    /// <param name="control"></param>
    /// <param name="code"></param>
    /// <example>
    ///   Store.ExecuteThread(this, () =>
    ///   {
    ///     Text1.Text = "Something new";
    ///   });
    /// </example>
    public static void ExecuteThread(this System.Windows.Forms.Control control, Action code)
    {
      if (control.InvokeRequired)
      {
        control.BeginInvoke(code);
        return;
      }
      code.Invoke();
    }

    /// <summary>
    ///   Execute on UI thread synchronously (waiting for completion before continuing).
    ///   Use this if we're reading back data from GUI thread, like text or something.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="code"></param>
    /// <example>
    ///   Store.ExecuteThread(this, () =>
    ///   {
    ///     string getText = Text1.Text;
    ///   });
    /// </example>
    public static void ExecuteThreadInvoke(this System.Windows.Forms.Control control, Action code)
    {
      if (control.InvokeRequired)
      {
        control.Invoke(code);
        return;
      }
      code.Invoke();
    }

    /// <summary>Load JSON file into object</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T FileDeserialize<T>(string fileName)
    {
      return JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
    }


    /// <summary>Save object in JSON format</summary>
    /// <param name="o">Object to serialize</param>
    /// <param name="fileName">Output file path. If null, don't output to file</param>
    /// <param name="indent">Prettify output</param>
    /// <returns>JSON output data</returns>
    public static string FileSerialize(object o, string fileName = null, bool indent = true)
    {
      Formatting format = (indent) ? Formatting.Indented : Formatting.None;
      string data = JsonConvert.SerializeObject(o, format);
      if (fileName != null)
        File.WriteAllText(fileName, data);
      return data;
    }
  }
}
