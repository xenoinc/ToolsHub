/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    AddinManager.cs
 * Description:
 *  Manager for system wide add-ins
 */

namespace Xeno.Dockzi.Managers;

using System;
using System.Collections.Generic;
using System.IO;
using Mono.Addins;
using Xeno.Dockzi.Config;
using Xeno.Dockzi.ExtensionModel;
using Xeno.Dockzi.Services.Logging;

public class AddinsManager : IDisposable
{
  private readonly string _configDir;
  private readonly Func<TypeExtensionNode, object> _activate;

  /// <remarks>Key = TypeExtensionNode.Id</remarks
  private Dictionary<string, UtilityAddin> _utilityAddins;

  private bool _verboseErrorMessage = true;
  private bool _utilitiesStarted;
  private bool _disposed;

  /// <summary>Initializes a new instance of the <see cref="AddinsManager"/> class.</summary>
  public AddinsManager()
    : this(string.Empty)
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="AddinsManager"/> class
  ///   providing an alternative add-in directory.
  /// </summary>
  /// <param name="activate">The host's add-in activation boundary.</param>
  public AddinsManager(Func<TypeExtensionNode, object> activate)
    : this(string.Empty, activate)
  {
  }

  public AddinsManager(string configDir)
    : this(configDir, node => node.CreateInstance())
  {
  }

  public AddinsManager(string configDir, Func<TypeExtensionNode, object> activate)
  {
    _activate = activate ?? throw new ArgumentNullException(nameof(activate));
    _utilityAddins = new Dictionary<string, UtilityAddin>();
    _configDir = string.IsNullOrWhiteSpace(configDir)
      ? AppContext.BaseDirectory
      : Path.GetFullPath(configDir);

    InitAddins();
  }

  public event EventHandler OnApplicationAddinListChanged;

  public object CreateAddin(TypeExtensionNode node) => _activate(node);

  public void Dispose()
  {
    if (_disposed)
      return;
    _disposed = true;
    Mono.Addins.AddinManager.AddinLoaded -= OnAddinLoaded;
    Mono.Addins.AddinManager.AddinUnloaded -= OnAddinUnloaded;
    Mono.Addins.AddinManager.RemoveExtensionNodeHandler(ExtensionPath.OnStartup, OnStartupAddins_ExtensionHandler);
    Mono.Addins.AddinManager.RemoveExtensionNodeHandler(ExtensionPath.SystemTray, OnSystemTrayAddins_ExtensionHandler);
    Mono.Addins.AddinManager.RemoveExtensionNodeHandler(ExtensionPath.Utility, OnUtilityAddins_ExtensionHandler);
    foreach (string id in new List<string>(_utilityAddins.Keys))
      OnDisableAddin(id);
  }

  /// <summary>Get all add-ins found in system</summary>
  /// <returns>List of add-ins.</returns>
  public List<Mono.Addins.Addin> GetAllAddins()
  {
    List<Mono.Addins.Addin> addinList = new List<Mono.Addins.Addin>();
    Mono.Addins.Addin[] addinArray = Mono.Addins.AddinManager.Registry.GetAddins();

    if (addinArray != null)
      addinList = new List<Mono.Addins.Addin>(addinArray);

    return addinList;
  }

  public List<IPreferencePageExtension> GetPreferenceAddins(Func<TypeExtensionNode, object> activate = null)
  {
    List<IPreferencePageExtension> pages = new List<IPreferencePageExtension>();
    Mono.Addins.ExtensionNodeList nodes = Mono.Addins.AddinManager.GetExtensionNodes(ExtensionPath.PreferencePage);
    foreach (Mono.Addins.ExtensionNode node in nodes)
    {
      Mono.Addins.TypeExtensionNode typeNode = node as Mono.Addins.TypeExtensionNode;
      try
      {
        IPreferencePageExtension page = (activate ?? _activate)(typeNode) as IPreferencePageExtension;
        page.InitializePage();
        page.Id = node.Id;
        pages.Add(page);
      }
      catch (Exception ex)
      {
        Log.Error("Couldn't create PreferencePage: " + ex.Message);
      }
    }

    return pages;
  }

  /// <summary>Load utility add-ins</summary>
  public void LoadUtilityAddins()
  {
    ObjectDisposedException.ThrowIf(_disposed, this);
    _utilitiesStarted = true;
    Mono.Addins.ExtensionNodeList nodes = Mono.Addins.AddinManager.GetExtensionNodes(ExtensionPath.Utility);
    foreach (Mono.Addins.ExtensionNode node in nodes)
    {
      Mono.Addins.TypeExtensionNode typeNode = node as Mono.Addins.TypeExtensionNode;

      try
      {
        if (_utilityAddins.ContainsKey(typeNode.Id))
          continue;
        UtilityAddin utility = CreateAddin(typeNode) as UtilityAddin;

        LoadUtilityAddin(utility, typeNode.Id, typeNode.TypeName);
      }
      catch (Exception ex)
      {
        Log.Error("Couldn't create UtilityAddin: " + ex.Message);
      }
    }
  }

