using UnityEngine;
using UnityEditor;
using SupanthaPaul;
using UnityEngine.SceneManagement;
public class NetworkPauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    [SerializeField] private AudioSource BGM;
    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        isPaused = false;

        Debug.Log("게임 재개");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        isPaused = true;

        Debug.Log("게임 일시 정지");
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