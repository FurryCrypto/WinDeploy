;Language: Iron (4099)
;By Joost Verburg

!insertmacro LANGFILE "Ossetian" = "Ирон" "Iron"

!ifdef MUI_WELCOMEPAGE
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TITLE "Дыккаг$(^NameDA)Бавæрд"
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TEXT "Уагӕвӕрд дын баххуыс кӕндзӕн, сӕвӕрынӕн ӕй хъӕуы$(^NameDA).$\r$\n$\r$\nУынаффӕ уадзын дӕ алы æфтуӕтты бахъхъӕнын, цалынмӕ сӕвӕрын кӕнай.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_UNWELCOMEPAGE
  ${LangFileString} MUI_UNTEXT_WELCOME_INFO_TITLE "Дыккаг$(^NameDA)Схафын"
  ${LangFileString} MUI_UNTEXT_WELCOME_INFO_TEXT "Уагӕвӕрд дын баххуыс кӕндзӕн, ногӕй дӕ куы нӕ адард кӕнай, уӕд$(^NameDA).$\r$\n$\r$\nРазӕй ногӕй рауадзын размӕ базон$(^NameDA)нӕ згъоры.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_LICENSEPAGE
  ${LangFileString} MUI_TEXT_LICENSE_TITLE "Литеративгӕ бадзырд"
  ${LangFileString} MUI_TEXT_LICENSE_SUBTITLE "Дӕ хорзӕхӕй, равдисыны размӕ лимӕнты уаг аскъуыддзаг кӕнын$(^NameDA)."
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM "Кӕд дӕуӕн дӕр дӕ уагӕвӕрдтӕм гӕсгӕ цӕрыс, уӕд дыккаг бакӕндзынӕ, цӕмӕй дарддӕр дӕр уай.$(^NameDA)."
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM_CHECKBOX "Кӕд бадзырды уагӕвӕрдтӕм гӕсгӕ цӕрыс, уӕд ныккӕс фынӕйы фӕлгӕты. Хъуамӕ бакӕнай, цӕмӕй сӕвӕрай бадзырд.$(^NameDA).$_CLICK"
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM_RADIOBUTTONS "Кӕд бадзырды хъуыддӕгты райсӕм, уӕд равзарын фыццаг миниуӕг. Хъуамӕ бакӕнай, цӕмӕй сӕвӕрай бадзырд.$(^NameDA).$_CLICK"
!endif

!ifdef MUI_UNLICENSEPAGE
  ${LangFileString} MUI_UNTEXT_LICENSE_TITLE "Литеративгӕ бадзырд"
  ${LangFileString} MUI_UNTEXT_LICENSE_SUBTITLE "Ныффыс лӕвӕрд уагӕвӕрдтӕ, размӕ$(^NameDA)."
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM "Кӕд дӕ бадзырды уагӕвӕрдтӕм гӕсгӕ архайыс, уӕд кликсут, цӕмӕй дарддӕр дӕр уа.$(^NameDA)."
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM_CHECKBOX "Кӕд ды бадзырды уагӕвӕрдтӕм гӕсгӕ архайыс, уӕд дӕхи фӕхъусдзынӕ. Хъуамӕ бадзырд ахицӕн кӕнай, цӕмӕй сӕвӕрай$(^NameDA).$_CLICK"
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM_RADIOBUTTONS "Кӕд ды бадзырды уагӕвӕрдтӕм гӕсгӕ архайыс, уӕд равзар дӕ фыццаг миниуӕг. Хъуамӕ бадзырд ахицӕн кӕнай, цӕмӕй сӕвӕрай$(^NameDA).$_CLICK"
!endif

!ifdef MUI_LICENSEPAGE | MUI_UNLICENSEPAGE
  ${LangFileString} MUI_INNERTEXT_LICENSE_TOP "Фӕссис Паge Down, цӕмӕй уыцы нырма бадзырды кой уа."
!endif

