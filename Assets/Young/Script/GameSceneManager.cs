using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public GameObject playerPrefab;

    public void SetPlayerPrefab(GameObject prefab)
    {
        playerPrefab = prefab;
    }
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
}