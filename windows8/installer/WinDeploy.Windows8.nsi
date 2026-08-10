Unicode true
!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "x64.nsh"
!include "WinVer.nsh"

!ifndef APP_VERSION
  !define APP_VERSION "0.1.11"
!endif
!ifndef APP_SOURCE
  !error "APP_SOURCE must point to the Windows 8/8.1 package directory."
!endif
!ifndef OUTPUT_FILE
  !define OUTPUT_FILE "WinDeploy-Windows8-Setup-${APP_VERSION}.exe"
!endif
!ifndef APP_ICON
  !error "APP_ICON must point to WinDeploy.Windows8.ico."
!endif
!ifndef APP_SIZE_KB
  !define APP_SIZE_KB 30000
!endif

!define APP_NAME "WinDeploy for Windows 8 and 8.1"
!define APP_PUBLISHER "FurryCrypto"
!define APP_REG_KEY "Software\WinDeployWindows8"
!define APP_UNINSTALL_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinDeployWindows8"

Name "${APP_NAME} ${APP_VERSION}"
Caption "${APP_NAME} Setup"
OutFile "${OUTPUT_FILE}"
InstallDir "$PROGRAMFILES\WinDeploy Windows 8"
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
VIProductVersion "0.1.11.0"
VIAddVersionKey /LANG=1033 "ProductName" "${APP_NAME}"
VIAddVersionKey /LANG=1033 "ProductVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=1033 "FileVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=1033 "FileDescription" "WinDeploy Windows 8/8.1 Setup"
VIAddVersionKey /LANG=1033 "CompanyName" "${APP_PUBLISHER}"
VIAddVersionKey /LANG=1033 "LegalCopyright" "Copyright (c) 2026 FurryCrypto"

!define MUI_ABORTWARNING
!define MUI_ICON "${APP_ICON}"
!define MUI_UNICON "${APP_ICON}"
!define MUI_LANGDLL_REGISTRY_ROOT HKLM
!define MUI_LANGDLL_REGISTRY_KEY "${APP_REG_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "InstallerLanguage"
!define MUI_WELCOMEPAGE_TITLE_3LINES
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
; Long localized headings (notably Russian) need the supported three-line
; finish-page title area instead of being clipped at two lines.
!define MUI_FINISHPAGE_TITLE_3LINES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!define MUI_FINISHPAGE_TITLE_3LINES
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
!insertmacro MUI_LANGUAGE "Norwegian"
!insertmacro MUI_LANGUAGE "Finnish"
!insertmacro MUI_LANGUAGE "Swedish"
!insertmacro MUI_LANGUAGE "Mongolian"
!insertmacro MUI_LANGUAGE "Armenian"
!insertmacro MUI_LANGUAGE "Tatar"
!insertmacro MUI_LANGUAGEEX "${__FILEDIR__}\..\..\installer\Languages" "Kazakh"
!insertmacro MUI_LANGUAGEEX "${__FILEDIR__}\..\..\installer\Languages" "Bashkir"
!insertmacro MUI_LANGUAGEEX "${__FILEDIR__}\..\..\installer\Languages" "CrimeanTatar"
!insertmacro MUI_LANGUAGEEX "${__FILEDIR__}\..\..\installer\Languages" "Abkhazian"
!insertmacro MUI_LANGUAGEEX "${__FILEDIR__}\..\..\installer\Languages" "Ossetian"

