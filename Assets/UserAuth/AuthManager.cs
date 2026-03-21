using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    private readonly string baseUrl = "https://localhost:7026/api/account";
    private IAuthUI ui;

    void Awake()
    {
        ui = GetComponent<IAuthUI>();
    }

    public void OnClickRegister()
    {
        if (!ui.ValidateRegisterInputs())
        {
            Debug.LogWarning("모든 필드를 입력해주세요.");
            return;
        }

        var data = ui.GetRegisterData();
        StartCoroutine(PostRequest($"{baseUrl}/register", JsonUtility.ToJson(data), false));
    }

    public void OnClickLogin()
    {
        if (!ui.ValidateLoginInputs())
        {
            Debug.LogWarning("모든 필드를 입력해주세요.");
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
                Debug.Log($"성공: {request.downloadHandler.text}");

                if (isLogin)
                {
                    var res = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
                    PlayerPrefs.SetString("AuthToken", res.token);
                    PlayerPrefs.Save();
                    Debug.Log("토큰이 성공적으로 저장되었습니다.");

                    ui.ShowStartButton();
                }
                else
                {
                    ui.ShowLoginPanel();
                }
            }
            else
            {
                Debug.LogError($"에러 ({request.responseCode}): {request.downloadHandler.text}");
            }
        }
    }
}

// HTTPS 테스트를 위한 인증서 우회 클래스
public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData) => true;
}
