/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    JsonTreeViewTests.cs
 * Description:
 *  Saved shortcuts to Json tests
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xeno.ToolsHub.Tests.SystemTests.Shortcuts
{
	[TestClass]
	public class JsonTreeViewTests
	{
		[TestMethod]
		public void TestParsing()
		{
		}

    private SettingsManager _settings = new SettingsManager(Config.StorageMethod.UnitTest);

    [TestInitialize]
    public void Initialize()
    {
      TestHelpers.PrepareTestsFolder();
      _settings.Clear();
    }
  }
}
