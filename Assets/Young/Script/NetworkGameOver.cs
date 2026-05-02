using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameOverManager : NetworkBehaviour
{
    public static GameOverManager Instance;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private string lobbySceneName = "NetworkLobby";

    private int currentDeadPlayers = 0;

    private void Awake()
    {
        if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReportPlayerDeathServerRpc()
    {
        if (!IsServer) return;

        currentDeadPlayers++;
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        int totalPlayers = NetworkManager.Singleton.ConnectedClients.Count;

        if (currentDeadPlayers >= totalPlayers && totalPlayers > 0)
        {
            ShowGameOverUIClientRpc();
        }
    }

    [ClientRpc]
    private void ShowGameOverUIClientRpc()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void OnExitToLobbyButtonClicked()
    {
        if (NetworkHistoryManager.Instance != null)
        {
            NetworkHistoryManager.Instance.ResetData();
        }

        if (NetworkInventoryManager.Instance != null)
        {
            NetworkInventoryManager.Instance.DontSendInventoryToSession();
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        SceneManager.LoadScene(lobbySceneName);
    }

    public void ResetDeathCount()
    {
        if (IsServer)
        {
            currentDeadPlayers = 0;
        }
    }
}