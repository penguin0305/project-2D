using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOverManager : NetworkBehaviour
{
        // 싱글톤 설정
        public static GameOverManager Instance;
        [SerializeField] private string lobbySceneName = "NetworkLobby";

        public NetworkVariable<int> PlayersReadyToExit = new NetworkVariable<int>(0);
        private HashSet<ulong> confirmedExitPlayers = new HashSet<ulong>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        // --- 포탈이나 플레이어 사망 시 호출할 서버 함수 ---
        public void NotifyAllPlayersClearServer()
        {
            if (!IsServer) return;
            // 모든 클라이언트에게 결과창을 띄우라고 명령
            ShowResultUIClientRpc();
        }

        [ClientRpc]
        private void ShowResultUIClientRpc()
        {
            // 씬에 있는 ResultUIManager를 찾아 결과창을 켬
            // (ResultUIManager가 싱글톤이라면 ResultUIManager.Instance.ShowResultUI() 사용)
            Object.FindAnyObjectByType<ResultUIManager>()?.ShowResultUI();
        }

        // --- UI의 Exit 버튼이 호출할 함수 ---
        public void RequestExit()
        {
            SubmitExitRequestServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SubmitExitRequestServerRpc(ulong clientId)
        {
            if (confirmedExitPlayers.Contains(clientId)) return;

            confirmedExitPlayers.Add(clientId);
            PlayersReadyToExit.Value = confirmedExitPlayers.Count;

            int total = NetworkManager.Singleton.ConnectedClients.Count;
            if (PlayersReadyToExit.Value >= total && total > 0)
            {
                // 모든 플레이어가 나가기 버튼을 눌렀다면 최종 이동
                GoToLobbyClientRpc();
            }
        }

        [ClientRpc]
        private void GoToLobbyClientRpc()
        {
            StartCoroutine(LeaveSequence());
        }

        private IEnumerator LeaveSequence()
        {
            bool isHost = IsServer;

            // 데이터 보존 처리
            if (NetworkInventoryManager.Instance != null)
                NetworkInventoryManager.Instance.DontSendInventoryToSession();

            if (NetworkHistoryManager.Instance != null)
                NetworkHistoryManager.Instance.ResetData();

            // 네트워크 종료
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.Shutdown();
                
                // 호스트라면 객체 파괴
                if (isHost && NetworkManager.Singleton.gameObject != null)
                {
                    Destroy(NetworkManager.Singleton.gameObject);
                }
            }

            yield return null; // 한 프레임 대기

            // 로비 씬으로 이동
            SceneManager.LoadScene(lobbySceneName);
        }
}
/*~0514 분
public class GameOverManager : NetworkBehaviour
{
    public static GameOverManager Instance;
    [SerializeField] private string lobbySceneName = "NetworkLobby";

    public NetworkVariable<int> PlayersReadyToExit = new NetworkVariable<int>(0);
    private HashSet<ulong> confirmedExitPlayers = new HashSet<ulong>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RequestExit()
    {
        SubmitExitRequestServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitExitRequestServerRpc(ulong clientId)
    {
        if (confirmedExitPlayers.Contains(clientId)) return;

        confirmedExitPlayers.Add(clientId);
        PlayersReadyToExit.Value = confirmedExitPlayers.Count;

        int total = NetworkManager.Singleton.ConnectedClients.Count;
        if (PlayersReadyToExit.Value >= total && total > 0)
        {
            GoToLobbyClientRpc();
        }
    }

    [ClientRpc]
    private void GoToLobbyClientRpc()
    {
        if (NetworkInventoryManager.Instance != null)
            NetworkInventoryManager.Instance.DontSendInventoryToSession();

        if (NetworkHistoryManager.Instance != null)
            NetworkHistoryManager.Instance.ResetData();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        if (IsServer && NetworkManager.Singleton.gameObject != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
        }
        SceneManager.LoadScene(lobbySceneName);
    }
}
*/