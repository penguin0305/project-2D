using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLobbyManager : MonoBehaviour
{
    public string gameSceneName = "NetworkMainScene";

    public void OnClickStartHost()
    {
        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    public void OnClickStartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}