LangString UnsupportedWindows ${LANG_ENGLISH} "This WinDeploy edition runs only on Windows 8 and Windows 8.1."
LangString UnsupportedWindows ${LANG_FRENCH} "Cette édition de WinDeploy fonctionne uniquement sous Windows 8 et Windows 8.1."
LangString UnsupportedWindows ${LANG_GERMAN} "Diese WinDeploy-Ausgabe läuft nur unter Windows 8 und Windows 8.1."
LangString UnsupportedWindows ${LANG_LUXEMBOURGISH} "Dës WinDeploy-Editioun leeft nëmmen op Windows 8 a Windows 8.1."
LangString UnsupportedWindows ${LANG_SERBIANLATIN} "Ovo izdanje programa WinDeploy radi samo na sistemima Windows 8 i Windows 8.1."
LangString UnsupportedWindows ${LANG_RUSSIAN} "Эта версия WinDeploy работает только в Windows 8 и Windows 8.1."
LangString UnsupportedWindows ${LANG_SIMPCHINESE} "此 WinDeploy 版本仅适用于 Windows 8 和 Windows 8.1。"
LangString UnsupportedWindows ${LANG_SPANISH} "Esta edición de WinDeploy solo funciona en Windows 8 y Windows 8.1."
LangString UnsupportedWindows ${LANG_POLISH} "Ta edycja WinDeploy działa tylko w systemach Windows 8 i Windows 8.1."
LangString UnsupportedWindows ${LANG_GREEK} "Αυτή η έκδοση του WinDeploy λειτουργεί μόνο σε Windows 8 και Windows 8.1."
LangString UnsupportedWindows ${LANG_DANISH} "Denne WinDeploy-udgave kører kun på Windows 8 og Windows 8.1."
LangString UnsupportedWindows ${LANG_NORWEGIAN} "Denne WinDeploy-utgaven kjører bare på Windows 8 og Windows 8.1."
LangString UnsupportedWindows ${LANG_FINNISH} "Tämä WinDeploy-versio toimii vain Windows 8:ssa ja Windows 8.1:ssä."
LangString UnsupportedWindows ${LANG_SWEDISH} "Den här WinDeploy-utgåvan körs endast på Windows 8 och Windows 8.1."
LangString UnsupportedWindows ${LANG_MONGOLIAN} "Энэ WinDeploy хувилбар зөвхөн Windows 8 болон Windows 8.1 дээр ажиллана."
LangString UnsupportedWindows ${LANG_ARMENIAN} "WinDeploy-ի այս տարբերակն աշխատում է միայն Windows 8-ում և Windows 8.1-ում։"
LangString UnsupportedWindows ${LANG_KAZAKH} "WinDeploy бағдарламасының бұл шығарылымы тек Windows 8 және Windows 8.1 жүйелерінде жұмыс істейді."
LangString UnsupportedWindows ${LANG_BASHKIR} "WinDeploy-ҙың был сығарылышы тик Windows 8 һәм Windows 8.1-ҙә эшләй."
LangString UnsupportedWindows ${LANG_TATAR} "WinDeploy-ның бу чыгарылышы Windows 8 һәм Windows 8.1-дә генә эшли."
LangString UnsupportedWindows ${LANG_CRIMEANTATAR} "WinDeploy-niñ bu sürümi tek Windows 8 ve Windows 8.1-de çalışa."
LangString UnsupportedWindows ${LANG_ABKHAZIAN} "WinDeploy ари аҭыжьымҭа Windows 8 ма Windows 8.1 мацара рҿы аус ауеит."
LangString UnsupportedWindows ${LANG_OSSETIAN} "Ацы WinDeploy-ы рауагъд æрмæст Windows 8 æмæ Windows 8.1-ы кусынц."
LangString DotNetRequired ${LANG_ENGLISH} "Microsoft .NET Framework 4.6.1 or later is required. Install it, restart Windows, and run Setup again."
LangString DotNetRequired ${LANG_FRENCH} "Microsoft .NET Framework 4.6.1 ou version ultérieure est requis. Installez-le, redémarrez Windows, puis relancez l’installation."
LangString DotNetRequired ${LANG_GERMAN} "Microsoft .NET Framework 4.6.1 oder neuer ist erforderlich. Installieren Sie es, starten Sie Windows neu und führen Sie Setup erneut aus."
LangString DotNetRequired ${LANG_LUXEMBOURGISH} "Microsoft .NET Framework 4.6.1 oder méi nei ass erfuerderlech. Installéiert et, start Windows nei a féiert de Setup nach eng Kéier aus."
LangString DotNetRequired ${LANG_SERBIANLATIN} "Potreban je Microsoft .NET Framework 4.6.1 ili noviji. Instalirajte ga, ponovo pokrenite Windows i zatim ponovo pokrenite instalaciju."
LangString DotNetRequired ${LANG_RUSSIAN} "Требуется Microsoft .NET Framework 4.6.1 или более поздней версии. Установите его, перезагрузите Windows и снова запустите установку."
LangString DotNetRequired ${LANG_SIMPCHINESE} "需要 Microsoft .NET Framework 4.6.1 或更高版本。请安装后重新启动 Windows，然后再次运行安装程序。"
LangString DotNetRequired ${LANG_SPANISH} "Se requiere Microsoft .NET Framework 4.6.1 o posterior. Instálelo, reinicie Windows y vuelva a ejecutar el instalador."
LangString DotNetRequired ${LANG_POLISH} "Wymagany jest Microsoft .NET Framework 4.6.1 lub nowszy. Zainstaluj go, uruchom ponownie Windows i ponownie uruchom instalator."
LangString DotNetRequired ${LANG_GREEK} "Απαιτείται Microsoft .NET Framework 4.6.1 ή νεότερο. Εγκαταστήστε το, επανεκκινήστε τα Windows και εκτελέστε ξανά την εγκατάσταση."
LangString DotNetRequired ${LANG_DANISH} "Microsoft .NET Framework 4.6.1 eller nyere er påkrævet. Installer det, genstart Windows, og kør installationen igen."
LangString DotNetRequired ${LANG_NORWEGIAN} "Microsoft .NET Framework 4.6.1 eller nyere kreves. Installer det, start Windows på nytt, og kjør installasjonsprogrammet igjen."
LangString DotNetRequired ${LANG_FINNISH} "Microsoft .NET Framework 4.6.1 tai uudempi vaaditaan. Asenna se, käynnistä Windows uudelleen ja suorita asennusohjelma uudelleen."
LangString DotNetRequired ${LANG_SWEDISH} "Microsoft .NET Framework 4.6.1 eller senare krävs. Installera det, starta om Windows och kör installationsprogrammet igen."
LangString DotNetRequired ${LANG_MONGOLIAN} "Microsoft .NET Framework 4.6.1 эсвэл түүнээс шинэ хувилбар шаардлагатай. Үүнийг суулгаж, Windows-ийг дахин эхлүүлээд суулгацыг дахин ажиллуулна уу."
LangString DotNetRequired ${LANG_ARMENIAN} "Պահանջվում է Microsoft .NET Framework 4.6.1 կամ ավելի նոր տարբերակ։ Տեղադրեք այն, վերագործարկեք Windows-ը և կրկին գործարկեք տեղադրիչը։"
LangString DotNetRequired ${LANG_KAZAKH} "Microsoft .NET Framework 4.6.1 немесе одан кейінгі нұсқасы қажет. Оны орнатып, Windows жүйесін қайта іске қосыңыз да, орнату бағдарламасын қайта іске қосыңыз."
LangString DotNetRequired ${LANG_BASHKIR} "Microsoft .NET Framework 4.6.1 йәки яңыраҡ версия кәрәк. Уны ҡуйығыҙ, Windows-ты яңынан эшләтеп ебәрегеҙ һәм урынлаштырыуҙы ҡабат эшләтегеҙ."
LangString DotNetRequired ${LANG_TATAR} "Microsoft .NET Framework 4.6.1 яки яңарак версия кирәк. Аны урнаштырыгыз, Windows-ны яңадан эшләтеп җибәрегез һәм урнаштыруны кабат эшләтегез."
LangString DotNetRequired ${LANG_CRIMEANTATAR} "Microsoft .NET Framework 4.6.1 ya da daa yañı sürüm kerek. Onı quruñız, Windows-nı kene başlatıñız ve qurucını kene çalıştırıñız."
LangString DotNetRequired ${LANG_ABKHAZIAN} "Microsoft .NET Framework 4.6.1 ма уи ишьҭанеиуа аверсиа аҭахуп. Ишьақәыргыланы, Windows еиҭаҿашәкны, ашьақәыргылара еиҭаҿашәкы."
LangString DotNetRequired ${LANG_OSSETIAN} "Microsoft .NET Framework 4.6.1 кæнæ фæстæдæр верси хъæуы. Йæ сæвæр, Windows ногæй райсæр æмæ сæвæрд ногæй райсæр."

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
  ${IfNot} ${AtLeastWin8}
    MessageBox MB_OK|MB_ICONSTOP "$(UnsupportedWindows)"
    Abort
  ${EndIf}
  ${IfNot} ${AtMostWin8.1}
    MessageBox MB_OK|MB_ICONSTOP "$(UnsupportedWindows)"
    Abort
  ${EndIf}
  ${If} ${RunningX64}
    SetRegView 64
    StrCpy $INSTDIR "$PROGRAMFILES64\WinDeploy Windows 8"
  ${Else}
    SetRegView 32
  ${EndIf}
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
  IntCmp $0 394254 dotnet_ok dotnet_missing dotnet_ok
  dotnet_missing:
    MessageBox MB_OK|MB_ICONSTOP "$(DotNetRequired)"
    Abort
  dotnet_ok:
  SetShellVarContext all
