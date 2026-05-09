using UnityEngine;
using TMPro;
using System.Collections;

public class ResultUIManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI DefeatText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI totalScoreText;

    public int WeightsTime = 10;
    public int WeightsDefeat = 1000;
    public int WeightsHP = 100;
    public int WeightsCoin = 500;

    void Start()
    {

        float time = HistoryManager.Instance.playTime;
        int Defeat = HistoryManager.Instance.DefeatCount;
        int hp = HistoryManager.Instance.currentHP;
        int coin = HistoryManager.Instance.coinCount;
        int timeLimit = 300;

        int timeScore = Mathf.Max(0, (timeLimit - (int)time) * WeightsTime);
        int DefeatScore = Defeat * WeightsDefeat;
        int hpScore = hp * WeightsHP;
        int coinScore = coin * WeightsCoin;


        int totalScore = timeScore + DefeatScore + hpScore + coinScore;

        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        timeText.text = $"{minutes}:{seconds}";

        DefeatText.text = $"{Defeat,6}";
        hpText.text = $"{hp,6}";
        coinText.text = $"{coin,6}";

        totalScoreText.text = $"{totalScore,6}";

        if (NetworkInventoryManager.Instance != null)
        {
            NetworkInventoryManager.Instance.SendInventoryToSession(totalScore);
        }
    }
}
