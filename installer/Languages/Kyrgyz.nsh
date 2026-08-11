; Language: Kyrgyz (1088)
; ESD Installer project translation

!insertmacro LANGFILE "Kyrgyz" = "Кыргызча" "Kyrgyz"

!ifdef MUI_WELCOMEPAGE
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TITLE "$(^NameDA) Setup кош келиңиз"
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TEXT "Setup сизге $(^NameDA).$\r$\n$\r$\n орнотуу боюнча жетекчилик кылат, Setupти баштоодон мурун бардык башка колдонмолорду жабуу сунушталат. Бул компьютериңизди өчүрүп күйгүзбөстөн тиешелүү тутум файлдарын жаңыртууга мүмкүндүк берет.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_DIRECTORYPAGE
  ${LangFileString} MUI_TEXT_DIRECTORY_TITLE "Орнотуу ордун тандаңыз"
  ${LangFileString} MUI_TEXT_DIRECTORY_SUBTITLE "$(^NameDA) орнотула турган папканы тандаңыз."
!endif

!ifdef MUI_INSTFILESPAGE
  ${LangFileString} MUI_TEXT_INSTALLING_TITLE "Орнотулууда"
  ${LangFileString} MUI_TEXT_INSTALLING_SUBTITLE "Сураныч, $(^NameDA) орнотулуп жатканда күтө туруңуз."
  ${LangFileString} MUI_TEXT_FINISH_TITLE "Орнотуу аяктады"
  ${LangFileString} MUI_TEXT_FINISH_SUBTITLE "Setup ийгиликтүү аяктады."
  ${LangFileString} MUI_TEXT_ABORT_TITLE "Орнотуу токтотулду"
  ${LangFileString} MUI_TEXT_ABORT_SUBTITLE "Setup ийгиликтүү аяктаган жок."
!endif

!ifdef MUI_UNINSTFILESPAGE
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_TITLE "Орнотуудан чыгарылууда"
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_SUBTITLE "Сураныч, $(^NameDA) орнотулбай жатканда күтө туруңуз."
  ${LangFileString} MUI_UNTEXT_FINISH_TITLE "Орнотуу аяктады"
  ${LangFileString} MUI_UNTEXT_FINISH_SUBTITLE "Орнотуу ийгиликтүү аяктады."
  ${LangFileString} MUI_UNTEXT_ABORT_TITLE "Орнотуу токтотулду"
  ${LangFileString} MUI_UNTEXT_ABORT_SUBTITLE "Орнотуу ийгиликтүү аяктаган жок."
!endif

!ifdef MUI_FINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_INFO_TITLE "$(^NameDA) Setup аякталууда"
  ${LangFileString} MUI_TEXT_FINISH_INFO_TEXT "$(^NameDA) компьютериңизге орнотулду. $\r$\n$\r$\nSetup жабуу үчүн Аяктоо баскычын басыңыз."
  ${LangFileString} MUI_TEXT_FINISH_INFO_REBOOT "$(^NameDA) орнотууну аяктоо үчүн компьютериңиз кайра күйгүзүлүшү керек. Азыр өчүрүп күйгүзүүнү каалайсызбы?"
!endif

!ifdef MUI_UNFINISHPAGE
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TITLE "$(^NameDA) орнотуудан чыгаруу аякталууда"
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA) компьютериңизден чыгарылды. $\r$\n$\r$\nSetup жабуу үчүн $\"Бүтүрүү$\" баскычын басыңыз."
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_REBOOT "$(^NameDA) орнотуудан чыгарууну аяктоо үчүн компьютериңиз өчүрүлүп күйгүзүлүшү керек. Азыр өчүрүп күйгүзүүнү каалайсызбы?"
!endif

!ifdef MUI_FINISHPAGE | MUI_UNFINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_REBOOTNOW "Азыр өчүрүп күйгүзүңүз"
  ${LangFileString} MUI_TEXT_FINISH_REBOOTLATER "Мен кийинчерээк кол менен өчүрүп күйгүм келет"
  ${LangFileString} MUI_TEXT_FINISH_RUN "&$(^NameDA) иштетиңиз"
  ${LangFileString} MUI_TEXT_FINISH_SHOWREADME "&Окуума көрсөтүү"
  ${LangFileString} MUI_BUTTONTEXT_FINISH "&Бүтүрүү"
!endif

!ifdef MUI_UNCONFIRMPAGE
  ${LangFileString} MUI_UNTEXT_CONFIRM_TITLE "$(^NameDA) орнотуудан чыгарыңыз"
  ${LangFileString} MUI_UNTEXT_CONFIRM_SUBTITLE "$(^NameDA) компьютериңизден алып салыңыз."
!endif

!ifdef MUI_ABORTWARNING
  ${LangFileString} MUI_TEXT_ABORTWARNING "$(^Name) Setupден чын эле чыккыңыз келеби?"
!endif

!ifdef MUI_UNABORTWARNING
  ${LangFileString} MUI_UNTEXT_ABORTWARNING "$(^Name) Uninstall'тан чын эле чыккыңыз келеби?"
!endif
