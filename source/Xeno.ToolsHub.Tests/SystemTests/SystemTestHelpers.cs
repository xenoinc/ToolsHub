﻿/* Copyright Xeno Innovations, Inc. 2018
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

  public static class SystemTestHelpers
  {
    /// <summary>Clear and create test folder</summary>
    public static void PrepareTestsFolder()
    {
      // WARNING: ONLY USE TEST!!
      Helpers.StorageMethod = StorageMethod.Test;

      try
      {
        System.IO.Directory.CreateDirectory(Helpers.StoragePath());
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
        if (System.IO.Directory.Exists(Helpers.StoragePath()))
          System.IO.Directory.Delete(Helpers.StoragePath(), true);
      }
      catch (Exception ex)
      {
        Assert.Fail($"Failed to delete UnitTest folder. {ex.Message}");
      }
    }
  }
}
