# ToolsHub Extension Points

## Endpoints

| Node Name | Path |
|---|---|
| StartupAddin | ToolsHub/AppStartup |
|  | ToolsHub/Sidebar |
|  | ToolsHub/SystemTray |

## Creating WPF Add-in
1. Create new project: WPF User Control
2. XML file change, ``UserControl`` to ``Window``
3. CS change, ``System.Windows.Controls.UserControl`` to ``System.Windows.Window``
    * Simply, ``UserControl`` to ``Window``
