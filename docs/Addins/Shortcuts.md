## Sidebar Launcher

* Launch apps, files, folders, Windows shortcuts, and web links from an icon dock.
* Drop items onto the dock to add them; drag icons to reorder them. Changes save immediately.
  While reordering, the grabbed icon follows the pointer and a blue marker shows its destination.
* Hover to enlarge an icon, click to launch, or right-click to add/remove launchers.
* In **Settings > Sidebar**, choose the monitor and Left, Right, Top, or Bottom edge.
  Layout follows the selected edge. Long lists scroll with the mouse wheel.
* Enabling the Sidebar add-in shows it immediately; disabling it closes the dock.
  The startup visibility setting only controls display when the app starts.
* Enabling the Sidebar add-in shows it immediately; disabling it closes the dock.
  The startup visibility setting only controls display when the app starts.
* Launchers are stored separately in `SidebarLaunchers.json`, beside `ToolsHub.db`.
  The Shortcuts add-in's list is independent. Dropped `.lnk` files keep their original
  target, arguments, and working directory; keep the link file at its saved location.

If Explorer shows a blocked cursor when dropping onto the dock, run ToolsHub without
administrator privileges. When debugging, start Visual Studio normally too: Windows
[does not allow drag-and-drop from a non-elevated app into an elevated app](https://learn.microsoft.com/en-us/windows/win32/com/the-com-elevation-moniker).
