; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
AppName=MonoLaunch
AppVerName=MonoLaunch 1.0.0.2
AppPublisher=Mono
AppPublisherURL=http://www.mono-project.com
AppSupportURL=http://www.mono-project.com
AppUpdatesURL=http://www.mono-project.com
DefaultDirName={pf}\MonoLaunch
DefaultGroupName=MonoLaunch
Compression=lzma
SolidCompression=true
AppVersion=1.0.0.2
AppID={{9AD75E15-64A1-4880-9601-7A94A9CA7C46}
UninstallDisplayIcon={app}\MonoCtrl2.exe
UninstallDisplayName=Mono Launcher
OutputBaseFilename=MonoLaunch-1.0.0.2-1
VersionInfoVersion=1.0.0.2
VersionInfoCompany=Mono
VersionInfoDescription=Runtime selector and Mono application launcher
VersionInfoTextVersion=Mono Launch 1.0.0.2

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked
;Name: quicklaunchicon; Description: {cm:CreateQuickLaunchIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: bin\MonoCtrl2.exe; DestDir: {app}
Source: bin\monoLaunchC.exe; DestDir: {sys}
Source: bin\monoLaunchW.exe; DestDir: {sys}
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[INI]
Filename: {app}\monoLaunch.url; Section: InternetShortcut; Key: URL; String: http://www.mono-project.com

[Icons]
Name: {group}\MonoLaunch; Filename: {app}\MonoCtrl2.exe
Name: {group}\{cm:ProgramOnTheWeb,MonoLaunch}; Filename: {app}\monoLaunch.url
Name: {group}\{cm:UninstallProgram,MonoLaunch}; Filename: {uninstallexe}
Name: {userdesktop}\MonoLaunchW; Filename: {sys}\monoLaunchW.exe; Tasks: desktopicon
Name: {userdesktop}\MonoLaunchC; Filename: {sys}\monoLaunchC.exe; Tasks: desktopicon
;Name: {userappdata}\Microsoft\Internet Explorer\Quick Launch\MonoLaunchC; Filename: {sys}\monoLaunchC.exe; Tasks: quicklaunchicon
;Name: {userappdata}\Microsoft\Internet Explorer\Quick Launch\MonoLaunchW; Filename: {sys}\monoLaunchW.exe; Tasks: quicklaunchicon

[Run]
Filename: {app}\MonoCtrl2.exe; Description: {cm:LaunchProgram,MonoLaunch}; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: files; Name: {app}\monoLaunch.url
