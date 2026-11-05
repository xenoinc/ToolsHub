## Sidebar Launcher

* Launch apps, files, folders, Windows shortcuts, and web links from an icon dock.
* Drop items onto the dock to add them; drag icons to reorder them. Changes save immediately.
  While reordering, the grabbed icon follows the pointer and a blue marker shows its destination.
* Hover to enlarge an icon, click to launch, or right-click to add/remove launchers.
* The permanent **+** button opens the launcher picker and also accepts dropped apps.
  Right-click the dock or any launcher and choose **Sidebar settings…** to open its preference page.
* In **Settings > Sidebar**, choose the monitor and Left, Right, Top, or Bottom edge.
  Layout follows the selected edge. Long lists scroll with the mouse wheel.
* Enabling the Sidebar add-in shows it immediately; disabling it closes the dock.
  The startup visibility setting only controls display when the app starts.
* Enabling the Sidebar add-in shows it immediately; disabling it closes the dock.
  The startup visibility setting only controls display when the app starts.
* Sidebar preferences and launchers are stored in the `Sidebar` section of `Dockzi.config`.
  The Shortcuts add-in's list remains independent. Existing `SidebarLaunchers.json`
  entries import automatically when no sidebar list is saved in the database; the
  original file is retained as a backup. Dropped `.lnk` files keep their original target,
  arguments, and working directory; keep the link file at its saved location.

If Explorer shows a blocked cursor when dropping onto the dock, run Dockzi without
administrator privileges. When debugging, start Visual Studio normally too: Windows
[does not allow drag-and-drop from a non-elevated app into an elevated app](https://learn.microsoft.com/en-us/windows/win32/com/the-com-elevation-moniker).
