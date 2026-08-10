;Language: Apsua (4098)
;Translation updated by Dmitry Yerokhin [erodim@mail.ru] (050424)

!insertmacro LANGFILE "Abkhazian" = "Аҧсшәа" "Apsua"

!ifdef MUI_WELCOMEPAGE
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TITLE "Шәахәаԥш ҳәа шәҳәоит астудент –$(^NameDA)"
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TEXT "Ари апрограмма ашьақәыргылара»$(^NameDA)Идырбала шәкомпиутер ахь$\r$\n$\r$\nАаргылара алагараанӡа, аус зуа апринципқәа зегьы рышьақәыргылатәуп. Уи ашьақәыргылара апрограмма акомпиутер еиҭалагаламкәа асистематә фаилқәа рҭахрақәа рԥатәуп$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_UNWELCOMEPAGE
  ${LangFileString} MUI_UNTEXT_WELCOME_INFO_TITLE "Шәарҭ шәидысныҳәоит адемонстрациа аус зуа –$(^NameDA)"
  ${LangFileString} MUI_UNTEXT_WELCOME_INFO_TEXT "Ари апрограмма аныхрахоит»$(^NameDA)Иахьа шәкомпиутер аҟынтә$\r$\n$\r$\nАныхра алагара шәалагаанӡа шәазхиаԥш апрограмма шынагӡоу$(^NameDA)Амузыка аус ауам$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_LICENSEPAGE
  ${LangFileString} MUI_TEXT_LICENSE_TITLE "Алицензиатәи аиқәшаҳаҭра"
  ${LangFileString} MUI_TEXT_LICENSE_SUBTITLE "Ашьақәыргылара алагамҭазы»$(^NameDA)Алитессиатә еизыҟазаашьақәа шәрыхәаԥш."
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM "Аиқәшаҳаҭра аҳәаақәа шәрықәшәозар, апытә» шәахәаԥш$\"Схы сбоит$\". Апрограмма ааргыларазы, аиқәшаҳаҭра адкылара ауп."
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM_CHECKBOX "Аиқәшаҳаҭра аҳәаақәа шәрықәшәо, анаҩстәи абираҭ аргыла. Апрограмма ааргыларазы аиқәшаҳаҭра рызнагара ауп. –$_CLICK"
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM_RADIOBUTTONS "Аиқәшаҳаҭра аҳәаақәа шәрықәшәозар, анаҩстәи алагамҭақәа рахь актәи авариант алх. Апрограмма аарԥшразы аиқәшаҳаҭра рызнагара ауп. «$_CLICK"
!endif

!ifdef MUI_UNLICENSEPAGE
  ${LangFileString} MUI_UNTEXT_LICENSE_TITLE "Алицензиатәи аиқәшаҳаҭра"
  ${LangFileString} MUI_UNTEXT_LICENSE_SUBTITLE "Аныхраанӡа»$(^NameDA)Алитессиатә еизыҟазаашьақәа шәрыхәаԥш."
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM "Аиқәшаҳаҭра аҳәаақәа шәрықәшәозар, апытә» шәахәаԥш$\"Схы сбоит$\". Акразы аиқәшаҳаҭра рыдыркытәуп. –$_CLICK"
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM_CHECKBOX "Аиқәшаҳаҭра аҳәаақәа шәрықәшәо, абираҭ анаҩстәи аҿы аргыла. Аныхразы аиқәшаҳаҭра рызнагара ауп. –$_CLICK"
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM_RADIOBUTTONS "Аиқәшаҳаҭра аҳәаақәа шәрықәшәо, анаҩстәи алагамҭақәа рахь актәи авариант алх. Аныхразы аиқәшаҳаҭра рызнагара ауп. –$_CLICK"
!endif

!ifdef MUI_LICENSEPAGE | MUI_UNLICENSEPAGE
  ${LangFileString} MUI_INNERTEXT_LICENSE_TOP "Атекст ала аиҭакразы шәақәдырхәа аклавишьқәа»$\"PageUp$\"«Ишьақәнарӷәӷәо»$\"PageDown$\"."
