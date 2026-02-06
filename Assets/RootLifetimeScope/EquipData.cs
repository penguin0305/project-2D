using UnityEngine;

public enum EquipCategory { Weapon, Helmet, Armor, Gloves, Shoes, Bracelet, Necklace, Ring }

public class EquipData : itemData
{
    public EquipCategory eSlot;
    //юс╫ц ╫╨ех
    public int BonusATK;
    public int BonusDEF;
    public int BonusHP;
}
