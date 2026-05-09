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

    /*
    [Header("Exit Status UI (Stacking)")]
    [SerializeField] private TextMeshProUGUI currentPlayersText;
    [SerializeField] private TextMeshProUGUI totalPlayersText;
    */
    public static ResultUIManager Instance;
    private void Awake() 
    { 
        Instance = this; 
    }
    
    private void Start()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        /*
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.PlayersReadyToExit.OnValueChanged += (prev, next) => UpdateExitStatusUI();
        }
        */
    }

    public void ShowResultUI()
    {
            gameOverUI.SetActive(true);
            UpdateScoreUI();
            /*
            UpdateExitStatusUI();
            */
    }

    private void UpdateScoreUI()
    {
        var data = NetworkHistoryManager.Instance.mySessionData;
        scoreText.SetText(data.coinCount.ToString()); 
        coinText.SetText(data.tempcoinCount.ToString());
        defeatCountText.SetText(data.DefeatCount.ToString());
    }

/*
    private void UpdateExitStatusUI()
    {
        if (GameOverManager.Instance == null) return;

        currentPlayersText.SetText(GameOverManager.Instance.PlayersReadyToExit.Value.ToString());

	int total = NetworkManager.Singleton.ConnectedClients.Count;
        totalPlayersText.SetText(total.ToString());
    }
*/
    public void OnExitBtnClick()
    {
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.RequestExit();
        }
    }

    private void OnDestroy()
    {
        /*
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.PlayersReadyToExit.OnValueChanged -= (prev, next) => UpdateExitStatusUI();
        }
        */
    }
}