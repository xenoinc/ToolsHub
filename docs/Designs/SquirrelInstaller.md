2019-04-10 - Damian Suess

Consider using Squirrel Installer

## Sample Squirrel Script
Add the following to build.proj file.

```xml

  <!-- Installer -->
  <Target Name="MakeInstaller" DependsOnTargets="CompileCode" Condition="'$(Configuration)' == 'Release'">
  	<Message Text="Creating installer..." />

    <GetAssemblyIdentity AssemblyFiles="$(MainExe)">
      <Output TaskParameter="Assemblies" ItemName="AppAssembly"/>
    </GetAssemblyIdentity>

    <ItemGroup>
      <!-- Using item group because it works with multiple-versions. Though it gets the lowest version -->
      <NuGetExe Include="source\packages\NuGet.CommandLine.*\tools\nuget.exe" />
      <SquirrelExe Include="source\packages\Squirrel.Windows.*\tools\squirrel.exe" />
    </ItemGroup>

    <PropertyGroup>
      <SemanticVersion>$([System.Version]::Parse(%(AppAssembly.Version)).ToString(3))</SemanticVersion>
      <NuSpecPath>$(OutputFolder)\$(ProjectName).$(SemanticVersion).nupkg</NuSpecPath>

      <NuGetCmd>"%(NuGetExe.FullPath)" pack "$(SourcePath)\$(ProjectName)\$(ProjectName).MSBuild.nuspec" -Version $(SemanticVersion) -Properties "Configuration=Release" -OutputDirectory "$(OutputFolder)" -BasePath "$(OutputFolder)"</NuGetCmd>
      <SquirrelCmd>"%(SquirrelExe.FullPath)" --releasify "$(OutputFolder)\$(ProjectName).$(SemanticVersion).nupkg" --releaseDir="$(SquirrelReleasePath)"</SquirrelCmd>
    </PropertyGroup>

    <Message Text="Assembly File (MainExe): '$(MainExe)'" />
    <Message Text="Assemblies:              '%(AppAssembly.Version)'" />
    <Message Text="Semantic Version:        '$(SemanticVersion)'" />
    <Message Text="Output:                  '$(NuSpecPath)'" />
    <Message Text="--------------" />
    <!--
    <Message Text="$(NuGetCmd)" />
    <Message Text="$(SquirrelCmd)" />
    -->
    <Error Condition="!Exists(%(NuGetExe.FullPath))" Text="You are trying to use the NuGet.CommandLine package, but it is not installed. Please install NuGet.CommandLine from the Package Manager." />
    <Error Condition="!Exists(%(SquirrelExe.FullPath))" Text="You are trying to use the Squirrel.Windows package, but it is not installed. Please install Squirrel.Windows from the Package Manager." />

    <Message Text="Creating Installer Package..." />
    <Exec Command="$(NuGetCmd)" />

    <Message Text="Registering with Auto-Updater..." />
    <Exec Command="$(SquirrelCmd)" />

    <Message Text="Done!" />
  </Target>
```

### Beta Builds
Untested script...
```xml
  <!--
  <Target Name="MakeInstallerBeta" DependsOnTargets="MakeInstaller">
    NOTE:
      * This requires conditions inside of "MakeInstaller" to allow "$(Conf)==ReleaseBeta"
      * Change Squirrel output directory to $(SquirrelReleasePathBeta).
    <Message Text="Creating Beta Release Package..." />
    <Configuration>ReleaseBeta</Configuration>
    <CallTarget Targets="MakeInstaller" />
  </Target>
  -->
```