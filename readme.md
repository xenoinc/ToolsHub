# Xeno.ToolsHub
Flexible tools launcher desktop app which is "_not your average extensible sidebar_"

**_This project is a Work in Progress_**


![CRAN](https://img.shields.io/cran/l/devtools.svg?style=plastic)
![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)



## Future Features
* Add-on containers via Mono.Addins
* Safely send shutdown signals to your add-ons
* Launch app from Sidebar (_i.e. RocketDock, ObjectDock_)
* Launch app from System Tray
* Launch app on system startup

## Coding
The framework relyes on Mono.Addins for extending to add new features. With that, there are key ``endpoints`` you can attach to.

### Endpoints
* OnStart
* OnShutdown


## Use case
### VeraCrypt
* **Status:** wip
* Auto-mount drives on startup
* Safely dismount drive on system shutdown to guard against curruption

### Sidebar Launcher
* **Status:** _n/a_

### SystemTray Launcher
* **Status:** _n/a_
