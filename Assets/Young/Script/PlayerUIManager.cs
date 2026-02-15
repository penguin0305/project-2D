using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI HP;
    //public TextMeshProUGUI skillText;

    private Player player;
    //private PlayerSkill playerSkill;

    IEnumerator Start()
    {
        // playerStats�� ã�� ������ ���� ��� (0.1�� ����)
        while (player == null)
        {
            player = FindAnyObjectByType<Player>();
            //playerSkill = FindObjectOfType<PlayerSkill>();
            if (player == null)
            {
                yield return null;
            }
        }

        player.OnCheckHP += UpdateHP;
        //playerSkill.OnCheckSkillCount += UpdateSkillUI;
        UpdateHP(player.Status.CurrentHealth);
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
        if (player != null)
        {
            player.OnCheckHP -= UpdateHP;
        }
        /*
        if (playerSkill != null)
        {
            playerSkill.OnCheckSkillCount -= UpdateSkillUI;
        }
        */
    }
}