;Language: Bashkortsa (1133)
;By Joost Verburg

!insertmacro LANGFILE "Bashkir" = "Башҡортса" "Bashkortsa"

!ifdef MUI_WELCOMEPAGE
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TITLE "Һеҙҙе сәләмләйбеҙ .$(^NameDA)Уйынтыҡ"
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TEXT "Ойоштороу һеҙгә$(^NameDA).$\r$\n$\r$\nБыл система файлдарын яңыртыу мөмкин буласаҡ, һеҙҙең компьютерҙы яңынан башлау кәрәкмәйенсә.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_UNWELCOMEPAGE
  ${LangFileString} MUI_UNTEXT_WELCOME_INFO_TITLE "Һеҙҙе сәләмләйбеҙ .$(^NameDA)Урынһыҙ ҡуйыу"
  ${LangFileString} MUI_UNTEXT_WELCOME_INFO_TEXT "Ойоштороу һеҙҙең өсөн$(^NameDA).$\r$\n$\r$\nИнсталляцияны ташлауҙан алда, тикшерегеҙ$(^NameDA)йүгереп йөрөмәй.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_LICENSEPAGE
  ${LangFileString} MUI_TEXT_LICENSE_TITLE "Лицензия килешеүе"
  ${LangFileString} MUI_TEXT_LICENSE_SUBTITLE "Инсталляция алдынан лицензия шарттарын тикшерегеҙ.$(^NameDA)."
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM "Әгәр һеҙ килешеү шарттарын ҡабул итәһегеҙ икән, дауам итеүгә ризамын тип клик итегеҙ.$(^NameDA)."
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM_CHECKBOX "Әгәр һеҙ килешеү шарттарын ҡабул итәһегеҙ икән, түбәндәге билдәләр буйынса клик итегеҙ.$(^NameDA).$_CLICK"
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM_RADIOBUTTONS "Әгәр һеҙ килешеү шарттарын ҡабул итәһегеҙ икән, түбәндәге беренсе вариантты һайлағыҙ.$(^NameDA).$_CLICK"
!endif

!ifdef MUI_UNLICENSEPAGE
  ${LangFileString} MUI_UNTEXT_LICENSE_TITLE "Лицензия килешеүе"
  ${LangFileString} MUI_UNTEXT_LICENSE_SUBTITLE "Инсталляцияны бөтөргәнсе лицензия шарттарын тикшерегеҙ.$(^NameDA)."
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM "Әгәр һеҙ килешеү шарттарын ҡабул итәһегеҙ икән, дауам итергә ризамын тип клик итегеҙ.$(^NameDA)."
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM_CHECKBOX "Әгәр һеҙ килешеү шарттарын ҡабул итәһегеҙ икән, түбәндәге билдәләмәне баҫығыҙ.$(^NameDA).$_CLICK"
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM_RADIOBUTTONS "Әгәр һеҙ килешеү шарттарын ҡабул итәһегеҙ икән, түбәндәге беренсе вариантты һайлағыҙ.$(^NameDA).$_CLICK"
!endif

!ifdef MUI_LICENSEPAGE | MUI_UNLICENSEPAGE
  ${LangFileString} MUI_INNERTEXT_LICENSE_TOP "Килешеүҙең ҡалған өлөшөн ҡарарға өсөн, түбәндәге биткә баҫығыҙ."
!endif

!ifdef MUI_COMPONENTSPAGE
  ${LangFileString} MUI_TEXT_COMPONENTS_TITLE "Компоненттар һайлағыҙ"
  ${LangFileString} MUI_TEXT_COMPONENTS_SUBTITLE "$(^NameDA)һеҙ уны урынлаштырырға теләйһегеҙ."
!endif

!ifdef MUI_UNCOMPONENTSPAGE
  ${LangFileString} MUI_UNTEXT_COMPONENTS_TITLE "Компоненттар һайлағыҙ"
  ${LangFileString} MUI_UNTEXT_COMPONENTS_SUBTITLE "$(^NameDA)һеҙ уны ҡуҙғатырға теләйһегеҙ."
!endif

!ifdef MUI_COMPONENTSPAGE | MUI_UNCOMPONENTSPAGE
  ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_TITLE "Тасуирлау"
  !ifndef NSIS_CONFIG_COMPONENTPAGE_ALTERNATIVE
    ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_INFO "Мустың һүрәтләнешен күрер өсөн, уны компонент өҫтөнә ҡуйығыҙ."
  !else
    ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_INFO "Уның һүрәтләнешен күрергә компонентты һайлағыҙ."
  !endif
!endif

!ifdef MUI_DIRECTORYPAGE
  ${LangFileString} MUI_TEXT_DIRECTORY_TITLE "Урынды ҡуйыу"
  ${LangFileString} MUI_TEXT_DIRECTORY_SUBTITLE "Инсталляция өсөн папканы һайлағыҙ$(^NameDA)."
!endif

!ifdef MUI_UNDIRECTORYPAGE
  ${LangFileString} MUI_UNTEXT_DIRECTORY_TITLE "Урынды ҡулынан ысҡындырыу"
  ${LangFileString} MUI_UNTEXT_DIRECTORY_SUBTITLE "Инстальциянан баш тартыу өсөн папканы һайлағыҙ$(^NameDA)."
!endif

