using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AuthUIManager : MonoBehaviour, IAuthUI
{
    [Header("Login UI")]
    public TMP_InputField loginId;
    public TMP_InputField loginPw;

    [Header("Register UI")]
    public TMP_InputField regName;
    public TMP_InputField regId;
    public TMP_InputField regEmail;
    public TMP_InputField regPw;
    public TMP_InputField regNick;

    [Header("UI Control")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject startButton;
    public GameObject LoadingPanel;

    [Header("Error Popup")]
    public GameObject errorPopupPanel;
    public TMP_Text errorMessageText;

    public TMP_InputField[] allInputFields;
    public float popupDisplayTime = 2.0f;
    private Coroutine autoCloseCoroutine;
    private bool isPwVisible = false;

    // 프로퍼티 구현
    public bool IsLoadingPanelActive => LoadingPanel != null && LoadingPanel.activeInHierarchy;
    public bool IsRegisterPanelActive => registerPanel != null && registerPanel.activeInHierarchy;
    public bool IsLoginPanelActive => loginPanel != null && loginPanel.activeInHierarchy;

    // 메서드 구현
    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        if (registerPanel != null) registerPanel.SetActive(false);
        ResetAll();
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        if (registerPanel != null) registerPanel.SetActive(true);
    }

    public void ShowStartButton()
    {
        loginPanel.SetActive(false);
        if (registerPanel != null) registerPanel.SetActive(false);
        if (startButton != null) startButton.SetActive(true);
    }

    public void SetLoadingPanel(bool isActive)
    {
        if (LoadingPanel != null) LoadingPanel.SetActive(isActive);
    }

    public void ShowErrorPopup(string message)
    {
        if (errorPopupPanel != null && errorMessageText != null)
        {
            errorMessageText.text = message;
            errorPopupPanel.SetActive(true);

            // 중복 타이머 방지
            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
            }

            // 새 타이머 시작
            autoCloseCoroutine = StartCoroutine(AutoClosePopupRoutine());
        }
    }

    private IEnumerator AutoClosePopupRoutine()
    {
        // 설정한 시간만큼 대기
        yield return new WaitForSeconds(popupDisplayTime);
        CloseErrorPopup();
    }

    public void CloseErrorPopup()
    {
        if (errorPopupPanel != null)
        {
            errorPopupPanel.SetActive(false);
        }

        // 타이머 정리
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }
    }

    public void ToggleRegisterPasswordVisibility()
    {
        isPwVisible = !isPwVisible;

        if (isPwVisible)
        {
            regPw.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            // 비밀번호 숨기기
            regPw.contentType = TMP_InputField.ContentType.Password;
        }

        // 즉시 갱신
        regPw.ForceLabelUpdate();
    }

    public void ResetAll()
    {
        foreach(var input in allInputFields)
        {
            input.text = string.Empty;
        }
    }


    public bool ValidateRegisterInputs()
    {
        bool isValid = true;
        isValid &= SignUpValidateAndHighlight(regName);
        isValid &= SignUpValidateAndHighlight(regId);
        isValid &= SignUpValidateAndHighlight(regEmail);
        isValid &= SignUpValidateAndHighlight(regPw);
        isValid &= SignUpValidateAndHighlight(regNick);
        return isValid;
    }

    public bool ValidateLoginInputs()
    {
        bool isValid = true;
        isValid &= SignInValidateAndHighlight(loginId);
        isValid &= SignInValidateAndHighlight(loginPw);
        return isValid;
    }

    public RegisterRequest GetRegisterData()
    {
        return new RegisterRequest
        {
            Name = regName.text,
            Id = regId.text,
            Email = regEmail.text,
            Password = regPw.text,
            Username = regNick.text
        };
    }

    public LoginRequest GetLoginData()
    {
        return new LoginRequest
        {
            Id = loginId.text,
            Password = loginPw.text
        };
    }

    private bool SignUpValidateAndHighlight(TMP_InputField input)
    {
        if (string.IsNullOrWhiteSpace(input.text))
        {
            input.image.color = new Color(1f, 0.8f, 0.8f);
            return false;
        }
        else
        {
            input.image.color = Color.white;
            return true;
        }
    }

    private bool SignInValidateAndHighlight(TMP_InputField input)
    {
        Outline outline = input.GetComponent<Outline>();
        if (outline == null)
        {
            outline = input.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.white;
            outline.effectDistance = new Vector2(0, 0);
        }

        if (string.IsNullOrWhiteSpace(input.text))
        {
            outline.enabled = true;
            return false;
        }
        else
        {
            outline.enabled = false;
            return true;
        }
    }
}