!endif

!ifdef MUI_COMPONENTSPAGE
  ${LangFileString} MUI_TEXT_COMPONENTS_TITLE "Ишьақәыргыло апрограмма акомпонентқәа"
  ${LangFileString} MUI_TEXT_COMPONENTS_SUBTITLE "Иалхтәуп акомпонентқәа»$(^NameDA)Ишьақәыргылара шәҭаху."
!endif

!ifdef MUI_UNCOMPONENTSPAGE
  ${LangFileString} MUI_UNTEXT_COMPONENTS_TITLE "Апрограмма акомпонентқәа"
  ${LangFileString} MUI_UNTEXT_COMPONENTS_SUBTITLE "Иалхтәуп акомпонентқәа»$(^NameDA)Ишәыхтәуп ҳәа шәҭаху"
!endif

!ifdef MUI_COMPONENTSPAGE | MUI_UNCOMPONENTSPAGE
  ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_TITLE "Асахьақәа"
  !ifndef NSIS_CONFIG_COMPONENTPAGE_ALTERNATIVE
    ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_INFO "Идырба амышь қырсор акомпонент ахьӡ, уи ахкы шәаԥхьац азы."
  !else
    ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_INFO "Иалхтәуп акомпонент, уи асахра шәахәаԥш."
  !endif
!endif

!ifdef MUI_DIRECTORYPAGE
  ${LangFileString} MUI_TEXT_DIRECTORY_TITLE "Ааргылара афаил алхра"
  ${LangFileString} MUI_TEXT_DIRECTORY_SUBTITLE "Иалхтәуп аҭа ашьақәыргыларазы»$(^NameDA)."
!endif

!ifdef MUI_UNDIRECTORYPAGE
  ${LangFileString} MUI_UNTEXT_DIRECTORY_TITLE "Афаил аныхразы аԥалхра"
  ${LangFileString} MUI_UNTEXT_DIRECTORY_SUBTITLE "Идырба афальт, уи аҟынтә иныхтәуп»$(^NameDA)."
!endif

!ifdef MUI_INSTFILESPAGE
  ${LangFileString} MUI_TEXT_INSTALLING_TITLE "Афаилқәа ркопиа"
  ${LangFileString} MUI_TEXT_INSTALLING_SUBTITLE "Шәазышәк, афаилқәа ркопиа шцало»$(^NameDA)..."
  ${LangFileString} MUI_TEXT_FINISH_TITLE "Ааргылара хыркәшан"
  ${LangFileString} MUI_TEXT_FINISH_SUBTITLE "Ааргылара инапынҭақәа рыла хыркәшахоит"
  ${LangFileString} MUI_TEXT_ABORT_TITLE "Ааргылара хықәкыс имоуп"
  ${LangFileString} MUI_TEXT_ABORT_SUBTITLE "Ааргылара хыркәам"
!endif

!ifdef MUI_UNINSTFILESPAGE
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_TITLE "Аныхра"
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_SUBTITLE "Шәазышәк, афаилқәа рышьақәыргылара иалагоит»$(^NameDA)..."
  ${LangFileString} MUI_UNTEXT_FINISH_TITLE "Аныхра хыркәшан"
  ${LangFileString} MUI_UNTEXT_FINISH_SUBTITLE "Апрограмма аныхра инапынҭақәа рыла хыркәшахоит"
  ${LangFileString} MUI_UNTEXT_ABORT_TITLE "Аныхра хықәкыс имоуп"
  ${LangFileString} MUI_UNTEXT_ABORT_SUBTITLE "Аныхра инаам"
!endif