!ifdef MUI_COMPONENTSPAGE
  ${LangFileString} MUI_TEXT_COMPONENTS_TITLE "Равзарын компоненттӕ"
  ${LangFileString} MUI_TEXT_COMPONENTS_SUBTITLE "Равзар, цавӕр миниуджытӕ$(^NameDA)Дæ фæнды сӕвӕрын."
!endif

!ifdef MUI_UNCOMPONENTSPAGE
  ${LangFileString} MUI_UNTEXT_COMPONENTS_TITLE "Равзарын компоненттӕ"
  ${LangFileString} MUI_UNTEXT_COMPONENTS_SUBTITLE "Равзар, цавӕр миниуджытӕ$(^NameDA)дӕ фӕнды ахицӕн кӕнын."
!endif

!ifdef MUI_COMPONENTSPAGE | MUI_UNCOMPONENTSPAGE
  ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_TITLE "Фыст"
  !ifndef NSIS_CONFIG_COMPONENTPAGE_ALTERNATIVE
    ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_INFO "Дӕ миты ныхмӕ кӕмдӕр къордмӕ ӕвӕр, уый фенӕн ис."
  !else
    ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_INFO "Равзарын дзы чидӕр, цӕмӕй йӕ базона."
  !endif
!endif

!ifdef MUI_DIRECTORYPAGE
  ${LangFileString} MUI_TEXT_DIRECTORY_TITLE "Равзарын бынат сӕвӕрын"
  ${LangFileString} MUI_TEXT_DIRECTORY_SUBTITLE "Равзарын, кӕцы папкӕйы хъуамӕ сӕвӕра$(^NameDA)."
!endif

!ifdef MUI_UNDIRECTORYPAGE
  ${LangFileString} MUI_UNTEXT_DIRECTORY_TITLE "Равзарын хицӕн бынат"
  ${LangFileString} MUI_UNTEXT_DIRECTORY_SUBTITLE "Равзарын папкӕ, кӕцыйӕ ахицӕн кӕнын хъӕуы$(^NameDA)."
!endif

!ifdef MUI_INSTFILESPAGE
  ${LangFileString} MUI_TEXT_INSTALLING_TITLE "Сӕвӕрын"
  ${LangFileString} MUI_TEXT_INSTALLING_SUBTITLE "Ӕрнӕкъуаджы ӕнхъӕлмӕ кӕсын$(^NameDA)ӕвӕрд ӕрцыд."
  ${LangFileString} MUI_TEXT_FINISH_TITLE "Ӕвӕрддзинад ӕнӕхъӕнӕй"
  ${LangFileString} MUI_TEXT_FINISH_SUBTITLE "Ӕвзӕрст ӕрцыд."
  ${LangFileString} MUI_TEXT_ABORT_TITLE "Ӕвӕрддзинад ныууагъта"
  ${LangFileString} MUI_TEXT_ABORT_SUBTITLE "Нӕ рауадис фӕци."
!endif

!ifdef MUI_UNINSTFILESPAGE
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_TITLE "Схафын"
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_SUBTITLE "Ӕрнӕкъуаджы ӕнхъӕлмӕ кӕсын$(^NameDA)ногӕй цӕуы."
  ${LangFileString} MUI_UNTEXT_FINISH_TITLE "Схафын ӕнӕхъӕнӕй"
  ${LangFileString} MUI_UNTEXT_FINISH_SUBTITLE "Схафын ӕнӕхъӕнӕй сӕххӕст."
  ${LangFileString} MUI_UNTEXT_ABORT_TITLE "Ныууадзын"
  ${LangFileString} MUI_UNTEXT_ABORT_SUBTITLE "Схафын ӕнӕхъӕнӕй нӕ фӕци."
!endif

!ifdef MUI_FINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_INFO_TITLE "Сӕххӕст кӕнын$(^NameDA)Бавæрд"
  ${LangFileString} MUI_TEXT_FINISH_INFO_TEXT "$(^NameDA)уӕ компьютеры ӕвӕрд ӕрцыд.$\r$\n$\r$\nКлассы кæнын, цӕмӕй сӕвӕрын бахъӕуа."
  ${LangFileString} MUI_TEXT_FINISH_INFO_REBOOT "Хъуамӕ дӕ компьютеры ногӕй баиу уай, цӕмӕй сӕвӕрынӕн кӕрон райдайай.$(^NameDA). Ныр дӕ фӕнды ногӕй баиу кӕнын?"
