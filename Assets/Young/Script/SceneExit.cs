using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour
{
    public static SceneExit Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ShutdownScene(string sceneName, bool isHost)
    {
        if (NetworkInventoryManager.Instance != null)
            NetworkInventoryManager.Instance.DontSendInventoryToSession();

        if (NetworkHistoryManager.Instance != null)
            NetworkHistoryManager.Instance.ResetData();

        if (ProjectSpellGameLobby.Singleton != null)
        {
            if (isHost) ProjectSpellGameLobby.Singleton.DeleteLobby();
            else ProjectSpellGameLobby.Singleton.LeaveLobby();
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
            
            if (isHost && NetworkManager.Singleton.gameObject != null)
            {
                Destroy(NetworkManager.Singleton.gameObject);
            }
        }

        SceneManager.LoadScene(sceneName);
    }
}