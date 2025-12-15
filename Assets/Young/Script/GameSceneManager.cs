using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public GameObject playerPrefab;

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
}