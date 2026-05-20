using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneExit : MonoBehaviour
{
    public static SceneExit Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async void ShutdownScene(string sceneName, bool isHost)
    {
        await Task.Delay(500);

        if (NetworkInventoryManager.Instance != null)
            NetworkInventoryManager.Instance.DontSendInventoryToSession();

        if (NetworkHistoryManager.Instance != null)
            NetworkHistoryManager.Instance.ResetData();

        if (ProjectSpellGameLobby.Singleton != null)
        {
            if (isHost)
            {
                await ProjectSpellGameLobby.Singleton.DeleteLobby();
            }
            else
            {
                await ProjectSpellGameLobby.Singleton.LeaveLobby();
            }

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