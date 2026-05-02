using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public string LobbySceneName = "NetworkLobby";

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

        SceneManager.LoadScene(LobbySceneName);
    }
}