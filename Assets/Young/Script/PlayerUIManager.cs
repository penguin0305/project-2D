using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI HP;
    //public TextMeshProUGUI skillText;스킬기능

    private Player player;
    //private PlayerSkill playerSkill;스킬기능

    [SerializeField] private TextMeshProUGUI timerText;

    IEnumerator Start()
    {
        while (player == null)
        {
            player = FindAnyObjectByType<Player>();
            //playerSkill = FindObjectOfType<PlayerSkill>();스킬기능
            if (player == null)
            {
                yield return null;
            }
        }

        player.OnCheckHP += UpdateHP;
        //playerSkill.OnCheckSkillCount += UpdateSkillUI;스킬기능
        UpdateHP(player.Status.CurrentHealth);

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
    /*스킬기능
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
        if (player != null)
        {
            player.OnCheckHP -= UpdateHP;
        }
        /*스킬기능
        if (playerSkill != null)
        {
            playerSkill.OnCheckSkillCount -= UpdateSkillUI;
        }
        */
    }
}