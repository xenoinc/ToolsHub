# VeraCrypt Manager

## Features
* Auto-mount drives on startup
* Safely dismount drives on shutdown
* VeraCrypt quick-manager from SysTray


## Shutdown signal

48
down vote
accepted
Attach an event handler method to the SystemEvents.SessionEnding event, and your handler method will be called each time the event is raised. Handling this event will allow you to cancel the pending log off or shut down, if you wish. (Although that doesn't actually work like it sounds in current operating systems; for more information see the MSDN documentation here.)

If you don't want to cancel the event, but just react to it appropriately, you should handle the SystemEvents.SessionEnded event instead.

You must make sure that you detach your event handlers when the application is closed, however, because both of these are static events.

shareeditflag