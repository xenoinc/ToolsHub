namespace Xeno.Dockzi.ExtensionModel.SystemTray;

/// <summary>Requests a custom tray icon, or resets it with null. The publisher retains ownership of the source icon.</summary>
public sealed record SystemTrayChangeIconRequest(System.Drawing.Icon Icon);