!ifdef MUI_FINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_INFO_TITLE "Ааргылара аус хыркәшара»$(^NameDA)"
  ${LangFileString} MUI_TEXT_FINISH_INFO_TEXT "Ааргылара»$(^NameDA)Иалкаауп.$\r$\n$\r$\nАпытәҭ» шәақәыргыла$\"Ишьақәыргылоуп$\"Ааргылара апрограмма аҟынтә аара."
  ${LangFileString} MUI_TEXT_FINISH_INFO_REBOOT "Ааргылара хыркәшаразы –$(^NameDA)Акомпиутер еиԥцатәуп. Уи уажәы мҩаԥыргашәома?"
!endif

!ifdef MUI_UNFINISHPAGE
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TITLE "Аныхра астудент аусура хыркәшара»$(^NameDA)"
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TEXT "Апрограмма –$(^NameDA)Иныхтәуп шәкомпиутер аҟынтә.$\r$\n$\r$\nАпытәҭ» шәақәыргыла$\"Ишьақәыргылоуп$\"Акра апрограмма аҟынтә аныхра."
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_REBOOT "Аныхра ахыркәшамҭазы»$(^NameDA)Акомпиутер еиԥцатәуп. Уи уажәы мҩаԥыргашәома?"
!endif

!ifdef MUI_FINISHPAGE | MUI_UNFINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_REBOOTNOW "Ааи, Акомпьютер уажәы еиҭагара"
  ${LangFileString} MUI_TEXT_FINISH_REBOOTLATER "Мап, сара ПК анаҩс еицыргоит"
  ${LangFileString} MUI_TEXT_FINISH_RUN "&Ачра»$(^NameDA)"
  ${LangFileString} MUI_TEXT_FINISH_SHOWREADME "&Идырба афаил ReadMe"
  ${LangFileString} MUI_BUTTONTEXT_FINISH "&Ишьақәыргылара"
!endif

!ifdef MUI_STARTMENUPAGE
  ${LangFileString} MUI_TEXT_STARTMENU_TITLE "Амениу ахь аҭаҭ$\"Агара$\""
  ${LangFileString} MUI_TEXT_STARTMENU_SUBTITLE "Амениу ахь аҭа алх$\"Агара$\"Апрограмма абираҭқәа рышьақәыргыларазы."
  ${LangFileString} MUI_INNERTEXT_STARTMENU_TOP "Амениу ахь аҭа алх$\"Агара$\", аппл. Акрақәа ахьаргылахо. Иара убас, афальт егьырҭ ахьӡ шәақәҳаршәалар шәылшоит"
  ${LangFileString} MUI_INNERTEXT_STARTMENU_CHECKBOX "Адыргақәа рыҟаҵара ауам"
!endif

!ifdef MUI_UNCONFIRMPAGE
  ${LangFileString} MUI_UNTEXT_CONFIRM_TITLE "Аныхра»$(^NameDA)"
  ${LangFileString} MUI_UNTEXT_CONFIRM_SUBTITLE "Аныхра»$(^NameDA)Акомпиутер аҟынтә"
!endif

!ifdef MUI_ABORTWARNING
  ${LangFileString} MUI_TEXT_ABORTWARNING "Ишаҭахума шәара ааргыла аркра»$(^Name)?»"
!endif

!ifdef MUI_UNABORTWARNING
  ${LangFileString} MUI_UNTEXT_ABORTWARNING "Ишәҭахума шәакәзар аныхра»$(^Name)?»"
!endif

!ifdef MULTIUSER_INSTALLMODEPAGE
  ${LangFileString} MULTIUSER_TEXT_INSTALLMODE_TITLE "Ашьақәыргылара арежим"
  ${LangFileString} MULTIUSER_TEXT_INSTALLMODE_SUBTITLE "Иалх, иарбан заҳархәаҩцәа рзы аартраҭ арара шәҭаху»$(^NameDA)."
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_TOP "Иалх, шәақәгылара шәымазар»$(^NameDA)Иахьа, ма арикомпиутер ахархәара змоу зегьы рзы. –$(^ClickNext)"
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_ALLUSERS "Ахархәаҩцәа зегьы рзы ашьақәыргылара"
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_CURRENTUSER "Ишьақәыргылатәуп уажәтәи ахатәҩызы мацара"
!endif