!endif

!ifdef MUI_UNFINISHPAGE
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TITLE "Сӕххӕст кӕнын$(^NameDA)Схафын"
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA)хицӕн кодта дӕ компьютерӕй.$\r$\n$\r$\nКлассы кæнын, цӕмӕй сӕвӕрын бахъӕуа."
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_REBOOT "Хъуамӕ дӕ компьютеры ногӕй баиу уай, цӕмӕй ногӕй ахицӕн уай$(^NameDA). Ныр дӕ фӕнды ногӕй баиу кӕнын?"
!endif

!ifdef MUI_FINISHPAGE | MUI_UNFINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_REBOOTNOW "Нырдӕгъы баиу кӕнын"
  ${LangFileString} MUI_TEXT_FINISH_REBOOTLATER "Фӕнды мӕ фӕстӕдӕр ӕххуысы руаджы ногӕй баиу кӕнын"
  ${LangFileString} MUI_TEXT_FINISH_RUN "& Баиу кӕнын$(^NameDA)"
  ${LangFileString} MUI_TEXT_FINISH_SHOWREADME "& Бакасын мӕ"
  ${LangFileString} MUI_BUTTONTEXT_FINISH "&Сфӕразын"
!endif

!ifdef MUI_STARTMENUPAGE
  ${LangFileString} MUI_TEXT_STARTMENU_TITLE "Равзарын райдиан меню папкӕ"
  ${LangFileString} MUI_TEXT_STARTMENU_SUBTITLE "Равзарын райдиан менюйы папкӕ$(^NameDA)ӕргъӕвдыст."
  ${LangFileString} MUI_INNERTEXT_STARTMENU_TOP "Равзарын равдисын фыццаг менюйы папкӕ, ӕвӕццӕгӕн, программӕйы ӕргъӕвтытӕ кӕм сфӕлдыстай. Дӕ бон у ног папкӕ сфӕнд кӕнынӕн дӕр номӕй бафӕрсын."
  ${LangFileString} MUI_INNERTEXT_STARTMENU_CHECKBOX "Нӕ сфӕнд кӕнын ӕргъӕвтӕ"
!endif

!ifdef MUI_UNCONFIRMPAGE
  ${LangFileString} MUI_UNTEXT_CONFIRM_TITLE "Схафын$(^NameDA)"
  ${LangFileString} MUI_UNTEXT_CONFIRM_SUBTITLE "Схафын$(^NameDA)дӕ компьютерӕй."
!endif

!ifdef MUI_ABORTWARNING
  ${LangFileString} MUI_TEXT_ABORTWARNING "Æцæг дæ фæнды ныххатыр кӕнын$(^Name)Бацӕттӕ кӕнынӕн?"
!endif

!ifdef MUI_UNABORTWARNING
  ${LangFileString} MUI_UNTEXT_ABORTWARNING "Æцæг дæ фæнды ныххатыр кӕнын$(^Name)Схафын?"
!endif

!ifdef MULTIUSER_INSTALLMODEPAGE
  ${LangFileString} MULTIUSER_TEXT_INSTALLMODE_TITLE "Равзарын кусджытӕ"
  ${LangFileString} MULTIUSER_TEXT_INSTALLMODE_SUBTITLE "Равзарын, кӕм дӕ фӕнды ӕвӕрдын, ахӕм кусӕнгæн$(^NameDA)."
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_TOP "Равзарын, дӕ фӕнды сӕвӕрын ӕви нӕ$(^NameDA)ӕрмӕстдӕр дӕхи тыххӕй кӕнӕ ацы компьютеры алкӕмӕн дӕр.$(^ClickNext)"
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_ALLUSERS "Ацы компьютерӕй пайда кӕнынӕн исчи сӕвӕрын"
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_CURRENTUSER "Ӕрмӕстдӕр мӕнӕн сӕвӕрын"
!endif
