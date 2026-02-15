using UnityEngine;

[CreateAssetMenu(fileName = "itemData", menuName = "Scriptable Objects/itemData")]
public class itemData : ScriptableObject
{
    public enum itemType //아이템 타입 필요시 추가
    {
        potion,
        coin,
        key,
        quest,
        equip
    }
    [Header("고유한 아이템의 ID(중복불가)")]
    [SerializeField] public int itemID;
   
    [Header("아이템의 중첩이 가능한가?")]
    [SerializeField] public bool canOverlap;
   
    [Header("아이템의 개수")]
    [SerializeField] public int itemQuantity;
 
    [Header("사용(상호작용)이 가능한 아이템인가?")]
    [SerializeField] public bool isInteractivity;

    [Header("아이템을 사용하면 사라지는가?")]
    [SerializeField] public bool isConsumable;

    [Header("아이템을 사용시 쿨타임")]
    [SerializeField] public float itemCooltime = -1;

    [Header("아이템의 타입")]
    [SerializeField] public itemType mItemType;

    [Header("인벤토리에서 보여질 아이템의 이미지")]
    [SerializeField] public Sprite mItemImage;

}
