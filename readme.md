# Xeno.ToolsHub
Flexible tools launcher desktop app which is "_not your average extensible sidebar_"

**_This project is a Work in Progress_**


|||
| --- | --- |
| **License** | [![GitHub license](https://img.shields.io/github/license/DamianSuess/ToolsHub.svg)](https://github.com/DamianSuess/ToolsHub/blob/master/LICENSE) |
| **Master** | <img src="https://ci.appveyor.com/api/projects/status/github/DamianSuess/ToolsHub?branch=master&svg=true" alt="Project Badge" /> |
| **Develop** | <img src="https://ci.appveyor.com/api/projects/status/github/DamianSuess/ToolsHub?branch=develop&svg=true" alt="Project Badge" /> |


## Active Features
* Launch app from System Tray
* Add-on extensions via [Mono.Addins](https://github.com/mono/mono-addins)


### Future Features
* Safely send shutdown signals to your add-ons
* Launch app from Sidebar (_i.e. RocketDock, ObjectDock_)
* Launch app on system startup

## Coding
The framework relies on [Mono.Addins](https://github.com/mono/mono-addins) for extending to add new features. We provide a list of key [extension points](https://github.com/xenoinc/ToolsHub/wiki/Addin-ExtensionPoints) you can attach to.

### Endpoints
* ``OnStart`` - _Called when ToolsHub starts up_
* ``OnSystemShutdown`` - _Called when Windows sends it's shutdown signal_
* ``Utility`` - _Disposable add-ins called after ToolsHub is fully launced_


## Use case
### SystemTray Launcher
* **Status:** Ready
* Launch shortcuts (_apps, folders, urls_) directly from SystemTray

### Sidebar Launcher
* **Status:** _n/a_
* (_i.e. RocketDock, ObjectDock_)

### VeraCrypt
* **Status:** WiP
* Auto-mount drives on startup
* Safely dismount drive on system shutdown to guard against corruption
