; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Lerp2API Docs"
#define MyAppVersion "1.0"
#define MyAppPublisher "Lerp2Dev"
#define MyAppURL "http://www.lerp2dev.com/"
#define MyAppExeName "Lerp2API-Docs-Final.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppID={{C501D7DE-5730-4B2D-A9B2-5AF06256199D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={%appdata}\Lerp2Dev\{#MyAppName}
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir=..\DOCS-FINAL
OutputBaseFilename=Lerp2API-Docs.Setup
Compression=lzma/Max
SolidCompression=true
WizardImageFile=Images\WizModernImage.bmp
WizardSmallImageFile=Images\WIZMODERNSMALLIMAGE.bmp

[Languages]
Name: english; MessagesFile: compiler:Default.isl
Name: spanish; MessagesFile: compiler:Languages\Spanish.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: ..\Lerp2API-Docs-Final.exe; DestDir: {app}; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: {commonprograms}\{#MyAppName}; Filename: {app}\{#MyAppExeName}
Name: {commondesktop}\{#MyAppName}; Filename: {app}\{#MyAppExeName}; Tasks: desktopicon

[Run]
Filename: {app}\{#MyAppExeName}; Description: {cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}; Flags: nowait postinstall skipifsilent
