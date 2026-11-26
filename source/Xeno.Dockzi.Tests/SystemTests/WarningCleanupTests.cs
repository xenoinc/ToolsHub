namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using Lite.EventIpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.ExtensionModel.SystemTray;
using Xeno.Dockzi.PomodoroAddin.Domain;
using Xeno.Dockzi.Services.Messaging;
using Xeno.Dockzi.VeraCryptAddin.Domain;

[TestClass]
[OSCondition(OperatingSystems.Windows)]
public class WarningCleanupTests
{
  [TestMethod]
  public void VeraCryptSettingsRetainTheExistingDpapiFormat()
  {
    byte[] entropy = { 8, 3, 198, 33, 7, 83, 1, 235 };
    const string PlainText = "Dockzi compatibility test";
    var crypto = new Crypto { ProtectionScope = DataProtectionScope.CurrentUser };
    string saved = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(PlainText), entropy, DataProtectionScope.CurrentUser));
    Assert.AreEqual(PlainText, crypto.Decrypt(saved));

    byte[] restored = ProtectedData.Unprotect(Convert.FromBase64String(crypto.Encrypt(PlainText)), entropy, DataProtectionScope.CurrentUser);
    Assert.AreEqual(PlainText, Encoding.UTF8.GetString(restored));
  }

  [TestMethod]
  public void QueuedTrayIconSurvivesPublisherDisposingItsOriginal()
  {
    var events = new EventAggregator();
    var queued = new List<Action>();
    Icon received = null;
    using var subscriptions = new TrayEventSubscriptions(events, () => { }, _ => { }, icon => received = icon, queued.Add);
    using (var original = new IconGenerator("5").ToIcon())
      events.Publish(new SystemTrayChangeIconRequest(original));

    Assert.HasCount(1, queued);
    queued[0]();
    using (received)
    using (var bitmap = received.ToBitmap())
      Assert.AreEqual(new Size(16, 16), bitmap.Size);
  }
}
