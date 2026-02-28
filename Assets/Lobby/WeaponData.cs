using NUnit.Framework.Internal.Execution;
using UnityEngine;

public enum WeaponType { Sword, Bow }

[CreateAssetMenu(fileName = "NewEquipData", menuName = "Equipment/WeaponData")]
public class WeaponData : EquipData
{
    [SerializeField] private static EquipCategory _category = EquipCategory.Weapon;
    public WeaponType weaponType;
}
