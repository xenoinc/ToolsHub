/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-27
 * Author:  Damian Suess
 * File:    SystemTestHelpers.cs
 * Description:
 *  Basic tests helper
 */

namespace Xeno.ToolsHub.Tests.SystemTests
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.ToolsHub.Config;

  public static class TestHelpers
  {
    /// <summary>Clear and create test folder</summary>
    public static void PrepareTestsFolder()
    {
      // WARNING: ONLY USE TEST!!
      Managers.Settings.StorageMethod = StorageMethod.Unknown;

      try
      {
        System.IO.Directory.CreateDirectory(Helpers.GetStorageFolder(StorageMethod.UnitTest));
      }
      catch (Exception ex)
      {
        Assert.Fail($"Failed to create UnitTest folder. {ex.Message}");
      }
    }

    /// <summary>Remove all documents from test folder</summary>
    public static void CleanTestsFolder()
    {
      try
      {
        if (System.IO.Directory.Exists(Helpers.GetStorageFolder(StorageMethod.UnitTest)))
          System.IO.Directory.Delete(Helpers.GetStorageFolder(StorageMethod.UnitTest), true);
      }
      catch (Exception ex)
      {
        Assert.Fail($"Failed to delete UnitTest folder. {ex.Message}");
      }
    }
  }
}
