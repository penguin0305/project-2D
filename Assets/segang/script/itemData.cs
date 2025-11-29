using UnityEngine;

[CreateAssetMenu(fileName = "itemData", menuName = "Scriptable Objects/itemData")]
public class itemData : ScriptableObject
{
    public enum itemType //아이템 타입 필요시 추가
    {
        potion,
        coin,
        key,
        quest
    }
    [Header("고유한 아이템의 ID(중복불가)")]
    [SerializeField] private int mItemID;
    public int itemID
    {
        get
        {
            return mItemID;
        }
    }
    [Header("아이템의 중첩이 가능한가?")]
    [SerializeField] private bool mCanOverlap;
    public bool canOverlap
    {
        get
        {
            return mCanOverlap;
        }
    }
    [Header("아이템의 개수")]
    [SerializeField] private int mItemQuantity;
    public int itemQuantity
    {
        get
        {
            return mItemQuantity;
        }
    }
    [Header("사용(상호작용)이 가능한 아이템인가?")]
    [SerializeField] private bool mIsInteractivity;
    public bool isInteractivity
    {
        get
        {
            return mIsInteractivity;
        }
    }

    [Header("아이템을 사용하면 사라지는가?")]
    [SerializeField] private bool mIsConsumable;
    public bool isConsumable
    {
        get
        {
            return mIsConsumable;
        }
    }

    [Header("아이템을 사용시 쿨타임")]
    [SerializeField] private float mItemCooltime = -1;
    public float cooltime
    {
        get
        {
            return mItemCooltime;
        }
    }

    [Header("아이템의 타입")]
    [SerializeField] private itemType mItemType;
    public itemType Type
    {
        get
        {
            return mItemType;
        }
    }

    [Header("인벤토리에서 보여질 아이템의 이미지")]
    [SerializeField] private Sprite mItemImage;
    public Sprite Image
    {
        get
        {
            return mItemImage;
        }
    }
}
