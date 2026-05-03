using UnityEngine;

[CreateAssetMenu(fileName = "itemData", menuName = "Scriptable Objects/itemData")]
public class itemData : ScriptableObject
{
    public enum itemType
    {
        potion,
        coin,
        key,
        quest,
        collection,
        equipment

    }
    [SerializeField] public int itemID;
   
    [SerializeField] public bool canOverlap;
   
    [SerializeField] public int itemQuantity;
 
    [SerializeField] public bool isInteractivity;

    [SerializeField] public bool isConsumable;

    [SerializeField] public float itemCooltime = -1;

    [SerializeField] public itemType mItemType;

    [SerializeField] public Sprite mItemImage;

    public CollectionData collectionInfo;
}
