public interface IAuthUI
{
    // 패널 활성화 상태 확인
    bool IsLoadingPanelActive { get; }
    bool IsRegisterPanelActive { get; }
    bool IsLoginPanelActive { get; }

    // 패널 제어
    void ShowLoginPanel();
    void ShowRegisterPanel();
    void ShowStartButton();
    void SetLoadingPanel(bool isActive);

    //에러 팝업
    void ShowErrorPopup(string message);

    // 유효성 검사 및 데이터 수집
    bool ValidateRegisterInputs();
    bool ValidateLoginInputs();
    RegisterRequest GetRegisterData();
    LoginRequest GetLoginData();
}