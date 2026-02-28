using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipStats
{
    public int sBonusATK { get; private set; }
    public int sBonusHP { get; private set; }
    public float sBonusDEF { get; private set; }

    private readonly Dictionary<EquipCategory, EquipData> equipData = new();
    private Dictionary<EquipCategory, int> equipLevels = new();

    public void Initialize(EquipCategory category, EquipData data, int currentLevel)
    {
        equipData[category] = data;
        equipLevels[category] = currentLevel;
        RecalculateStats();
    }

    public void InitializeBaseEquipment(List<EquipData> allEquips)
    {
        if (allEquips == null) return;

        foreach (var data in allEquips)
        {
            Initialize(data.category, data, data.level);
        }
    }

    public void Remove(EquipCategory category)
    {
        if (category == EquipCategory.Weapon)
        {
            equipData.Remove(category);
            equipLevels.Remove(category);
            RecalculateStats();
        }
    }

    public void Upgrade() { }

    private void RecalculateStats()
    {
        sBonusATK = 0; sBonusHP = 0; sBonusDEF = 0f;

        foreach (var category in equipData.Keys)
        {
            var data = equipData[category];
            var level = equipLevels[category];
            var (atk, hp, def) = data.CalcStats(level);

            sBonusATK += atk;
            sBonusHP += hp;
            sBonusDEF += def;
        }
    }
}
