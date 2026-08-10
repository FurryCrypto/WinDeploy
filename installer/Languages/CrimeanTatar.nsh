;Language: Qirimtatarca (4097)
;By Joost Verburg

!insertmacro LANGFILE "CrimeanTatar" = "Qırımtatarca" "Qirimtatarca"

!ifdef MUI_WELCOMEPAGE
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TITLE "Hoş keldiñiz$(^NameDA)Qurulış"
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TEXT "Qurulış siziñ qurulışıñız boyunca közetir.$(^NameDA)- Oña yardım etecek.$\r$\n$\r$\nQurulışnı başlatmadan evel diger bütün qullanmalarnı qapatmaq tevsiye etile. Bu, kompyuterni yañıdan çalıştırmayıp, mütehassıs sistem fayllarını yañartmağa imkân berecek.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_UNWELCOMEPAGE
  ${LangFileString} MUI_UNTEXT_WELCOME_INFO_TITLE "Hoş keldiñiz$(^NameDA)Qurulmadan çıqaruv"
  ${LangFileString} MUI_UNTEXT_WELCOME_INFO_TEXT "Qurulış, siziñ qurulışnı yoq etkeniñizge yol berecek.$(^NameDA)- Oña yardım etecek.$\r$\n$\r$\nQurulıştan çıqma başlamazdan evel emin olun$(^NameDA)qaçmay.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_LICENSEPAGE
  ${LangFileString} MUI_TEXT_LICENSE_TITLE "Litsenziya añlaşması"
  ${LangFileString} MUI_TEXT_LICENSE_SUBTITLE "Lütfen, qurulmadan evel litsenziya şartlarını baqıñız$(^NameDA)- Oña yardım etecek."
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM "Eger añlaşmanıñ şartlarını qabul etesiñiz, devam etmege razı olğanımnı basıñız.$(^NameDA)- Oña yardım etecek."
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM_CHECKBOX "Eger añlaşmanıñ şartlarını qabul etesiñiz, aşağıdaki işaretleme saifesine tıklañız.$(^NameDA).$_CLICK"
  ${LangFileString} MUI_INNERTEXT_LICENSE_BOTTOM_RADIOBUTTONS "Eger añlaşmanıñ şartlarını qabul etesiñiz, aşağıdaki birinci variantnı saylañız.$(^NameDA).$_CLICK"
!endif

!ifdef MUI_UNLICENSEPAGE
  ${LangFileString} MUI_UNTEXT_LICENSE_TITLE "Litsenziya añlaşması"
  ${LangFileString} MUI_UNTEXT_LICENSE_SUBTITLE "Silâdan çıqmazdan evel litsenziyanıñ şartlarını ögrenmek içün rica etemiz$(^NameDA)- Oña yardım etecek."
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM "Añlaşmanıñ şartlarını qabul etesiñiz, devam etmege razı olğanımnı basıñız.$(^NameDA)- Oña yardım etecek."
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM_CHECKBOX "Añlaşmanıñ şartlarını qabul etesiñiz, aşağıdaki işaretleme saifesine basıñız.$(^NameDA).$_CLICK"
  ${LangFileString} MUI_UNINNERTEXT_LICENSE_BOTTOM_RADIOBUTTONS "Añlaşmanıñ şartlarını qabul etesiñiz, aşağıdaki birinci variantnı saylañız.$(^NameDA).$_CLICK"
!endif

!ifdef MUI_LICENSEPAGE | MUI_UNLICENSEPAGE
  ${LangFileString} MUI_INNERTEXT_LICENSE_TOP "Añlaşmanıñ qalğan qısmını baqmaq içün Sayfayı basıñız."
!endif

!ifdef MUI_COMPONENTSPAGE
  ${LangFileString} MUI_TEXT_COMPONENTS_TITLE "Qısımlarnı sayla"
  ${LangFileString} MUI_TEXT_COMPONENTS_SUBTITLE "Qaysı hususiyetlerni saylañız$(^NameDA)qurmaq isteysiñiz."
!endif

!ifdef MUI_UNCOMPONENTSPAGE
  ${LangFileString} MUI_UNTEXT_COMPONENTS_TITLE "Qısımlarnı sayla"
  ${LangFileString} MUI_UNTEXT_COMPONENTS_SUBTITLE "Qaysı hususiyetlerni saylañız$(^NameDA)qurulmasını lâğu etmege isteysiñiz."
!endif

!ifdef MUI_COMPONENTSPAGE | MUI_UNCOMPONENTSPAGE
  ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_TITLE "Tafsiri"
  !ifndef NSIS_CONFIG_COMPONENTPAGE_ALTERNATIVE
    ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_INFO "Tüyeniñizni bir qısımnıñ üstünde yerleştir ve onıñ tasvirini kör."
  !else
    ${LangFileString} MUI_INNERTEXT_COMPONENTS_DESCRIPTION_INFO "Onıñ tarifini körmek içün bir komponentni sayla."
  !endif
!endif

!ifdef MUI_DIRECTORYPAGE
  ${LangFileString} MUI_TEXT_DIRECTORY_TITLE "Qurulğan yerni sayla"
  ${LangFileString} MUI_TEXT_DIRECTORY_SUBTITLE "Qurmaq içün klaserni sayla$(^NameDA)- Oña yardım etecek."
!endif

!ifdef MUI_UNDIRECTORYPAGE
  ${LangFileString} MUI_UNTEXT_DIRECTORY_TITLE "Yerni yoq etüvni sayla"
  ${LangFileString} MUI_UNTEXT_DIRECTORY_SUBTITLE "Qurulmadan çıqaruv içün klaskanı sayla$(^NameDA)- Oña yardım etecek."
