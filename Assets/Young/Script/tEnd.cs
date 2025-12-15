using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using NUnit.Framework;

public class tGameClearManager : MonoBehaviour
{

    public string endSceneName = "EndScene";
    public StageManager stageManager;

    private void OnEnable()
    {
        stageManager.OnStageClear += StageClear;
    }

    private void StageClear(List<itemData> tmpinventory)
    {
        Debug.Log("end 실행");

        Time.timeScale = 1f;

        SceneManager.LoadScene(endSceneName);

        // 인자를 글로벌 인벤토리에 추가
    }

    private void OnDisable()
    {
        stageManager.OnStageClear -= StageClear;
    }
}