  private static bool IsMissingExtensionPointException(Exception ex)
  {
    return ex is InvalidOperationException
      && ex.Message.IndexOf("Extension node not found in path", StringComparison.OrdinalIgnoreCase) >= 0;
  }

  /// <summary>
  ///   Load UtilityAddin and store in memory
  /// </summary>
  /// <param name="addin">Add-in to load.</param>
  /// <param name="addinId">Add-in Id.</param>
  /// <param name="addinTypeName">Add-in assembly type name.</param>
  /// <param name="enabledByUser">True for explicit activation after startup.</param>
  private void LoadUtilityAddin(UtilityAddin addin, string addinId, string addinTypeName, bool enabledByUser = false)
  {
    try
    {
      if (_utilityAddins.ContainsKey(addinId))
        return;
      if (addin.IsInitialized)
      {
        _utilityAddins.Add(addinId, addin);
        if (enabledByUser)
          addin.OnEnabled();
        else
          addin.Execute();
      }
      else
        Log.Debug("Add-in not initialized; not executing.");
    }
    catch (Exception ex)
    {
      if (_utilityAddins.TryGetValue(addinId, out var active) && ReferenceEquals(active, addin))
        OnDisableAddin(addinId);
      string msg = $"Error while attempting to initialize UtilityAddin, Id: '{addinId}', TypeName: '{addinTypeName}': {ex.Message}";
      Log.Error(msg);

      // TODO: Use custom pop-up notification, versus a Modal
      if (_verboseErrorMessage)
        System.Windows.Forms.MessageBox.Show(msg);
    }
  }

