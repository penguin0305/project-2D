using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler
{
    private EquipStats _equipStats;
    public EquipCategory eCategory = EquipCategory.Weapon;
    public Image icon;

    private WeaponData _weapon;
    [SerializeField] private WeaponData initialWeapon; // UI테스트용

    [Inject]
    public void Construct(EquipStats equipStats)
    {
        _equipStats = equipStats;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isRightClick = eventData.button == PointerEventData.InputButton.Right;
        bool isDoubleLeftClick = eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2;

        if (isRightClick || isDoubleLeftClick)
        {
            Unequip();
        }
    }

    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        _weapon = weapon;
        _equipStats.Initialize(eCategory, _weapon, _weapon.level);

        if (icon != null)
        {
            icon.sprite = _weapon.mItemImage;
            icon.enabled = true;
        }
    }

    public void Unequip()
    {
        if (_weapon == null) return;

        _equipStats.Remove(eCategory);

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }


        _weapon = null;
    }

    /*
    public void SwapWeapon(WeaponData newWeapon)
    {
        if (newWeapon == null) return;

        _equipStats.Initialize(EquipCategory.Weapon, newWeapon, newWeapon.level);
    }
    */


    //임시 테스트용
    private void Start()
    {
        if (initialWeapon != null)
        {
            EquipWeapon(initialWeapon);
        }
    }
}