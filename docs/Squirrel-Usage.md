Copyright 2017 Omnicell, Inc.

# Squirrel Updater

## Create Installer (Official)
This uses a different NuSpec file than the "manual" process below. This MSBuild process uses,

**Uses NuSpec:**
``.\source\ToolsHub.MSBuild.nuspec``

1. Open command prompt in root of project
2. ``MSBuild /t:MakeInstaller``
3. **Carefully copy** select output from "Releases" folder to our network share.

## MANUAL Distributing
Only perform the manual process if the official **MSBuild** one does not work.

To learn more about distributing, read the following [official document](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/getting-started/2-packaging.md).

1. **VS:** Update the version numbers in ApplicationInfo.cs
2. **VS:** Build from Release
3. **NuGet Package Explorer** (_or Notepad++_)
    1. Update .nuspec version number to match ApplicationInfo version.
        * **FILE:** ``.\source\ToolsHub.nuspec``
    2. Click, Save As.
    3. Place file in root of project folder.
4. Execute Releasify from VS inside of project
    1. VS: View > Other Windows > **Package Manager Console**
    2. Navigate to root of project folder and **releasify**
    3. Example:
```powershell
PM> cd ..
PM> Squirrel --releasify ToolsHub.1.1.53.nupkg
```
5. Copy "Releases" output to our network share for archiving


## Visual Studio Build Packaging
More information can be found on the [official page](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/using/visual-studio-packaging.md).
