using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject Player;

    public void OnStartButtonClicked()
    {
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.SetPlayerPrefab(Player);
        }


        SceneManager.LoadScene("tScene");
    }
}