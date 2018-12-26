alt

```cs
var Startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
```

## File Path

Visible from task manager, Running on current user login success, No admin privileges required
```
C:\Users\USER_NAME\AppData\Roaming\Microsoft\Windows\Start
  Menu\Programs\Startup
```

Visible from task manager, Running on all user login success, Admin privileges required
```
C:\Users\Default\AppData\Roaming\Microsoft\Windows\Start
  Menu\Programs\Startup
```

## Reg

```cs
using Microsoft.Win32;
private void SetStartup()
{
  RegistryKey rk = Registry.CurrentUser.OpenSubKey
      ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

  if (chkStartUp.Checked)
    rk.SetValue(AppName, Application.ExecutablePath);
  else
    rk.DeleteValue(AppName,false);
}
```

Registry path
HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run (Visible from task manager, Running on current user login success, No admin privileges required)

HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunOnce (Not visible from task manager, Running on current user login success, Running for one login time, No admin privileges required)

HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run (Visible from task manager, Running on all user login success, Admin privileges required)

HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce (Not visible from task manager, Running on all user login success, Running for one login time, Admin privileges required)

