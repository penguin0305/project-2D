using UnityEngine;
using TMPro;
using System.Collections;

public class NetworkPlayerUI : MonoBehaviour
{
    public static NetworkPlayerUI Instance;

    public TextMeshProUGUI HP;
    //public TextMeshProUGUI skillText;-스킬 추가

    //private PlayerSkill playerSkill;-스킬 추가

    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
        if (timerText != null && NetworkHistoryManager.Instance != null)
        {
            UpdateTimerUI(NetworkHistoryManager.Instance.playTime.Value);
        }
    }
    private void UpdateTimerUI(float Seconds)
    {

        int minutes = Mathf.FloorToInt(Seconds / 60);
        int seconds = Mathf.FloorToInt(Seconds % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void UpdateHP(int currentHp)
    {
        HP.text = $"{currentHp}";
    }
    /*-스킬 추가
    void UpdateSkillUI(int currentSkill)
    {

        skillText.text = $"{currentSkill}";
    }
    */
}