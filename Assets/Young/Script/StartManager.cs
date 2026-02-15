using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject playerPrefab;
    public static StartMenuController Instance;


    /// 재시작용 캐릭터 프리팹 정보 저장
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

        if (HistoryManager.Instance != null)
        {
            HistoryManager.Instance.ResetData();
        }
            GameSceneManager.Instance.SetPlayerPrefab(Player);
        }


        SceneManager.LoadScene("tScene");
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