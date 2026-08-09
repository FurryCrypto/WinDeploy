Unicode true

!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "x64.nsh"
!include "WinVer.nsh"

!ifndef APP_VERSION
  !define APP_VERSION "0.1.7"
!endif
!ifndef APP_SOURCE
  !error "APP_SOURCE must point to the self-contained WinDeploy publish directory."
!endif
!ifndef OUTPUT_FILE
  !define OUTPUT_FILE "WinDeploy-Setup-${APP_VERSION}.exe"
!endif
!ifndef APP_ICON
  !error "APP_ICON must point to WinDeploy.ico."
!endif
!ifndef APP_SIZE_KB
  !define APP_SIZE_KB 450000
!endif

!define APP_NAME "WinDeploy"
!define APP_PUBLISHER "WinDeploy Project"
!define APP_REG_KEY "Software\WinDeploy"
!define APP_UNINSTALL_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinDeploy"

Name "${APP_NAME} ${APP_VERSION}"
Caption "${APP_NAME} Setup"
OutFile "${OUTPUT_FILE}"
InstallDir "$PROGRAMFILES64\WinDeploy"
InstallDirRegKey HKLM "${APP_REG_KEY}" "InstallDir"
RequestExecutionLevel admin
SetCompressor /SOLID lzma
SetCompressorDictSize 64
CRCCheck on
ShowInstDetails show
ShowUninstDetails show
AutoCloseWindow false
BrandingText "WinDeploy"
Icon "${APP_ICON}"
UninstallIcon "${APP_ICON}"
VIProductVersion "0.1.7.0"
VIAddVersionKey /LANG=1033 "ProductName" "WinDeploy"
VIAddVersionKey /LANG=1033 "ProductVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=1033 "FileDescription" "WinDeploy Setup"
VIAddVersionKey /LANG=1033 "FileVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=1033 "CompanyName" "${APP_PUBLISHER}"
VIAddVersionKey /LANG=1033 "LegalCopyright" "Copyright (c) 2026 WinDeploy Project"

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

LangString UnsupportedWindows ${LANG_ENGLISH} "WinDeploy requires Windows 10 or Windows 11."
LangString UnsupportedWindows ${LANG_FRENCH} "WinDeploy nécessite Windows 10 ou Windows 11."
LangString UnsupportedWindows ${LANG_GERMAN} "WinDeploy erfordert Windows 10 oder Windows 11."
LangString UnsupportedWindows ${LANG_LUXEMBOURGISH} "WinDeploy erfuerdert Windows 10 oder Windows 11."
LangString UnsupportedWindows ${LANG_SERBIANLATIN} "WinDeploy zahteva Windows 10 ili Windows 11."
LangString UnsupportedWindows ${LANG_RUSSIAN} "Для WinDeploy требуется Windows 10 или Windows 11."
LangString UnsupportedWindows ${LANG_SIMPCHINESE} "WinDeploy 需要 Windows 10 或 Windows 11。"
LangString UnsupportedWindows ${LANG_SPANISH} "WinDeploy requiere Windows 10 o Windows 11."
LangString UnsupportedWindows ${LANG_POLISH} "WinDeploy wymaga systemu Windows 10 lub Windows 11."
LangString UnsupportedWindows ${LANG_GREEK} "Το WinDeploy απαιτεί Windows 10 ή Windows 11."
LangString UnsupportedWindows ${LANG_DANISH} "WinDeploy kræver Windows 10 eller Windows 11."

LangString UnsupportedArchitecture ${LANG_ENGLISH} "This WinDeploy build requires 64-bit Windows (x64)."
LangString UnsupportedArchitecture ${LANG_FRENCH} "Cette version de WinDeploy nécessite Windows 64 bits (x64)."
LangString UnsupportedArchitecture ${LANG_GERMAN} "Diese WinDeploy-Version erfordert 64-Bit-Windows (x64)."
LangString UnsupportedArchitecture ${LANG_LUXEMBOURGISH} "Dës WinDeploy-Versioun erfuerdert 64-Bit-Windows (x64)."
LangString UnsupportedArchitecture ${LANG_SERBIANLATIN} "Ova verzija programa WinDeploy zahteva 64-bitni Windows (x64)."
LangString UnsupportedArchitecture ${LANG_RUSSIAN} "Для этой версии WinDeploy требуется 64-разрядная Windows (x64)."
LangString UnsupportedArchitecture ${LANG_SIMPCHINESE} "此 WinDeploy 版本需要 64 位 Windows (x64)。"
LangString UnsupportedArchitecture ${LANG_SPANISH} "Esta versión de WinDeploy requiere Windows de 64 bits (x64)."
LangString UnsupportedArchitecture ${LANG_POLISH} "Ta wersja WinDeploy wymaga 64-bitowego systemu Windows (x64)."
LangString UnsupportedArchitecture ${LANG_GREEK} "Αυτή η έκδοση του WinDeploy απαιτεί Windows 64 bit (x64)."
LangString UnsupportedArchitecture ${LANG_DANISH} "Denne version af WinDeploy kræver 64-bit Windows (x64)."

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
  ${IfNot} ${AtLeastWin10}
    MessageBox MB_OK|MB_ICONSTOP "$(UnsupportedWindows)"
    Abort
  ${EndIf}
  ${IfNot} ${RunningX64}
    MessageBox MB_OK|MB_ICONSTOP "$(UnsupportedArchitecture)"
    Abort
  ${EndIf}
  SetRegView 64
  SetShellVarContext all
FunctionEnd

Section "WinDeploy" SEC_MAIN
  SectionIn RO
  SetRegView 64
  SetShellVarContext all
  SetOverwrite on
  SetOutPath "$INSTDIR"
  File /r /x "*.pdb" "${APP_SOURCE}\*.*"

  WriteUninstaller "$INSTDIR\Uninstall.exe"
  CreateDirectory "$SMPROGRAMS\WinDeploy"
  CreateShortcut "$SMPROGRAMS\WinDeploy\WinDeploy.lnk" "$INSTDIR\WinDeploy.exe" "" "$INSTDIR\WinDeploy.exe" 0
  CreateShortcut "$SMPROGRAMS\WinDeploy\Uninstall WinDeploy.lnk" "$INSTDIR\Uninstall.exe"

  WriteRegStr HKLM "${APP_REG_KEY}" "InstallDir" "$INSTDIR"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayName" "WinDeploy"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayVersion" "${APP_VERSION}"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "Publisher" "${APP_PUBLISHER}"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayIcon" "$INSTDIR\WinDeploy.exe"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "QuietUninstallString" "$\"$INSTDIR\Uninstall.exe$\" /S"
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "NoModify" 1
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "NoRepair" 1
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "EstimatedSize" ${APP_SIZE_KB}
SectionEnd

Section "Uninstall"
  SetRegView 64
  SetShellVarContext all
  Delete "$SMPROGRAMS\WinDeploy\WinDeploy.lnk"
  Delete "$SMPROGRAMS\WinDeploy\Uninstall WinDeploy.lnk"
  RMDir "$SMPROGRAMS\WinDeploy"
  DeleteRegKey HKLM "${APP_UNINSTALL_KEY}"
  DeleteRegKey HKLM "${APP_REG_KEY}"
  RMDir /r "$INSTDIR"
SectionEnd

Function un.onInit
  !insertmacro MUI_UNGETLANGUAGE
  SetRegView 64
  SetShellVarContext all
FunctionEnd
