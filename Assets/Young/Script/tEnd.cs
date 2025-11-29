using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class tGameClearManager : MonoBehaviour
{

    public string endSceneName = "EndScene";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("end ½ÇÇà");

            Time.timeScale = 1f;

            SceneManager.LoadScene(endSceneName);
        }
    }
}