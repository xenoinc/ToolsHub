/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-1-18
 * Author:  Damian Suess
 * File:    ExampleTestFixture.cs
 * Description:
 *  Example shit class configuration
 */

namespace Xeno.ToolsHub.Tests
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.ToolsHub.Services.Logging;

  [TestClass]
  public class ExampleTestFixture
  {
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
      // testContext.TestName
      Log.Debug("Assembly initialize - Name: " + context.TestName);
    }

    [AssemblyCleanup]
    public static void AssemblyCleanUp()
    {
      Log.Debug("Assembly Cleaned up");
    }

    [ClassInitialize]
    public static void RunBeforeAllTestsInClass(TestContext context)
    {
      // string name = "ClassInit: " + context.TestName;
    }

    [ClassCleanup]
    public void RunAfterAllTestsInClass()
    {
    }

    [TestInitialize]
    public void RunBeforeEachTest()
    {
    }

    [TestCleanup]
    public void RunAfterEachTest()
    {
    }
  }
}
