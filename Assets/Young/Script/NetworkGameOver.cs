using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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

    public void NotifyAllPlayersClearServer()
    {
        if (!IsServer) return;
        ShowResultUIClientRpc();
    }

    [ClientRpc]
    private void ShowResultUIClientRpc()
    {
        Object.FindAnyObjectByType<ResultUIManager>()?.ShowResultUI();
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
        bool isHost = IsServer; 

        if (SceneExit.Instance != null)
        {
            SceneExit.Instance.ShutdownScene(lobbySceneName, isHost);
        }
        else
        {
            NetworkManager.Singleton.Shutdown();
            UnityEngine.SceneManagement.SceneManager.LoadScene(lobbySceneName);
        }
    }
}