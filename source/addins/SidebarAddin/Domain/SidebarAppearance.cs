namespace Xeno.ToolsHub.SidebarAddin.Domain;

using Xeno.ToolsHub.Services;

public sealed record SidebarAppearance(int ScreenArea, int ScreenNumber, int Opacity, bool OnTop, bool AutoHide)
{
  public static SidebarAppearance Load(ISettingsService settings) => new(
    settings.GetInt(Constants.AddinId, Constants.KeyScreenArea, 1),
    settings.GetInt(Constants.AddinId, Constants.KeyScreenNumber, 1),
    settings.GetInt(Constants.AddinId, Constants.KeyOpacity, 90),
    settings.GetBool(Constants.AddinId, "OnTop", true),
    settings.GetBool(Constants.AddinId, "AutoHide", true));
}
