; Language: Kazakh (1087)
; ESDInstaller project translation

!insertmacro LANGFILE "Kazakh" = "Қазақша" "Qazaqsha"

!ifdef MUI_WELCOMEPAGE
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TITLE "$(^NameDA) орнату шеберіне қош келдіңіз"
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TEXT "Бұл бағдарлама $(^NameDA) қолданбасын компьютеріңізге орнатады.$\r$\n$\r$\nОрнатуды бастамас бұрын барлық басқа қолданбаларды жабу ұсынылады. Бұл жүйелік файлдарды компьютерді қайта іске қоспай жаңартуға мүмкіндік береді.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_DIRECTORYPAGE
  ${LangFileString} MUI_TEXT_DIRECTORY_TITLE "Орнату қалтасын таңдау"
  ${LangFileString} MUI_TEXT_DIRECTORY_SUBTITLE "$(^NameDA) орнатылатын қалтаны таңдаңыз."
!endif

!ifdef MUI_INSTFILESPAGE
  ${LangFileString} MUI_TEXT_INSTALLING_TITLE "Орнатылуда"
  ${LangFileString} MUI_TEXT_INSTALLING_SUBTITLE "$(^NameDA) орнатылғанша күтіңіз."
  ${LangFileString} MUI_TEXT_FINISH_TITLE "Орнату аяқталды"
  ${LangFileString} MUI_TEXT_FINISH_SUBTITLE "Орнату сәтті аяқталды."
  ${LangFileString} MUI_TEXT_ABORT_TITLE "Орнату тоқтатылды"
  ${LangFileString} MUI_TEXT_ABORT_SUBTITLE "Орнату аяқталған жоқ."
!endif

!ifdef MUI_UNINSTFILESPAGE
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_TITLE "Жойылуда"
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_SUBTITLE "$(^NameDA) жойылғанша күтіңіз."
  ${LangFileString} MUI_UNTEXT_FINISH_TITLE "Жою аяқталды"
  ${LangFileString} MUI_UNTEXT_FINISH_SUBTITLE "Бағдарлама сәтті жойылды."
  ${LangFileString} MUI_UNTEXT_ABORT_TITLE "Жою тоқтатылды"
  ${LangFileString} MUI_UNTEXT_ABORT_SUBTITLE "Жою толық аяқталған жоқ."
!endif

!ifdef MUI_FINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_INFO_TITLE "$(^NameDA) орнату шебері аяқталды"
  ${LangFileString} MUI_TEXT_FINISH_INFO_TEXT "$(^NameDA) компьютеріңізге орнатылды.$\r$\n$\r$\nОрнату бағдарламасынан шығу үшін $\"Дайын$\" түймесін басыңыз."
  ${LangFileString} MUI_TEXT_FINISH_INFO_REBOOT "$(^NameDA) орнатуын аяқтау үшін компьютерді қайта іске қосу қажет. Қазір қайта іске қосу керек пе?"
!endif

!ifdef MUI_UNFINISHPAGE
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TITLE "$(^NameDA) жою шебері аяқталды"
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA) компьютеріңізден жойылды.$\r$\n$\r$\nЖою бағдарламасынан шығу үшін $\"Дайын$\" түймесін басыңыз."
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_REBOOT "$(^NameDA) жоюын аяқтау үшін компьютерді қайта іске қосу қажет. Қазір қайта іске қосу керек пе?"
!endif

!ifdef MUI_FINISHPAGE | MUI_UNFINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_REBOOTNOW "Иә, компьютерді қазір қайта іске қосу"
  ${LangFileString} MUI_TEXT_FINISH_REBOOTLATER "Жоқ, кейін қайта іске қосамын"
  ${LangFileString} MUI_TEXT_FINISH_RUN "$(^NameDA) &іске қосу"
  ${LangFileString} MUI_TEXT_FINISH_SHOWREADME "ReadMe файлын &көрсету"
  ${LangFileString} MUI_BUTTONTEXT_FINISH "&Дайын"
!endif

!ifdef MUI_UNCONFIRMPAGE
  ${LangFileString} MUI_UNTEXT_CONFIRM_TITLE "$(^NameDA) жою"
  ${LangFileString} MUI_UNTEXT_CONFIRM_SUBTITLE "$(^NameDA) қолданбасын компьютерден жою."
!endif

!ifdef MUI_ABORTWARNING
  ${LangFileString} MUI_TEXT_ABORTWARNING "$(^Name) орнату бағдарламасынан шығу керек пе?"
!endif

!ifdef MUI_UNABORTWARNING
  ${LangFileString} MUI_UNTEXT_ABORTWARNING "$(^Name) жою бағдарламасынан шығу керек пе?"
!endif
