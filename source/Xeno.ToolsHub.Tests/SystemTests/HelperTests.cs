/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-27
 * Author:  Damian Suess
 * File:    HelperTests.cs
 * Description:
 *  Helpers tests
 */

namespace Xeno.ToolsHub.Tests.SystemTests
{
  using System.Linq;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.ToolsHub.Config;

  [TestClass]
  public class HelperTests
  {
    [TestMethod]
    public void OutputPathTests()
    {
      string path = string.Empty;

      path = Helpers.GetStorageFolder(StorageMethod.AllUsers);
      Assert.IsNotNull(path);
      Log.Debug($"AllUsers: {path}");

      path = Helpers.GetStorageFolder(StorageMethod.SingleUser);
      Assert.IsNotNull(path);
      Log.Debug($"SingleUser: {path}");

      path = Helpers.GetStorageFolder(StorageMethod.PortableApp);
      Assert.IsNotNull(path);
      Log.Debug($"PortableApp: {path}");

      path = Helpers.GetStorageFolder(StorageMethod.Unknown);
      Assert.IsNotNull(path);
      Log.Debug($"Unknown: {path}");

      path = Helpers.GetStorageFolder(StorageMethod.UnitTest);
      Assert.IsNotNull(path);
      Log.Debug($"UnitTest: {path}");
    }

    [TestMethod]
    public void OutputPathsTest()
    {
      // Used for debugging where our folders point to
      var values = GetValues<System.Environment.SpecialFolder>();
      foreach (var e in values)
      {
        string path = System.Environment.GetFolderPath(e);
        Log.Debug("| " + e.ToString() + " | " + path + " |");
      }
    }

    private static System.Collections.Generic.IEnumerable<T> GetValues<T>()
    {
      return System.Enum.GetValues(typeof(T)).Cast<T>();
    }
  }
}
