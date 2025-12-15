using UnityEngine;
using System;

public class PlayerSkill : MonoBehaviour
{
    public event Action<int> OnCheckSkillCount;

    [SerializeField] private int maxSkillCount = 3;
    private int currentSkillCount;

    private void Start()
    {
        currentSkillCount = maxSkillCount;
        OnCheckSkillCount.Invoke(currentSkillCount);
    }

    public void UseSkill()
    {
        if (currentSkillCount > 0)
        {
            currentSkillCount--;
            Debug.Log("Use Skill");

            OnCheckSkillCount.Invoke(currentSkillCount);
        }
        else
        {
            Debug.Log("No SkillCount");
        }
    }
    /*  스킬횟수 추가
    public void AddSkillCount(int amount)
    {
    }
    */
}