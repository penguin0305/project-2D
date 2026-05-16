using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ResultUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverUI;

    [Header("Score UI (History Data)")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI defeatCountText;
    [SerializeField] private TextMeshProUGUI hpText;
    public static ResultUIManager Instance;
    
    private void Awake() 
    { 
        Instance = this; 
    }
    
    private void Start()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    public void ShowResultUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        var data = NetworkHistoryManager.Instance.mySessionData;

        int calculatedScore = (data.tempcoinCount * 100) + (data.DefeatCount * 500) + (data.currentHP * 1000);

        if (scoreText != null) scoreText.SetText(calculatedScore.ToString()); 
        if (coinText != null) coinText.SetText(data.tempcoinCount.ToString());
        if (defeatCountText != null) defeatCountText.SetText(data.DefeatCount.ToString());
        if (hpText != null) hpText.SetText(data.currentHP.ToString());
    }

    public void OnExitBtnClick()
    {
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.RequestExit();
        }
    }
}