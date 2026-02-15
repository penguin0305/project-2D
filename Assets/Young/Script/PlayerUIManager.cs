using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI HP;
    //public TextMeshProUGUI skillText;-스킬 추가

    private PlayerStats playerstats;
    //private PlayerSkill playerSkill;-스킬 추가

    [SerializeField] private TextMeshProUGUI timerText;

    IEnumerator Start()
    {
        // playerStats를 찾을 때까지 대기
        while (playerstats == null)
        {
            playerstats = FindAnyObjectByType<PlayerStats>();
            //playerSkill = FindObjectOfType<PlayerSkill>();-스킬 추가
            if (playerstats == null)
            {
                yield return null;
            }
        }

        playerstats.OnCheckHP += UpdateHP;
        //playerSkill.OnCheckSkillCount += UpdateSkillUI;-스킬 추가
        UpdateHP(playerstats.CurrentHP);

        if (timerText != null)
        {
            timerText.text = "00:00";
        }
    }

    void Update()
    {
        UpdateTimer();
    }
    void UpdateHP(int currentHp)
    {
        HP.text = $"{currentHp}";
    }
    /*-스킬 추가
    void UpdateSkillUI(int currentSkill)
    {

        skillText.text = $"{currentSkill}";
    }
    */


    void UpdateTimer()
    {
        if (timerText == null) return;

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

    void OnDestroy()
    {
        if (playerstats != null)
        {
            playerstats.OnCheckHP -= UpdateHP;
        }
        /*-스킬 추가
        if (playerSkill != null)
        {
            playerSkill.OnCheckSkillCount -= UpdateSkillUI;
        }
        */
    }
}