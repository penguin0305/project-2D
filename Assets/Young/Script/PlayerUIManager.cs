using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI HP;
    //public TextMeshProUGUI skillText;

    private PlayerStats playerStats;
    //private PlayerSkill playerSkill;

    IEnumerator Start()
    {
        // playerStats를 찾을 때까지 무한 대기 (0.1초 간격)
        while (playerStats == null)
        {
            playerStats = FindAnyObjectByType<PlayerStats>();
            //playerSkill = FindObjectOfType<PlayerSkill>();
            if (playerStats == null)
            {
                yield return null;
            }
        }

        playerStats.OnCheckHP += UpdateHP;
        //playerSkill.OnCheckSkillCount += UpdateSkillUI;
        UpdateHP(playerStats.CurrentHP);
    }
    void UpdateHP(int currentHp)
    {
            HP.text = $"{currentHp}";
    }

    void UpdateSkillUI(int currentSkill)
    {

            //skillText.text = $"{currentSkill}";
    }

    void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnCheckHP -= UpdateHP;
        }
        /*
        if (playerSkill != null)
        {
            playerSkill.OnCheckSkillCount -= UpdateSkillUI;
        }
        */
    }
}