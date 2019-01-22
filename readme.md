# Xeno.ToolsHub
Flexible tools launcher desktop app which is "_not your average extensible sidebar_"

**_This project is a Work in Progress_**


|||
| --- | --- |
| **License** | [![GitHub license](https://img.shields.io/github/license/DamianSuess/ToolsHub.svg)](https://github.com/DamianSuess/ToolsHub/blob/master/LICENSE) |
| **Master** | <img src="https://ci.appveyor.com/api/projects/status/github/DamianSuess/ToolsHub?branch=master&svg=true" alt="Project Badge" /> |
| **Develop** | [![Build status](https://ci.appveyor.com/api/projects/status/k8breb3kj6d0fhwo/branch/develop?svg=true)](https://ci.appveyor.com/project/DamianSuess/toolshub/branch/develop) |

![Screenshot](docs/ScreenShots/SysTrayShortcuts.png)


## Active Features
* Launch app from System Tray
* Add-on extensions via [Mono.Addins](https://github.com/mono/mono-addins)


### Future Features
* Safely send shutdown signals to your add-ons
* Launch app from Sidebar (_i.e. RocketDock, ObjectDock_)
* Launch app on system startup

## Coding
The framework relies on [Mono.Addins](https://github.com/mono/mono-addins) for extending to add new features. We provide a list of key [extension points](https://github.com/xenoinc/ToolsHub/wiki/Addin-ExtensionPoints) you can attach to.

### Cloning

From a fresh start:

``git clone --recurse-submodules https://github.com/xenoinc/ToolsHub.git``


If closed and forgot the submodule:

``git submodule update --init --recursive``


### How to contribute
Read our wiki to learn how to help and make your own add-in.

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