  private void InitAddins()
  {
    Log.Info("Initializing Mono.Addins");

    Mono.Addins.AddinManager.AddinLoaded += OnAddinLoaded;
    Mono.Addins.AddinManager.AddinUnloaded += OnAddinUnloaded;

    Mono.Addins.AddinManager.Initialize(_configDir, _configDir, _configDir);

    // Rebuild registry when debugging.
    if (Helpers.IsDebugging)
      Mono.Addins.AddinManager.Registry.Rebuild(null);
    else
      Mono.Addins.AddinManager.Registry.Update();

    try
    {
      RegisterExtensionNodeHandlers();
    }
    catch (Exception ex)
    {
      if (IsMissingExtensionPointException(ex))
      {
        Log.Info("Mono.Addins extension points were missing; rebuilding registry.");
        Mono.Addins.AddinManager.Registry.Rebuild(null);

        try
        {
          RegisterExtensionNodeHandlers();
          return;
        }
        catch (Exception retryEx)
        {
          ex = retryEx;
        }
      }

      Log.Error("Could not register one or more ExtensionPoints. Possibly could not find XML manifest.");
      Log.Error("Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);

      throw new Exception("Unable to add extension handler.", ex);
    }
  }

  private void RegisterExtensionNodeHandlers()
  {
    // EventHandlers for ExtensionNodes
    Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPath.OnStartup, OnStartupAddins_ExtensionHandler);
    //// TODO: Do we need to reenable the PreferencePageManager?
    //// Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPath.PreferencePage, OnPreferencesAddins_ExtensionHandler);
    Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPath.SystemTray, OnSystemTrayAddins_ExtensionHandler);
    Mono.Addins.AddinManager.AddExtensionNodeHandler(ExtensionPath.Utility, OnUtilityAddins_ExtensionHandler);
  }

  private void OnAddinLoaded(object sender, Mono.Addins.AddinEventArgs args)
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

  private void OnAddinUnloaded(object sender, Mono.Addins.AddinEventArgs args)
  {
    Log.Debug($"Add-in Id: {args.AddinId}");
  }

  private void OnDisableAddin(string extensionNodeId)
  {
    if (!_utilityAddins.Remove(extensionNodeId, out var utility))
      return;
    try
    {
      utility.Shutdown();
    }
    catch (Exception ex)
    {
      Log.Error($"Error shutting down utility '{extensionNodeId}': {ex.Message}");
    }
  }

  private void OnStartupAddins_ExtensionHandler(object sender, Mono.Addins.ExtensionNodeEventArgs args)
  {
    Log.Debug("Entering");

    Mono.Addins.TypeExtensionNode extNode = args.ExtensionNode as Mono.Addins.TypeExtensionNode;
    PrintInfo(ExtensionPath.OnStartup, args, extNode);

    // Execute via class interface definition of extension path
    // IOnStartupExtension ext = (IOnStartupExtension)args.ExtensionObject;
    IOnStartupExtension addin = CreateAddin(extNode) as IOnStartupExtension;
    addin.Execute();

    // Push event changed out to listeners
    OnApplicationAddinListChanged?.Invoke(sender, args);
  }

  private void OnPreferencesAddins_ExtensionHandler(object sender, ExtensionNodeEventArgs args)
  {
  }

  private void OnSystemTrayAddins_ExtensionHandler(object sender, ExtensionNodeEventArgs args)
  {
  }

  private void OnUtilityAddins_ExtensionHandler(object sender, ExtensionNodeEventArgs args)
  {
    if (args.Change == Mono.Addins.ExtensionChange.Add)
      OnUtilityAddinEnable(args);
    else
      OnUtilityAddinDisable(args);
  }

  private void OnUtilityAddinDisable(Mono.Addins.ExtensionNodeEventArgs args)
  {
    Mono.Addins.TypeExtensionNode node = args.ExtensionNode as Mono.Addins.TypeExtensionNode;
    try
    {
      OnDisableAddin(node.Id);
    }
    catch (Exception ex)
    {
      Log.Error("Error unloading add-in: " + ex.Message);
    }
  }

  private void OnUtilityAddinEnable(Mono.Addins.ExtensionNodeEventArgs args)
  {
    if (!_utilitiesStarted || _disposed || args.ExtensionNode is not TypeExtensionNode node
      || _utilityAddins.ContainsKey(node.Id))
      return;
    try
    {
      LoadUtilityAddin(CreateAddin(node) as UtilityAddin, node.Id, node.TypeName, enabledByUser: true);
    }
    catch (Exception ex)
    {
      Log.Error($"Error enabling utility '{node.Id}': {ex.Message}");
    }
  }

  private void OnUtilityHandle_Example()
  {
    ////Mono.Addins.TypeExtensionNode extNode = args.ExtensionNode as Mono.Addins.TypeExtensionNode;
    ////PrintInfo(ExtensionPaths.UtilityPath, args, extNode);
    ////
    ////UtilityAddin addin;
    ////if (args.Change == Mono.Addins.ExtensionChange.Add)
    ////{
    ////  // Load add-in
    ////  addin = extNode.GetInstance(typeof(UtilityAddin)) as UtilityAddin;
    ////  if (addin != null)
    ////  {
    ////    try
    ////    {
    ////      addin.Initialize();
    ////      _appAddins[extNode.Id] = addin;
    ////      addin.Execute();
    ////    }
    ////    catch (Exception loadEx)
    ////    {
    ////      Log.Error($"Issue initializing add-in, '{addin.GetType().ToString()}' " +
    ////                $"with exception: {loadEx.Message}{Environment.NewLine}StackTrace: {loadEx.StackTrace}");
    ////    }
    ////  }
    ////}
    ////else
    ////{
    ////  // Remove add-in from app cache
    ////  if (_appAddins.ContainsKey(extNode.Id))
    ////  {
    ////    addin = _appAddins[extNode.Id];
    ////    try
    ////    {
    ////      addin.Shutdown();
    ////    }
    ////    catch (Exception unloadEx)
    ////    {
    ////      Log.Error($"Issue shutting down add-in, '{addin.GetType().ToString()}' " +
    ////                $"with exception: {unloadEx.Message}{Environment.NewLine}StackTrace: {unloadEx.StackTrace}");
    ////    }
    ////    finally
    ////    {
    ////      _appAddins.Remove(extNode.Id);
    ////    }
    ////  }
    ////
    ////// Push event changed out to listeners
    ////OnApplicationAddinListChanged?.Invoke(sender, args);
    ////}
  }

  private void PrintInfo(string extPoint, ExtensionNodeEventArgs args, Mono.Addins.TypeExtensionNode extNode)
  {
    Log.Debug("###########################");
    Log.Debug($"'{extPoint}'");
    Log.Debug($"  Id      - '{args.ExtensionNode.Id}'");
    Log.Debug($"  Path    - '{args.Path}'");
    Log.Debug($"  Node    - '{args.ExtensionNode}'");
    Log.Debug($"  Object  - '{extNode.TypeName}'");
    Log.Debug($"  Changed - '{args.Change.ToString()}'");
    Log.Debug("   --[ ExtensionNode ]------");
    Log.Debug($"  Id      - '{extNode.Id}'");
    Log.Debug($"  ToString- '{extNode.ToString()}'");
    Log.Debug($"  TypeName- '{extNode.TypeName}'");
  }
}
