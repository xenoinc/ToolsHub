﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-3
 * Author:  Damian Suess
 * File:    AddinUtilityHandler.cs
 * Description:
 *  SampleXml Utility extension
 */

namespace Xeno.ToolsHub.SampleXmlAddin.Handlers
{
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services.Logging;

  public class UtilityHandler : UtilityAddin
  {
    private bool _initialized = false;

    public UtilityHandler()
    {
      _initialized = true;

      Log.Debug("External SampleXml utility add-in: Initializing");
    }

    public override bool IsInitialized => _initialized;

    public override void Execute()
    {
      Log.Debug("External SampleXml utility add-in: Executed!");

      ////if (Helpers.IsDebugging)
      ////  MessageBox.Show("External SampleXml utility add-in: Executed!");
    }

    public override void Shutdown()
    {
      Log.Debug("External SampleXml utility add-in: Shutting down!");

      ////if (Helpers.IsDebugging)
      ////  MessageBox.Show("External SampleXml utility add-in: Shutting down!");
    }
  }
}
