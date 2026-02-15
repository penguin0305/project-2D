using UnityEngine;

public enum EquipCategory { Weapon, Helmet, Armor, Gloves, Shoes, Bracelet, Necklace, Ring }

[CreateAssetMenu(fileName = "NewEquipData", menuName = "Equipment/EquipData")]
public class EquipData : itemData
{
    public EquipCategory category;
    public int level;

    public int bonusATK;
    public int bonusHP;
    public float bonusDEF;

    public float enhanceATK;
    public float enhanceHP;
    public float enhanceDEF;

    private int finalATK;
    private int finalHP;
    private float finalDEF;

    //로직 나중에 구현
    public (int atk, int hp, float def) CalcStats(int level) {
        return (bonusATK, bonusHP, bonusDEF); // 현재는 기본 능력치만 리턴
    }
}
