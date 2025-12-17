using UnityEngine;
using TMPro;
public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float TimeLapse;

    void Start()
    {
        if (timerText != null)
        {
            timerText.text = "00:00";
        }

        TimeLapse = 0f;
    }

    void Update()
    {
        if (HistoryManager.Instance != null)
        {
            UpdateTimerUI(HistoryManager.Instance.playTime);
        }
    }

    private void UpdateTimerUI(float Seconds)
    {

        int minutes = Mathf.FloorToInt(Seconds / 60);
        int seconds = Mathf.FloorToInt(Seconds % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}