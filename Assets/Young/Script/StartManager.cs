using UnityEditor;
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


        SceneManager.LoadScene("Lobby");
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