FunctionEnd

Section "WinDeploy for Windows 8 and 8.1" SEC_MAIN
  SectionIn RO
  SetShellVarContext all
  SetOverwrite on
  SetOutPath "$INSTDIR"
  File /r /x "*.pdb" /x "Redist" "${APP_SOURCE}\*.*"
  ; wimlib uses the Universal CRT. Windows 8/8.1 may not have KB2999226,
  ; so deploy Microsoft's down-level UCRT app-locally for the host architecture.
  ${If} ${RunningX64}
    SetOutPath "$INSTDIR"
    File "${APP_SOURCE}\Redist\UCRT\x64\*.dll"
    SetOutPath "$INSTDIR\Worker"
    File "${APP_SOURCE}\Redist\UCRT\x64\*.dll"
  ${Else}
    SetOutPath "$INSTDIR"
    File "${APP_SOURCE}\Redist\UCRT\x86\*.dll"
    SetOutPath "$INSTDIR\Worker"
    File "${APP_SOURCE}\Redist\UCRT\x86\*.dll"
  ${EndIf}
  SetOutPath "$INSTDIR"
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  CreateDirectory "$SMPROGRAMS\WinDeploy"
  CreateShortcut "$SMPROGRAMS\WinDeploy\WinDeploy for Windows 8 and 8.1.lnk" "$INSTDIR\WinDeploy.Windows8.exe" "" "$INSTDIR\WinDeploy.Windows8.exe" 0
  CreateShortcut "$DESKTOP\WinDeploy for Windows 8 and 8.1.lnk" "$INSTDIR\WinDeploy.Windows8.exe" "" "$INSTDIR\WinDeploy.Windows8.exe" 0
  WriteRegStr HKLM "${APP_REG_KEY}" "InstallDir" "$INSTDIR"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayName" "${APP_NAME}"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayVersion" "${APP_VERSION}"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "Publisher" "${APP_PUBLISHER}"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "DisplayIcon" "$INSTDIR\WinDeploy.Windows8.exe"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr HKLM "${APP_UNINSTALL_KEY}" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "NoModify" 1
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "NoRepair" 1
  WriteRegDWORD HKLM "${APP_UNINSTALL_KEY}" "EstimatedSize" ${APP_SIZE_KB}
SectionEnd

Section "Uninstall"
  SetShellVarContext all
  Delete "$SMPROGRAMS\WinDeploy\WinDeploy for Windows 8 and 8.1.lnk"
  RMDir "$SMPROGRAMS\WinDeploy"
  Delete "$DESKTOP\WinDeploy for Windows 8 and 8.1.lnk"
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