!endif

!ifdef MUI_INSTFILESPAGE
  ${LangFileString} MUI_TEXT_INSTALLING_TITLE "Qurulış"
  ${LangFileString} MUI_TEXT_INSTALLING_SUBTITLE "Lütfen , bir vaqıt bekleñiz .$(^NameDA)qurula."
  ${LangFileString} MUI_TEXT_FINISH_TITLE "Qurulış tamamlandı"
  ${LangFileString} MUI_TEXT_FINISH_SUBTITLE "Qurulış muvafaqiyetnen bitirildi."
  ${LangFileString} MUI_TEXT_ABORT_TITLE "Qurulış toqtatıldı"
  ${LangFileString} MUI_TEXT_ABORT_SUBTITLE "Qurulış muvafaqiyetli bitirilmedi."
!endif

!ifdef MUI_UNINSTFILESPAGE
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_TITLE "Qurulmadan çıqaruv"
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_SUBTITLE "Lütfen , bir vaqıt bekleñiz .$(^NameDA)qurulıp çıqarıla."
  ${LangFileString} MUI_UNTEXT_FINISH_TITLE "Qurultaydan çıqaruv bitken"
  ${LangFileString} MUI_UNTEXT_FINISH_SUBTITLE "Qurultaynı yoq etüv muvafaqiyetnen yekünlendi."
  ${LangFileString} MUI_UNTEXT_ABORT_TITLE "Qurulmadan çıqaruv toqtatıldı"
  ${LangFileString} MUI_UNTEXT_ABORT_SUBTITLE "Qurulıştan çıqaruv muvafaqiyetnen bitirilmedi."
!endif

!ifdef MUI_FINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_INFO_TITLE "Tamamlav$(^NameDA)Qurulış"
  ${LangFileString} MUI_TEXT_FINISH_INFO_TEXT "$(^NameDA)kompyuterinize qurulğan.$\r$\n$\r$\nQararnı qapatmaq içün Bitirgenini tıklañız."
  ${LangFileString} MUI_TEXT_FINISH_INFO_REBOOT "Kompüterniñ qurulışı bitmek içün yañıdan başlatılması kerek.$(^NameDA)Şimdi yañıdan başlatmaq istersiñmi?"
!endif

!ifdef MUI_UNFINISHPAGE
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TITLE "Tamamlav$(^NameDA)Qurulmadan çıqaruv"
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA)kompyuterinden yoq etildi.$\r$\n$\r$\nQararnı qapatmaq içün Bitirgenini tıklañız."
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_REBOOT "$(^NameDA)Şimdi yañıdan başlatmaq istersiñmi?"
!endif

!ifdef MUI_FINISHPAGE | MUI_UNFINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_REBOOTNOW "Şimdi yañıdan başlat"
  ${LangFileString} MUI_TEXT_FINISH_REBOOTLATER "Men daa soñra elnen yañıdan başlatmaq isteyim"
  ${LangFileString} MUI_TEXT_FINISH_RUN "&amp; Run$(^NameDA)"
  ${LangFileString} MUI_TEXT_FINISH_SHOWREADME "&Show Readme"
  ${LangFileString} MUI_BUTTONTEXT_FINISH "& Finish"
!endif

!ifdef MUI_STARTMENUPAGE
  ${LangFileString} MUI_TEXT_STARTMENU_TITLE "Başlanğıç menü klaskasını sayla"
  ${LangFileString} MUI_TEXT_STARTMENU_SUBTITLE "içün Başlanğıç menü klasörini sayla$(^NameDA)qısqa yollar."
  ${LangFileString} MUI_INNERTEXT_STARTMENU_TOP "Programmanıñ qısqa yollarını yaratmaq istegen Başlav menü klaskasını sayla. Yañı klaska yaratmaq içün bir ad da kirsetesiñiz mümkün."
  ${LangFileString} MUI_INNERTEXT_STARTMENU_CHECKBOX "Qısqartmalar yaratma"
!endif

!ifdef MUI_UNCONFIRMPAGE
  ${LangFileString} MUI_UNTEXT_CONFIRM_TITLE "Qurulmadan çıqaruv$(^NameDA)"
  ${LangFileString} MUI_UNTEXT_CONFIRM_SUBTITLE "Çıqtır$(^NameDA)kompyuterinden."
!endif

!ifdef MUI_ABORTWARNING
  ${LangFileString} MUI_TEXT_ABORTWARNING "İstiyorsan , lâğu etmege isteysiñmi ?$(^Name)Qurulış?"
!endif

!ifdef MUI_UNABORTWARNING
  ${LangFileString} MUI_UNTEXT_ABORTWARNING "İstiyorsan , lâğu etmege isteysiñmi ?$(^Name)Qurultaynı yoq etmek?"
!endif

!ifdef MULTIUSER_INSTALLMODEPAGE
  ${LangFileString} MULTIUSER_TEXT_INSTALLMODE_TITLE "Kullanıcılarnı sayla"
  ${LangFileString} MULTIUSER_TEXT_INSTALLMODE_SUBTITLE "Ne içün qurmaq istegen qullanıcılarnı sayla$(^NameDA)- Oña yardım etecek."
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_TOP "Qurmaq istegenini sayla$(^NameDA)yalıñız özüñiz içün ya da bu kompyuterniñ bütün qullanıcıları içün.$(^ClickNext)"
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_ALLUSERS "Bu kompyuterni qullanğan er kes içün qur"
  ${LangFileString} MULTIUSER_INNERTEXT_INSTALLMODE_CURRENTUSER "Tek menim içün qur"
!endif
