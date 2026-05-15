// #define LOCALn
// using System;
// using System.Collections;
// using System.Text;
// using System.Transactions;
// using UnityEngine;
// using UnityEngine.Networking;
// using System.Linq;

// public class PlayerDataManager : MonoBehaviour
// {
// #if LOCAL
//     private readonly string baseUrl = "https://localhost:7026/api/playerdata";
// #else
//     private readonly string baseUrl = "http://3.37.127.156:8080/api/playerdata";
// #endif

//     public void FetchMyData()
//     {
//         StartCoroutine(GetPlayerDataRoutine());
//     }
//     public void SaveMyData()
//     {
//         StartCoroutine(SavePlayerDataRoutine());
//     }
//     // 서버에서 처리하는 로직이 있는 경우 비동기 함수를 순차적으로 실행시켜 세션을 업데이트
//     public void SaveAndFetch(Action onComplete = null)
//     {
//         StartCoroutine(SaveAndFetchData(onComplete));
//     }
//     public IEnumerator SaveAndFetchData(Action onComplete) // 인자는 콜백용
//     {
//         yield return StartCoroutine(SavePlayerDataRoutine());
//         yield return StartCoroutine(GetPlayerDataRoutine());
//         onComplete?.Invoke(); // 콜백용으로만 동작
//     }

//     public void SendLog(EnhanceLogDto logData)
//     {
//         StartCoroutine(PostLogCoroutine(logData));
//     }

//     private IEnumerator GetPlayerDataRoutine()
//     {
//         string token = PlayerPrefs.GetString("AuthToken", "");

//         if (string.IsNullOrEmpty(token)) yield break; // 토큰이 없으면 중단


//         // GET request
//         using (UnityWebRequest request = UnityWebRequest.Get(baseUrl))
//         {
//             request.SetRequestHeader("Authorization", $"Bearer {token}");

//             // HTTPS 인증서 우회 ( 나중에 지움 )
//             request.certificateHandler = new BypassCertificate();

//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 // JSON to PlayerData
//                 string jsonResponse = request.downloadHandler.text;
//                 PlayerData pData = JsonUtility.FromJson<PlayerData>(jsonResponse);

//                 string itemsString = pData.items != null && pData.items.Count > 0 ? string.Join(", ", pData.items.Select(item => item.eid)) : "없음";
//                 Debug.Log($"데이터 로드, 닉네임: {pData.username}, 레벨: {pData.level}, 아이템 개수: {(pData.items != null ? pData.items.Count : 0)}개, [아이템 목록: {itemsString}]");

//                 PlayerSession.Instance.UpdateSessionData(pData);
//             }
//             else
//             {
//                 // 토큰이 만료되었거나 잘못된 경우 에러 메시지 출력  
//                 Debug.LogError($"데이터 로드 실패 ({request.responseCode}): {request.error}");
//             }
//         }
//      }

//     private IEnumerator SavePlayerDataRoutine()
//     {
//         PlayerData saveData = new PlayerData
//         {
//             id = PlayerSession.Instance.Id,
//             username = PlayerSession.Instance.Username,
//             level = PlayerSession.Instance.Level,
//             exp = PlayerSession.Instance.Exp,
//             currency = PlayerSession.Instance.Currency,
//             items = PlayerSession.Instance.PlayerItems
//         };

//         string jsonData = JsonUtility.ToJson(saveData);
//         string token = PlayerPrefs.GetString("AuthToken", "");
//         if (string.IsNullOrEmpty(token)) yield break; // 토큰이 없으면 중단

//         string updateUrl = baseUrl + "/update";

//         using (UnityWebRequest request = new UnityWebRequest(updateUrl, "POST"))
//         {
//             byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
//             request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//             request.downloadHandler = new DownloadHandlerBuffer();

//             // 헤더 설정 
//             request.SetRequestHeader("Content-Type", "application/json");
//             request.SetRequestHeader("Authorization", $"Bearer {token}");

//             // 요청
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("데이터 저장 완료: " + request.downloadHandler.text);
//             }
//             else
//             {
//                 Debug.LogError($"데이터 저장 실패 ({request.responseCode}): {request.error}");
//             }
//         }
//     }

//     private IEnumerator PostLogCoroutine(EnhanceLogDto data)
//     {
//         string json = JsonUtility.ToJson(data);
//         byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

//         // UnityWebRequest 생성
//         using (UnityWebRequest request = new UnityWebRequest(baseUrl, "POST"))
//         {
//             request.uploadHandler = new UploadHandlerRaw(jsonToSend);
//             request.downloadHandler = new DownloadHandlerBuffer();
//             request.SetRequestHeader("Content-Type", "application/json");

//             // 서버 응답 대기
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("강화 로그 서버 전송 성공");
//             }
//             else
//             {
//                 Debug.LogError($"강화 로그 전송 실패: {request.error}");
//             }
//         }
//     }
// }

