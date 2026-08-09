Unicode true
!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "x64.nsh"
!include "WinVer.nsh"

!ifndef APP_VERSION
  !define APP_VERSION "0.1.7"
!endif
!ifndef APP_SOURCE
  !error "APP_SOURCE must point to the Windows 7 package directory."
!endif
!ifndef OUTPUT_FILE
  !define OUTPUT_FILE "WinDeploy-Windows7-Setup-${APP_VERSION}.exe"
!endif
!ifndef APP_ICON
  !error "APP_ICON must point to WinDeploy.Windows7.ico."
!endif
!ifndef APP_SIZE_KB
  !define APP_SIZE_KB 30000
!endif

!define APP_NAME "WinDeploy for Windows 7"
!define APP_PUBLISHER "FurryCrypto"
!define APP_REG_KEY "Software\WinDeployWindows7"
!define APP_UNINSTALL_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinDeployWindows7"

Name "${APP_NAME} ${APP_VERSION}"
Caption "${APP_NAME} Setup"
OutFile "${OUTPUT_FILE}"
InstallDir "$PROGRAMFILES\WinDeploy Windows 7"
InstallDirRegKey HKLM "${APP_REG_KEY}" "InstallDir"
RequestExecutionLevel admin
SetCompressor /SOLID lzma
SetCompressorDictSize 32
CRCCheck on
ShowInstDetails show
ShowUninstDetails show
BrandingText "WinDeploy"
Icon "${APP_ICON}"
UninstallIcon "${APP_ICON}"
VIProductVersion "0.1.7.0"
VIAddVersionKey /LANG=1033 "ProductName" "${APP_NAME}"
VIAddVersionKey /LANG=1033 "ProductVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=1033 "FileVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=1033 "FileDescription" "WinDeploy Windows 7 Setup"
VIAddVersionKey /LANG=1033 "CompanyName" "${APP_PUBLISHER}"
VIAddVersionKey /LANG=1033 "LegalCopyright" "Copyright (c) 2026 FurryCrypto"

!define MUI_ABORTWARNING
!define MUI_ICON "${APP_ICON}"
!define MUI_UNICON "${APP_ICON}"
!define MUI_LANGDLL_REGISTRY_ROOT HKLM
!define MUI_LANGDLL_REGISTRY_KEY "${APP_REG_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "InstallerLanguage"
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "French"
!insertmacro MUI_LANGUAGE "German"
!insertmacro MUI_LANGUAGE "Luxembourgish"
!insertmacro MUI_LANGUAGE "SerbianLatin"
!insertmacro MUI_LANGUAGE "Russian"
!insertmacro MUI_LANGUAGE "SimpChinese"
!insertmacro MUI_LANGUAGE "Spanish"
!insertmacro MUI_LANGUAGE "Polish"
!insertmacro MUI_LANGUAGE "Greek"
!insertmacro MUI_LANGUAGE "Danish"

