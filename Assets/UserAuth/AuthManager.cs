#define LOCALn
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
#if LOCAL
    private readonly string baseUrl = "https://localhost:7026/api/account";
#else
    private readonly string baseUrl = "http://3.37.127.156:8080/api/account";
#endif
    private IAuthUI ui;
    private PlayerDataManager playerData;

    void Awake()
    {   
        ui = GetComponent<IAuthUI>();
        playerData = GetComponent<PlayerDataManager>();
    }

    public void OnClickRegister()
    {
        if (!ui.ValidateRegisterInputs())
        {
            ui.ShowErrorPopup("All fields are required.");
            return;
        }

        var data = ui.GetRegisterData();
        StartCoroutine(PostRequest($"{baseUrl}/register", JsonUtility.ToJson(data), false));
    }

    public void OnClickLogin()
    {
        if (!ui.ValidateLoginInputs())
        {
            ui.ShowErrorPopup("Invalid ID or password.");
            return;
        }

        var data = ui.GetLoginData();
        StartCoroutine(PostRequest($"{baseUrl}/login", JsonUtility.ToJson(data), true));
    }

    public void OnClickGuestLogin()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        string json = $"\"{deviceId}\"";
        StartCoroutine(PostRequest($"{baseUrl}/guest-login", json, true));
    }

    public void OnClickShowRegisterPanel()
    {
        ui.ShowRegisterPanel();
    }

    void Update()
    {
        // 로딩 중이면 입력 무시
        if (ui.IsLoadingPanelActive) return;

        // 엔터키 처리
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (ui.IsRegisterPanelActive)
            {
                OnClickRegister();
            }
            else if (ui.IsLoginPanelActive)
            {
                OnClickLogin();
            }
        }

        // ESC키 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ui.IsRegisterPanelActive)
            {
                ui.ShowLoginPanel();
            }
        }
    }

    IEnumerator PostRequest(string url, string json, bool isLogin)
    {
        ui.SetLoadingPanel(true);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.certificateHandler = new BypassCertificate();

            yield return request.SendWebRequest();

            ui.SetLoadingPanel(false);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"성공: {request.downloadHandler.text}"); // 서버로부터 전달받은 로그

                if (isLogin)
                {
                    var res = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
                    PlayerPrefs.SetString("AuthToken", res.token);
                    PlayerPrefs.Save();
                    Debug.Log("토큰이 성공적으로 저장되었습니다.");
                    playerData.FetchMyData();

                    ui.ShowStartButton();
                }
                else
                {
                    ui.ShowLoginPanel();
                }
            }
            else
            {
                string serverMessage = request.downloadHandler.text;

                if (string.IsNullOrWhiteSpace(serverMessage))
                {
                    ui.ShowErrorPopup("Please check your network connection.");
                }
                // 예상치 못한 에러가 발생하는 경우
                else if (serverMessage.Contains("<!DOCTYPE html>") || serverMessage.Length > 100)
                {
                    ui.ShowErrorPopup("Please try again later.");
                }
                
                else
                {
                    ui.ShowErrorPopup(serverMessage);
                }
                Debug.Log($"에러 ({request.responseCode}): {serverMessage}");
            }
        }
    }
}

// HTTPS 테스트를 위한 인증서 우회 클래스
public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData) => true;
}
