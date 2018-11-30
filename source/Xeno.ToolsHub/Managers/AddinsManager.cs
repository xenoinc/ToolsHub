/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    AddinManager.cs
 * Description:
 *  Manager for system wide add-ins
 */

using System;
using System.Collections.Generic;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.Managers
{
  public class AddinsManager
  {
    //private static readonly Log _log = myLogger.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    ///   Key = TypeExtensionNode.Id
    /// </summary>
    private Dictionary<string, UtilityAddin> _appAddins;

    public AddinsManager() : this("")
    {
    }

    /// <summary>Constructor providing alternative add-in directory</summary>
    /// <param name="configDir">Add-ins configuration directory</param>
    public AddinsManager(string configDir)
    {
      _appAddins = new Dictionary<string, UtilityAddin>();

      InitAddins();
    }

    public event EventHandler OnApplicationAddinListChanged;

    public PreferenceAddin[] GetPreferenceAddins()
    {
      PreferenceAddin[] addins;

      try
      {
        addins = (PreferenceAddin[])Mono.Addins.AddinManager.GetExtensionObjects(
          ExtensionPaths.PreferencePath, typeof(PreferenceAddin));
      }
      catch (Exception ex)
      {
        Log.Warn($"No perferenceAddins found '{ex.Message}'");
        addins = new PreferenceAddin[0];
      }

      return addins;
    }

    private void InitAddins()
    {
      Log.Info("Initializing Mono.Addins");

      Mono.Addins.AddinManager.AddinLoaded += OnMonoAddinLoaded;
      Mono.Addins.AddinManager.AddinUnloaded += OnMonoAddinUnloaded;

      Mono.Addins.AddinManager.Initialize(".");

      // Rebuild registry when debugging
      if (Helpers.IsDebugging)
        Mono.Addins.AddinManager.Registry.Rebuild(null);
      else
        Mono.Addins.AddinManager.Registry.Update();

      //InitExtensions(true);

      try
      {
        // EventHandlers for ExtensionNodes
        Mono.Addins.AddinManager.AddExtensionNodeHandler("/TestApp33/StartupHandler", OnStartupAddins_ExtensionHandler);
      }
      catch (Exception ex)
      {
        Log.Error("Could not register one or more ExtensionPoints. Possibly could not find XML manifest.");
        Log.Error("Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);

        // System.Windows.Forms.MessageBox.Show("BoOOO!!");
        throw new Exception("Unable to add extension handler.", ex);
      }
    }

    /// <summary>
    ///   Used for debugging Mono.Addins
    /// </summary>
    /// <param name="safely"></param>
    private void InitExtensions(bool safely = true)
    {
      if (!safely)
      {
        Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPaths.OnStartupAddinsPath, OnStartupAddins_ExtensionHandler);
      }
      else
      {
        try
        {
          // EventHandlers for ExtensionNodes
          Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPaths.OnStartupAddinsPath, OnStartupAddins_ExtensionHandler);
        }
        catch (Exception ex)
        {
          Log.Error("Could not register one or more ExtensionPoints. Possibly could not find XML manifest.");
          Log.Error("Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);

          throw new Exception("Unable to add extension handler.", ex);
        }
      }
    }

    private void OnMonoAddinLoaded(object sender, Mono.Addins.AddinEventArgs args)
    {
      Mono.Addins.Addin addin = Mono.Addins.AddinManager.Registry.GetAddin(args.AddinId);
      Log.Debug($"=============================");
      Log.Debug($"OnAddinLoaded: {args.AddinId}");
      Log.Debug($"         Name: '{addin.Name}'");
      Log.Debug($"  Description: '{addin.Description.Description}'");
      Log.Debug($"    Namespace: '{addin.Namespace}'");
      Log.Debug($"      Enabled: '{addin.Enabled}'");
      Log.Debug($"         File: '{addin.AddinFile}'");
      Log.Debug("= = = = = = = = = = = = =");
    }

    private void OnMonoAddinUnloaded(object sender, Mono.Addins.AddinEventArgs args)
    {
      Log.Debug($"Add-in Id: {args.AddinId}");
    }

    private void OnStartupAddins_ExtensionHandler(object sender, Mono.Addins.ExtensionNodeEventArgs args)
    {
      Log.Debug("Entering");

      Mono.Addins.TypeExtensionNode extNode = args.ExtensionNode as Mono.Addins.TypeExtensionNode;

      Log.Debug("###########################");
      Log.Debug("OnStartChanged");
      Log.Debug($"  Id      - '{args.ExtensionNode.Id}'");
      Log.Debug($"  Path    - '{args.Path}'");
      Log.Debug($"  Node    - '{args.ExtensionNode}'");
      Log.Debug($"  Object  - '{args.ExtensionObject}'");
      Log.Debug($"  Changed - '{args.Change.ToString()}'");
      Log.Debug("   --[ ExtensionNode ]------");
      Log.Debug($"  Id      - '{extNode.Id}'");
      Log.Debug($"  ToString- '{extNode.ToString()}'");
      Log.Debug($"  TypeName- '{extNode.TypeName}'");

      // Execute via class interface definition of extension path
      //IStartupExtension ext = (IStartupExtension)args.ExtensionObject;
      //ext.Execute();

      UtilityAddin addin;
      if (args.Change == Mono.Addins.ExtensionChange.Add)
      {
        // Load add-in
        addin = extNode.GetInstance(typeof(UtilityAddin)) as UtilityAddin;
        if (addin != null)
        {
          try
          {
            addin.Initialize();
            _appAddins[extNode.Id] = addin;
          }
          catch (Exception loadEx)
          {
            Log.Error($"Issue initializing add-in, '{addin.GetType().ToString()}' " +
                      $"with exception: {loadEx.Message}{Environment.NewLine}StackTrace: {loadEx.StackTrace}");
          }
        }
      }
      else
      {
        // Remove add-in from app cache
        if (_appAddins.ContainsKey(extNode.Id))
        {
          addin = _appAddins[extNode.Id];
          try
          {
            addin.Shutdown();
          }
          catch (Exception unloadEx)
          {
            Log.Error($"Issue shutting down add-in, '{addin.GetType().ToString()}' " +
                      $"with exception: {unloadEx.Message}{Environment.NewLine}StackTrace: {unloadEx.StackTrace}");
          }
          finally
          {
            _appAddins.Remove(extNode.Id);
          }
        }
      }

      // Push event changed out to listeners
      OnApplicationAddinListChanged?.Invoke(sender, args);
    }
  }
}
