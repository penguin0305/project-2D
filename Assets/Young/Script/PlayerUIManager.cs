using UnityEngine;
using TMPro; // TextMeshPro ÇÊ¼ö

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI HP;
    //public TextMeshProUGUI skillText;

    private PlayerStats playerStats;
    //private PlayerSkill playerSkill;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        //playerSkill = FindObjectOfType<PlayerSkill>();

        playerStats.OnCheckHP += UpdateHP;
        //playerSkill.OnCheckSkillCount += UpdateSkillUI;
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