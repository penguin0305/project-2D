using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // 프로퍼티 구현
    public bool IsLoadingPanelActive => LoadingPanel != null && LoadingPanel.activeInHierarchy;
    public bool IsRegisterPanelActive => registerPanel != null && registerPanel.activeInHierarchy;
    public bool IsLoginPanelActive => loginPanel != null && loginPanel.activeInHierarchy;

    // 메서드 구현
    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        if (registerPanel != null) registerPanel.SetActive(false);
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
            Nickname = regNick.text
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