!ifdef MUI_INSTFILESPAGE
  ${LangFileString} MUI_TEXT_INSTALLING_TITLE "Урынлаштырыу"
  ${LangFileString} MUI_TEXT_INSTALLING_SUBTITLE "Көтөгөҙ , зинһар .$(^NameDA)ҡуйыла."
  ${LangFileString} MUI_TEXT_FINISH_TITLE "Установка тамамланған"
  ${LangFileString} MUI_TEXT_FINISH_SUBTITLE "Уйын уңышлы тамамланды."
  ${LangFileString} MUI_TEXT_ABORT_TITLE "Урынлаштырыу туҡтатылды"
  ${LangFileString} MUI_TEXT_ABORT_SUBTITLE "Уйын уңышлы тамамланмаған."
!endif

!ifdef MUI_UNINSTFILESPAGE
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_TITLE "Урынһыҙ ҡуйыу"
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_SUBTITLE "Көтөгөҙ , зинһар .$(^NameDA)уны ҡуҙғатып торалар."
  ${LangFileString} MUI_UNTEXT_FINISH_TITLE "Урынһыҙ ҡуйыу тамамланған"
  ${LangFileString} MUI_UNTEXT_FINISH_SUBTITLE "Уңайһыҙлау уңышлы тамамланды."
  ${LangFileString} MUI_UNTEXT_ABORT_TITLE "Урынһыҙ ҡуйыу туҡтатылды"
  ${LangFileString} MUI_UNTEXT_ABORT_SUBTITLE "Установканы бөтөрөү уңышлы тамамланмаған."
!endif

!ifdef MUI_FINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_INFO_TITLE "Төҙөлөш тамамлана$(^NameDA)Уйынтыҡ"
  ${LangFileString} MUI_TEXT_FINISH_INFO_TEXT "$(^NameDA)һеҙҙең компьютерҙа ҡуйылған.$\r$\n$\r$\nҠуйылыуҙы ябыу өсөн тамамлауды баҫығыҙ."
  ${LangFileString} MUI_TEXT_FINISH_INFO_REBOOT "Компьютерҙы яңынан башларға кәрәк , сөнки уны ҡуйыу тамамлана .$(^NameDA)Хәҙер яңынан эшләй башларға теләйһеңме?"
!endif

!ifdef MUI_UNFINISHPAGE
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TITLE "Төҙөлөш тамамлана$(^NameDA)Урынһыҙ ҡуйыу"
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA)компьютерҙан ҡуҙғатылды.$\r$\n$\r$\nҠуйылыуҙы ябыу өсөн тамамлауды баҫығыҙ."
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_REBOOT "Компьютерҙы яңынан башларға кәрәк , сөнки уны монтажлау тамамлана .$(^NameDA)Хәҙер яңынан эшләй башларға теләйһеңме?"
!endif

!ifdef MUI_FINISHPAGE | MUI_UNFINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_REBOOTNOW "Хәҙер яңынан эшләй башлағыҙ"
  ${LangFileString} MUI_TEXT_FINISH_REBOOTLATER "Мин ҡулдан һуң яңынан эшләй башларға теләйем"
  ${LangFileString} MUI_TEXT_FINISH_RUN "& Run$(^NameDA)"
  ${LangFileString} MUI_TEXT_FINISH_SHOWREADME "&Show Readme"
  ${LangFileString} MUI_BUTTONTEXT_FINISH "& Finish"
!endif

!ifdef MUI_STARTMENUPAGE
  ${LangFileString} MUI_TEXT_STARTMENU_TITLE "Менюның башлау папкаһын һайлағыҙ"
  ${LangFileString} MUI_TEXT_STARTMENU_SUBTITLE "өсөн Start Menu папкаһын һайлағыҙ$(^NameDA)ҡыҫҡа юлдар."
  ${LangFileString} MUI_INNERTEXT_STARTMENU_TOP "Программаның ҡыҫҡа юлдар төҙөү өсөн башлау менюһы папкаһын һайлағыҙ. Яңы папка булдырыу өсөн исем дә индерегеҙ."
  ${LangFileString} MUI_INNERTEXT_STARTMENU_CHECKBOX "Кисе юлдар яһама"
!endif

!ifdef MUI_UNCONFIRMPAGE
  ${LangFileString} MUI_UNTEXT_CONFIRM_TITLE "Инстальциянан ысҡындырыу$(^NameDA)"
  ${LangFileString} MUI_UNTEXT_CONFIRM_SUBTITLE "Алып сығыу$(^NameDA)компьютерҙан."
!endif

!ifdef MUI_ABORTWARNING
  ${LangFileString} MUI_TEXT_ABORTWARNING "Һин , ысынлап та , китергә теләйһеңме ?$(^Name)Нимә тип уйлайһығыҙ?"
!endif

!ifdef MUI_UNABORTWARNING
  ${LangFileString} MUI_UNTEXT_ABORTWARNING "Һин , ысынлап та , китергә теләйһеңме ?$(^Name)Уны ташларғамы?"
!endif

!ifdef MULTIUSER_INSTALLMODEPAGE
  ${LangFileString} MULTIUSER_TEXT_INSTALLMODE_TITLE "Ҡулланыусыларҙы һайлағыҙ"
  ${LangFileString} MULTIUSER_TEXT_INSTALLMODE_SUBTITLE "Ниндәй файҙаланыусылар өсөн ҡуйырға теләйһегеҙ ?$(^NameDA)."
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_TOP "Инстальция итергә теләүегеҙҙе һайлағыҙ$(^NameDA)тик үҙең өсөн йәки был компьютерҙың бөтә ҡулланыусылары өсөн.$(^ClickNext)"
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_ALLUSERS "Был компьютер менән ҡулланыусы һәр кем өсөн ҡуйыу"
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_CURRENTUSER "Минең өсөн генә ҡуйығыҙ"
!endif
