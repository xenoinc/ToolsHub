# Overview
The VeraCrypt add-in aims to provide accessability for mounting and dismounting virtual drives on demand.

We want to provide you with an easy to use drive management system, and not go too overboard. Security should be easy and intutive, so let us do the heavy lifting for you.

## System tray

The VeraCrypt submenu refreshes its drive state each time it opens. Mount is enabled when VeraCrypt is installed, a volume path is configured, and the selected drive letter is free. Dismount is enabled only when that letter currently maps to a VeraCrypt volume. Commands recheck the state when clicked.

**Open in Explorer** lists all detected VeraCrypt-mounted drive letters, including volumes mounted outside Dockzi. It is disabled when none are mounted. Selecting a drive opens its root in Windows Explorer; browsing does not require the VeraCrypt executable to remain installed. The current settings still configure one volume for Mount/Dismount.

## Feature Wish-List
We can't provide all features at once. Below is a quick wish-list of basic feature we want to provide you from with-in

* Manage multiple drives
* Single click to mount drives
* Auto-sync (dis)mounted drive status in Shortcuts add-in
* Master password wrapper, so you don't have to recall each one while staying secure
* Auto-install VeraCrypt drivers
* Auto-discover previous install of VeraCrypt
* Drive creation wizard
* Auto-mount on app start
* Auto-dismount on system shutdown
* Auto-dismount on log-off
* Auto-dismount after x-minutes (_meh?_)
* Set encryption methods/strength for storing passwords

## Short-term To-do
* [ ] SysTray's ``target`` use a GUID index from Dockzi settings to link to the .HC file