LangString UnsupportedWindows ${LANG_ENGLISH} "WinDeploy for Windows 7 requires Windows 7 SP1 or later."
LangString UnsupportedWindows ${LANG_FRENCH} "WinDeploy pour Windows 7 nécessite Windows 7 SP1 ou une version ultérieure."
LangString UnsupportedWindows ${LANG_GERMAN} "WinDeploy für Windows 7 erfordert Windows 7 SP1 oder neuer."
LangString UnsupportedWindows ${LANG_LUXEMBOURGISH} "WinDeploy fir Windows 7 erfuerdert Windows 7 SP1 oder méi nei."
LangString UnsupportedWindows ${LANG_SERBIANLATIN} "WinDeploy za Windows 7 zahteva Windows 7 SP1 ili noviji."
LangString UnsupportedWindows ${LANG_RUSSIAN} "WinDeploy для Windows 7 требует Windows 7 SP1 или более новую версию."
LangString UnsupportedWindows ${LANG_SIMPCHINESE} "WinDeploy Windows 7 版需要 Windows 7 SP1 或更高版本。"
LangString UnsupportedWindows ${LANG_SPANISH} "WinDeploy para Windows 7 requiere Windows 7 SP1 o posterior."
LangString UnsupportedWindows ${LANG_POLISH} "WinDeploy dla Windows 7 wymaga Windows 7 SP1 lub nowszego."
LangString UnsupportedWindows ${LANG_GREEK} "Το WinDeploy για Windows 7 απαιτεί Windows 7 SP1 ή νεότερη έκδοση."
LangString UnsupportedWindows ${LANG_DANISH} "WinDeploy til Windows 7 kræver Windows 7 SP1 eller nyere."
LangString DotNetRequired ${LANG_ENGLISH} "Microsoft .NET Framework 4.8 is required. Install it, restart Windows, and run Setup again."
LangString DotNetRequired ${LANG_FRENCH} "Microsoft .NET Framework 4.8 est requis. Installez-le, redémarrez Windows, puis relancez l’installation."
LangString DotNetRequired ${LANG_GERMAN} "Microsoft .NET Framework 4.8 ist erforderlich. Installieren Sie es, starten Sie Windows neu und führen Sie Setup erneut aus."
LangString DotNetRequired ${LANG_LUXEMBOURGISH} "Microsoft .NET Framework 4.8 ass erfuerderlech. Installéiert et, start Windows nei a féiert de Setup nach eng Kéier aus."
LangString DotNetRequired ${LANG_SERBIANLATIN} "Potreban je Microsoft .NET Framework 4.8. Instalirajte ga, ponovo pokrenite Windows i zatim ponovo pokrenite instalaciju."
LangString DotNetRequired ${LANG_RUSSIAN} "Требуется Microsoft .NET Framework 4.8. Установите его, перезагрузите Windows и снова запустите установку."
LangString DotNetRequired ${LANG_SIMPCHINESE} "需要 Microsoft .NET Framework 4.8。请安装后重新启动 Windows，然后再次运行安装程序。"
LangString DotNetRequired ${LANG_SPANISH} "Se requiere Microsoft .NET Framework 4.8. Instálelo, reinicie Windows y vuelva a ejecutar el instalador."
LangString DotNetRequired ${LANG_POLISH} "Wymagany jest Microsoft .NET Framework 4.8. Zainstaluj go, uruchom ponownie Windows i ponownie uruchom instalator."
LangString DotNetRequired ${LANG_GREEK} "Απαιτείται το Microsoft .NET Framework 4.8. Εγκαταστήστε το, επανεκκινήστε τα Windows και εκτελέστε ξανά την εγκατάσταση."
LangString DotNetRequired ${LANG_DANISH} "Microsoft .NET Framework 4.8 er påkrævet. Installer det, genstart Windows, og kør installationen igen."

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
  ${IfNot} ${AtLeastWin7}
    MessageBox MB_OK|MB_ICONSTOP "$(UnsupportedWindows)"
    Abort
  ${EndIf}
  ${If} ${RunningX64}
    SetRegView 64
    StrCpy $INSTDIR "$PROGRAMFILES64\WinDeploy Windows 7"
  ${Else}
    SetRegView 32
  ${EndIf}
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
  IntCmp $0 528040 dotnet_ok dotnet_missing dotnet_ok
  dotnet_missing:
    MessageBox MB_OK|MB_ICONSTOP "$(DotNetRequired)"
    Abort
  dotnet_ok:
  SetShellVarContext all
FunctionEnd

Section "WinDeploy for Windows 7" SEC_MAIN
  SectionIn RO
  SetShellVarContext all
  SetOverwrite on
  SetOutPath "$INSTDIR"
  File /r /x "*.pdb" "${APP_SOURCE}\*.*"
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  CreateDirectory "$SMPROGRAMS\WinDeploy"
  CreateShortcut "$SMPROGRAMS\WinDeploy\WinDeploy for Windows 7.lnk" "$INSTDIR\WinDeploy.Windows7.exe" "" "$INSTDIR\WinDeploy.Windows7.exe" 0
  CreateShortcut "$DESKTOP\WinDeploy for Windows 7.lnk" "$INSTDIR\WinDeploy.Windows7.exe" "" "$INSTDIR\WinDeploy.Windows7.exe" 0
  WriteRegStr HKLM "${APP_REG_KEY}" "InstallDir" "$INSTDIR"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayName" "${APP_NAME}"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayVersion" "${APP_VERSION}"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "Publisher" "${APP_PUBLISHER}"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayIcon" "$INSTDIR\WinDeploy.Windows7.exe"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "NoModify" 1
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "NoRepair" 1
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "EstimatedSize" ${APP_SIZE_KB}
SectionEnd

Section "Uninstall"
  SetShellVarContext all
  Delete "$SMPROGRAMS\WinDeploy\WinDeploy for Windows 7.lnk"
  RMDir "$SMPROGRAMS\WinDeploy"
  Delete "$DESKTOP\WinDeploy for Windows 7.lnk"
  DeleteRegKey HKLM "${APP_UNINSTALL_KEY}"
  DeleteRegKey HKLM "${APP_REG_KEY}"
  RMDir /r "$INSTDIR"
SectionEnd

Function un.onInit
  !insertmacro MUI_UNGETLANGUAGE
  ${If} ${RunningX64}
    SetRegView 64
  ${Else}
    SetRegView 32
  ${EndIf}
  SetShellVarContext all
FunctionEnd
