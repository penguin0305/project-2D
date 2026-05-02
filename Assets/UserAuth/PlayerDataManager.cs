using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataManager : MonoBehaviour
{
    private readonly string baseUrl = "http://3.37.127.156:8080/api/playerdata"; // 로컬은 https://localhost:7026/api/playerdata

    public void FetchMyData()
    {
        StartCoroutine(GetPlayerDataRoutine());
    }

    public void SaveMyData()
    {
        StartCoroutine(SavePlayerDataRoutine());
    }

    private IEnumerator GetPlayerDataRoutine()
    {
        string token = PlayerPrefs.GetString("AuthToken", "");
        
        if (string.IsNullOrEmpty(token)) yield break; // 토큰이 없으면 중단


        // GET request
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl))
        {
            request.SetRequestHeader("Authorization", $"Bearer {token}");

            // HTTPS 인증서 우회 ( 나중에 지움 )
            request.certificateHandler = new BypassCertificate();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // JSON to PlayerData
                string jsonResponse = request.downloadHandler.text;
                PlayerData pData = JsonUtility.FromJson<PlayerData>(jsonResponse);
                Debug.Log($"데이터 로드, 닉네임: {pData.username}, 레벨: {pData.level}, 재화: {pData.currency}");

                PlayerSession.Instance.UpdateSessionData(pData);
            }
            else
            {
                // 토큰이 만료되었거나 잘못된 경우 에러 메시지 출력  
                Debug.LogError($"데이터 로드 실패 ({request.responseCode}): {request.error}");
            }
        }
     }

    private IEnumerator SavePlayerDataRoutine()
    {
        PlayerData saveData = new PlayerData
        {
            id = PlayerSession.Instance.Id,
            username = PlayerSession.Instance.Username,
            level = PlayerSession.Instance.Level,
            exp = PlayerSession.Instance.Exp,
            currency = PlayerSession.Instance.Currency,
            playeritems = PlayerSession.Instance.PlayerItems
        };

        string jsonData = JsonUtility.ToJson(saveData);
        string token = PlayerPrefs.GetString("AuthToken", "");
        if (string.IsNullOrEmpty(token)) yield break; // 토큰이 없으면 중단

        string updateUrl = baseUrl + "/update";

        using (UnityWebRequest request = new UnityWebRequest(updateUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 헤더 설정 
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {token}");

            // 요청
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("데이터 저장 완료: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"데이터 저장 실패 ({request.responseCode}): {request.error}");
            }
        }
    }
}