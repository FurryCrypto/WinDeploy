; Language: UyghurCyrillic (32767)
; ESD Installer project translation

!insertmacro LANGFILE "UyghurCyrillic" = "Уйғурчә (кириллица)" "UyghurCyrillic"

!ifdef MUI_WELCOMEPAGE
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TITLE "$(^NameDA) Setup ға хуш кәпсиз"
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TEXT "Setup сизни $(^NameDA).$\r$\n$\r$\nI қачилаш арқилиқ йетәкләйду , Setup ни башлаштин бурун башқа барлиқ программиларни тақаш тәвсийә қилиниду. бундақ болғанда компйутериңизни қайта қозғимай туруп мунасивәтлик система һөҗҗәтлирини йеңилайду. $\r$\nZXPPXXZ$_CLICK"
!endif

!ifdef MUI_DIRECTORYPAGE
  ${LangFileString} MUI_TEXT_DIRECTORY_TITLE "орни орни ни таллаң"
  ${LangFileString} MUI_TEXT_DIRECTORY_SUBTITLE "$(^NameDA) орнитидиған һөҗҗәт қисқучни таллаң."
!endif

!ifdef MUI_INSTFILESPAGE
  ${LangFileString} MUI_TEXT_INSTALLING_TITLE "қачилаш"
  ${LangFileString} MUI_TEXT_INSTALLING_SUBTITLE "$(^NameDA) орнитиливатқанда сақлаң."
  ${LangFileString} MUI_TEXT_FINISH_TITLE "қачилаш тамам"
  ${LangFileString} MUI_TEXT_FINISH_SUBTITLE "Setup мувәппәқийәтлик тамамланди."
  ${LangFileString} MUI_TEXT_ABORT_TITLE "қачилаш әмәлдин қалдурулди"
  ${LangFileString} MUI_TEXT_ABORT_SUBTITLE "Setup мувәппәқийәтлик тамамланмиди."
!endif

!ifdef MUI_UNINSTFILESPAGE
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_TITLE "өчүрүветиш"
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_SUBTITLE "$(^NameDA) қачилиниватқанда сақлаң."
  ${LangFileString} MUI_UNTEXT_FINISH_TITLE "өчүрүш тамамланди"
  ${LangFileString} MUI_UNTEXT_FINISH_SUBTITLE "өчүрүш мувәппәқийәтлик тамамланди."
  ${LangFileString} MUI_UNTEXT_ABORT_TITLE "өчүрүветилди"
  ${LangFileString} MUI_UNTEXT_ABORT_SUBTITLE "өчүрүш мувәппәқийәтлик тамамланмиди."
!endif

!ifdef MUI_FINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_INFO_TITLE "$(^NameDA) Setup ни тамамлаш"
  ${LangFileString} MUI_TEXT_FINISH_INFO_TEXT "компйутериңизға $(^NameDA) орнитилди. $\r$\n$\r$\nClick Finish Setup ни тақаш."
  ${LangFileString} MUI_TEXT_FINISH_INFO_REBOOT "$(^NameDA) ни қачилашни тамамлаш үчүн компйутериңизни қайта қозғитиш керәк. һазир қайта қозғитишни халамсиз?"
!endif

!ifdef MUI_UNFINISHPAGE
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TITLE "$(^NameDA) ни өчүрүветиш"
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA) компйутериңиздин өчүрүветилди. $\r$\n$\r$\nClick Finish Setup ни тақаш."
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_REBOOT "$(^NameDA) ни өчүрүветиш үчүн компйутериңизни қайта қозғитиш керәк. һазир қайта қозғитишни халамсиз?"
!endif

!ifdef MUI_FINISHPAGE | MUI_UNFINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_REBOOTNOW "қайта қозғитиң"
  ${LangFileString} MUI_TEXT_FINISH_REBOOTLATER "кейин қолда қайта қозғитишни халаймән"
  ${LangFileString} MUI_TEXT_FINISH_RUN "& $(^NameDA) ни иҗра қилиң"
  ${LangFileString} MUI_TEXT_FINISH_SHOWREADME "& Readme ни көрсәт"
  ${LangFileString} MUI_BUTTONTEXT_FINISH "& тамам"
!endif

!ifdef MUI_UNCONFIRMPAGE
  ${LangFileString} MUI_UNTEXT_CONFIRM_TITLE "$(^NameDA) ни өчүрүң"
  ${LangFileString} MUI_UNTEXT_CONFIRM_SUBTITLE "$(^NameDA) ни компйутериңиздин өчүрүң."
!endif

!ifdef MUI_ABORTWARNING
  ${LangFileString} MUI_TEXT_ABORTWARNING "$(^Name) Setup дин ваз кечишни халамсиз?"
!endif

!ifdef MUI_UNABORTWARNING
  ${LangFileString} MUI_UNTEXT_ABORTWARNING "$(^Name) өчүрүветишни халамсиз?"
!endif