#define LOCALn
using System;
using System.Collections;
using System.Text;
using System.Transactions;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class PlayerDataManager : MonoBehaviour
{
#if LOCAL
    private readonly string baseUrl = "https://localhost:7026/api/playerdata";
#else
    private readonly string baseUrl = "http://3.37.127.156:8080/api/playerdata";
#endif

    public void FetchMyData()
    {
        StartCoroutine(GetPlayerDataRoutine());
    }

    public void SaveMyData()
    {
        StartCoroutine(SavePlayerDataRoutine());
    }

    // 서버에서 처리하는 로직이 있는 경우 비동기 함수를 순차적으로 실행시켜 세션을 업데이트
    public void SaveAndFetch(Action onComplete = null)
    {
        StartCoroutine(SaveAndFetchData(onComplete));
    }

    public IEnumerator SaveAndFetchData(Action onComplete) // 인자는 콜백용
    {
        yield return StartCoroutine(SavePlayerDataRoutine());
        yield return StartCoroutine(GetPlayerDataRoutine());
        onComplete?.Invoke(); // 콜백용으로만 동작
    }

    public void SendLog(EnhanceLogDto logData)
    {
        StartCoroutine(PostLogCoroutine(logData));
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

            // ===== 추가 로그 =====
            Debug.Log("===== GET PLAYER DATA REQUEST =====");
            Debug.Log($"URL: {baseUrl}");
            Debug.Log($"TOKEN: {token}");

            yield return request.SendWebRequest();

            // ===== 추가 로그 =====
            Debug.Log("===== GET PLAYER DATA RESPONSE =====");
            Debug.Log($"STATUS CODE: {request.responseCode}");
            Debug.Log($"RESPONSE BODY: {request.downloadHandler.text}");

            if (request.result == UnityWebRequest.Result.Success)
            {
                // JSON to PlayerData
                string jsonResponse = request.downloadHandler.text;
                PlayerData pData = JsonUtility.FromJson<PlayerData>(jsonResponse);

                string itemsString = pData.items != null && pData.items.Count > 0
                    ? string.Join(", ", pData.items.Select(item => item.eid))
                    : "없음";

                Debug.Log($"데이터 로드, 닉네임: {pData.username}, 레벨: {pData.level}, 아이템 개수: {(pData.items != null ? pData.items.Count : 0)}개, [아이템 목록: {itemsString}]");

                PlayerSession.Instance.UpdateSessionData(pData);
            }
            else
            {
                // 토큰이 만료되었거나 잘못된 경우 에러 메시지 출력
                Debug.LogError($"데이터 로드 실패 ({request.responseCode}): {request.error}");

                // ===== 추가 로그 =====
                Debug.LogError($"SERVER RESPONSE: {request.downloadHandler.text}");
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
            items = PlayerSession.Instance.PlayerItems
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

            // ===== 추가 로그 =====
            Debug.Log("===== SAVE PLAYER DATA REQUEST =====");
            Debug.Log($"URL: {updateUrl}");
            Debug.Log($"TOKEN: {token}");
            Debug.Log($"JSON BODY: {jsonData}");

            // 요청
            yield return request.SendWebRequest();

            // ===== 추가 로그 =====
            Debug.Log("===== SAVE PLAYER DATA RESPONSE =====");
            Debug.Log($"STATUS CODE: {request.responseCode}");
            Debug.Log($"RESPONSE BODY: {request.downloadHandler.text}");

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("데이터 저장 완료: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"데이터 저장 실패 ({request.responseCode}): {request.error}");

                // ===== 추가 로그 =====
                Debug.LogError($"SERVER RESPONSE: {request.downloadHandler.text}");
            }
        }
    }

    private IEnumerator PostLogCoroutine(EnhanceLogDto data)
    {
        string json = JsonUtility.ToJson(data);
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

        // UnityWebRequest 생성
        using (UnityWebRequest request = new UnityWebRequest(baseUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // ===== 추가 로그 =====
            Debug.Log("===== ENHANCE LOG REQUEST =====");
            Debug.Log($"URL: {baseUrl}");
            Debug.Log($"JSON BODY: {json}");

            // 서버 응답 대기
            yield return request.SendWebRequest();

            // ===== 추가 로그 =====
            Debug.Log("===== ENHANCE LOG RESPONSE =====");
            Debug.Log($"STATUS CODE: {request.responseCode}");
            Debug.Log($"RESPONSE BODY: {request.downloadHandler.text}");

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("강화 로그 서버 전송 성공");
            }
            else
            {
                Debug.LogError($"강화 로그 전송 실패: {request.error}");

                // ===== 추가 로그 =====
                Debug.LogError($"SERVER RESPONSE: {request.downloadHandler.text}");
            }
        }
    }
}