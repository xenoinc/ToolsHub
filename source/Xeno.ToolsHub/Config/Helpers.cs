/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-6
 * Author:  Damian Suess
 * File:    Helpers.cs
 * Description:
 *  Global helper methods. Code does not necessarily rely on other internal classes
 */

namespace Xeno.ToolsHub.Config
{
  using System;
  using Newtonsoft.Json;

  public static class Helpers
  {
    /// <summary>Gets a value indicating whether we're in debug mode or not</summary>
    /// <value>True or false</value>
    public static bool IsDebugging => System.Diagnostics.Debugger.IsAttached;

    /// <summary>Gets or sets the app's default storage method</summary>
    /// <value>The app's default storage method</value>
    public static StorageMethod StorageMethod { get; set; }

    /// <summary>Get default storage path</summary>
    /// <param name="fileName">OPTIONA: File name to include inpath</param>
    /// <returns>Folder path to base storage or with included file</returns>
    public static string StoragePath(string fileName = "")
    {
      string pth = string.Empty;
      pth = GetStoragePath(StorageMethod);

      // Append file to path
      if (!string.IsNullOrEmpty(fileName))
        pth = System.IO.Path.Combine(pth, fileName);

      return pth;
    }

    /// <summary>Gets a specified storage path</summary>
    /// <param name="storageMethod">Method of storage</param>
    /// <returns>Directory path based on storage method</returns>
    public static string GetStoragePath(StorageMethod storageMethod)
    {
      string path = string.Empty;

      //// -- Sample --
      //// Linux / Mac:
      ////  C:\Users\USERNAME\Documents\
      ////  path = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

      switch (storageMethod)
      {
        case StorageMethod.PortableApp:
          path = System.IO.Directory.GetCurrentDirectory();
          break;

        case StorageMethod.AllUsers:
          path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
          path = System.IO.Path.Combine(path, "XI", "ToolsHub");
          break;

        case StorageMethod.SingleUser:
          // We're using local over roaming because we don't want settings uploaded to server if user is on a domain.
          path = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
          path = System.IO.Path.Combine(path, "XI", "ToolsHub");
          break;

        case StorageMethod.Unknown:
        case StorageMethod.UnitTest:
        default:
          //// pth = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
          path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ToolsHubTests");
          break;
      }

      return path;
    }

    /// <summary>
    ///   Execute on UI thread asynchronously (don't wait for completion)
    /// </summary>
    /// <param name="control">Parent object</param>
    /// <param name="code">Code to execute</param>
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
    /// <param name="control">Parent</param>
    /// <param name="code">Code to execute</param>
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
    /// <typeparam name="T">Object type</typeparam>
    /// <param name="fileName">File name</param>
    /// <returns>Object file to convert JSON to</returns>
    public static T FileDeserialize<T>(string fileName)
    {
      return JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(fileName));
    }

    /// <summary>Save object in JSON format</summary>
    /// <param name="o">Object to serialize</param>
    /// <param name="fileName">Output file path. If null, don't output to file</param>
    /// <param name="indent">Prettify output</param>
    /// <returns>JSON output data</returns>
    public static string FileSerialize(object o, string fileName = null, bool indent = true)
    {
      Formatting format = indent ? Formatting.Indented : Formatting.None;
      string data = JsonConvert.SerializeObject(o, format);
      if (fileName != null)
        System.IO.File.WriteAllText(fileName, data);
      return data;
    }
  }
}
