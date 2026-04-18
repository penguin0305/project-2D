using UnityEngine;
using Unity.Netcode;

public class NewStartMenuController : MonoBehaviour
{
    public static NewStartMenuController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnStartButtonClicked()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            NetworkManager.Singleton.SceneManager.LoadScene("NetworkScenario", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}