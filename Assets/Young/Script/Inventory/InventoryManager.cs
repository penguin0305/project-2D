using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<int, int> tmpInventory = new Dictionary<int, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void AddItem(itemData item, int amount = 1)
    {
        if (tmpInventory.ContainsKey(item.itemID))
        {
            tmpInventory[item.itemID] += amount;
        }
        else
        {
            tmpInventory.Add(item.itemID, amount);
        }

        Debug.Log($"{item.mItemType} È¹µæ");

        if (HistoryManager.Instance != null)
        {
            if (item.itemID == 1001)
            {
                HistoryManager.Instance.AddcoinCount();
            }
        }
    }
}
    /* Æ¯Á¤ »óÈ£ÀÛ¿ë °¹¼ö È®ÀÎ¿ë
    public int GetItemCount(int id)
    {
        if (tmpInventory.ContainsKey(id))
        {
            return tmpInventory[id];
        }
        return 0;
    }

    */