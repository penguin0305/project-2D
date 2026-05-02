using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject playerPrefab;
    public static StartMenuController Instance;
    public string loadSceneName = "NetworkLobby";
    /// ����ۿ� ĳ���� ������ ���� ����
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

    public void SetPlayerPrefab(GameObject prefab)
    {
        playerPrefab = prefab;
    }

    public void OnStartButtonClicked()
    {
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.SetPlayerPrefab(playerPrefab);
        }

        SceneManager.LoadScene(loadSceneName);
    }




    public void OnQuitButtonClicked()
    {
        Debug.Log("Game End");

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}