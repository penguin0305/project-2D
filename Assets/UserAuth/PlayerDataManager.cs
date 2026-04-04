using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataManager : MonoBehaviour
{
    private readonly string baseUrl = "http://3.37.127.156:8080/api/playerdata"; // 로컬은 https://localhost:7026/api/playerdata

    public void FetchMyData()
    {
        StartCoroutine(GetPlayerDataRoutine());
    }

    private IEnumerator GetPlayerDataRoutine()
    {
        string token = PlayerPrefs.GetString("AuthToken", "");
        /*
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("저장된 토큰이 없습니다. 다시 로그인해주세요.");
            yield break; // 토큰이 없으면 중단
        }
        */

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
                
                // UI표시 스크립트
            }
            else
            {
                // 토큰이 만료되었거나 잘못된 경우 에러 메시지 출력  
                Debug.LogError($"데이터 로드 실패 ({request.responseCode}): {request.error}");
            }
        }
    }
}