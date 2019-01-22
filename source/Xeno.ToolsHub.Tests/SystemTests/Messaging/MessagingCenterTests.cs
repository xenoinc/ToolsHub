/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-1-18
 * Author:  Damian Suess
 * File:    MessagingCenterTests.cs
 * Description:
 *  Messaging center tests
 */

namespace Xeno.ToolsHub.Tests.SystemTests.Messaging
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.ToolsHub.Services.Logging;
  using Xeno.ToolsHub.Services.Messaging;

  [TestClass]
  public class MessagingCenterTests
  {
    private const string KeySimple = "Simple";
    private const string KeyParamString = "ParamString";

    private int _msgCount = 0;
    private string _msgLastString = string.Empty;

    [TestInitialize]
    public void RunBeforeEachTest()
    {
      _msgCount = 0;
      _msgLastString = string.Empty;

      this.InitMessageSubscriptions();
    }

    [TestCleanup]
    public void RunAfterEachTest()
    {
      _msgCount = 0;
      _msgLastString = string.Empty;

      this.TeardownMessageSubscriptions();
    }

    [TestMethod]
    public void CanSendSepecifiedSimpleMessageCountTest()
    {
      for (int i = 1; i <= 3; i++)
      {
        MessagingCenter.Send<MessagingCenterTests>(this, KeySimple);
      }

      Assert.AreEqual(3, _msgCount);
    }

    public void CanSendParamedMessagesTest()
    {
      int sendCount = 5;

      for (int i = 1; i <= sendCount; i++)
      {
        MessagingCenter.Send<MessagingCenterTests, string>(this, KeyParamString, $"LastIndex={i}");
      }

      Assert.AreEqual(sendCount, _msgCount);
      Assert.AreEqual($"LastIndex={sendCount}", _msgLastString);
    }

    private void InitMessageSubscriptions()
    {
      MessagingCenter.Subscribe<MessagingCenterTests>(this, KeySimple, (sender) =>
      {
        Log.Debug("Someone messaged, 'SimpleKey'");
        _msgCount++;
      });

      MessagingCenter.Subscribe<MessagingCenterTests, string>(this, KeyParamString, (sender, arg) =>
      {
        Log.Debug($"Someone messaged, 'ParamStringKey' with: '{arg}'");
        _msgCount++;
        _msgLastString = arg;
      });
    }

    private void TeardownMessageSubscriptions()
    {
      MessagingCenter.Unsubscribe<MessagingCenterTests>(this, KeySimple);
      MessagingCenter.Unsubscribe<MessagingCenterTests, string>(this, KeyParamString);
    }
  }
}
