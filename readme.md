# Xeno.ToolsHub
Fexable tools launcher desktop app.

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
* Auto-mount drives on startup
* Safely dismount drive on system shutdown to guard against curruption


