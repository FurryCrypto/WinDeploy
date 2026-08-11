; Language: Azerbaijani (1068)
; ESD Installer project translation

!insertmacro LANGFILE "Azerbaijani" = "Azərbaycanca" "Azerbaijani"

!ifdef MUI_WELCOMEPAGE
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TITLE "$(^NameDA) Setup-ə xoş gəlmisiniz"
  ${LangFileString} MUI_TEXT_WELCOME_INFO_TEXT "Setup sizə $(^NameDA).$\r$\n$\r$\n-in quraşdırılması ilə bağlı bələdçilik edəcək. Setup-ə başlamazdan əvvəl bütün digər proqramları bağlamağınız tövsiyə olunur. Bu, kompüterinizi yenidən yükləmədən müvafiq sistem fayllarını yeniləməyə imkan verəcək.$\r$\n$\r$\n$_CLICK"
!endif

!ifdef MUI_DIRECTORYPAGE
  ${LangFileString} MUI_TEXT_DIRECTORY_TITLE "Quraşdırma yeri seçin"
  ${LangFileString} MUI_TEXT_DIRECTORY_SUBTITLE "$(^NameDA)-nin quraşdırılması üçün qovluğu seçin."
!endif

!ifdef MUI_INSTFILESPAGE
  ${LangFileString} MUI_TEXT_INSTALLING_TITLE "Quraşdırılır"
  ${LangFileString} MUI_TEXT_INSTALLING_SUBTITLE "$(^NameDA) quraşdırılarkən gözləyin."
  ${LangFileString} MUI_TEXT_FINISH_TITLE "Quraşdırma Tamamlandı"
  ${LangFileString} MUI_TEXT_FINISH_SUBTITLE "Setup uğurla tamamlandı."
  ${LangFileString} MUI_TEXT_ABORT_TITLE "Quraşdırma dayandırıldı"
  ${LangFileString} MUI_TEXT_ABORT_SUBTITLE "Setup uğurla tamamlanmadı."
!endif

!ifdef MUI_UNINSTFILESPAGE
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_TITLE "Silinir"
  ${LangFileString} MUI_UNTEXT_UNINSTALLING_SUBTITLE "$(^NameDA) silinən zaman gözləyin."
  ${LangFileString} MUI_UNTEXT_FINISH_TITLE "Silinmə tamamlandı"
  ${LangFileString} MUI_UNTEXT_FINISH_SUBTITLE "Silinmə uğurla tamamlandı."
  ${LangFileString} MUI_UNTEXT_ABORT_TITLE "Silinmə dayandırıldı"
  ${LangFileString} MUI_UNTEXT_ABORT_SUBTITLE "Silinmə uğurla tamamlanmadı."
!endif

!ifdef MUI_FINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_INFO_TITLE "$(^NameDA) Setup tamamlanır"
  ${LangFileString} MUI_TEXT_FINISH_INFO_TEXT "$(^NameDA) kompüterinizdə quraşdırılıb. $\r$\n$\r$\nSetup-ni bağlamaq üçün Bitir düyməsini basın."
  ${LangFileString} MUI_TEXT_FINISH_INFO_REBOOT "$(^NameDA) quraşdırılmasını başa çatdırmaq üçün kompüteriniz yenidən işə salınmalıdır. İndi yenidən yükləmək istəyirsiniz?"
!endif

!ifdef MUI_UNFINISHPAGE
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TITLE "$(^NameDA) Silinməsi tamamlanır"
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA) kompüterinizdən silindi.$\r$\n$\r$\nSetup-ni bağlamaq üçün Bitir düyməsini basın."
  ${LangFileString} MUI_UNTEXT_FINISH_INFO_REBOOT "$(^NameDA)-nin silinməsini başa çatdırmaq üçün kompüteriniz yenidən işə salınmalıdır. İndi yenidən yükləmək istəyirsiniz?"
!endif

!ifdef MUI_FINISHPAGE | MUI_UNFINISHPAGE
  ${LangFileString} MUI_TEXT_FINISH_REBOOTNOW "İndi yenidən başladın"
  ${LangFileString} MUI_TEXT_FINISH_REBOOTLATER "Daha sonra əl ilə yenidən yükləmək istəyirəm"
  ${LangFileString} MUI_TEXT_FINISH_RUN "&$(^NameDA)-ni işə salın"
  ${LangFileString} MUI_TEXT_FINISH_SHOWREADME "&Oxunu göstər"
  ${LangFileString} MUI_BUTTONTEXT_FINISH "&Bitir"
!endif

!ifdef MUI_UNCONFIRMPAGE
  ${LangFileString} MUI_UNTEXT_CONFIRM_TITLE "$(^NameDA)-ni silin"
  ${LangFileString} MUI_UNTEXT_CONFIRM_SUBTITLE "$(^NameDA)-ni kompüterinizdən çıxarın."
!endif

!ifdef MUI_ABORTWARNING
  ${LangFileString} MUI_TEXT_ABORTWARNING "$(^Name) Setup-dən çıxmaq istədiyinizə əminsiniz?"
!endif

!ifdef MUI_UNABORTWARNING
  ${LangFileString} MUI_UNTEXT_ABORTWARNING "$(^Name) Uninstall-dan çıxmaq istədiyinizə əminsiniz?"
!endif
