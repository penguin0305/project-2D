using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<int, int> tmpInventory = new Dictionary<int, int>();

    public List<int> itemOrder = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void AddItem(itemData item)
    {
        if (item.mItemType == itemData.itemType.collection)
        {
            if (tmpInventory.ContainsKey(item.collectionInfo.collectionId))
            {
                tmpInventory[item.collectionInfo.collectionId] += 1;
            }
            else
            {
                tmpInventory.Add(item.collectionInfo.collectionId, 1);
                itemOrder.Add(item.collectionInfo.collectionId);
            }
        }
        else
        {
            if (tmpInventory.ContainsKey(item.itemID))
            {
                tmpInventory[item.itemID] += 1;
            }
            else
            {
                tmpInventory.Add(item.itemID, 1);
                itemOrder.Add(item.itemID);
            }
        }

        Debug.Log($"{item.mItemType} ȹ��");

        if (HistoryManager.Instance != null)
        {
            if (item.itemID == 1001)
            {
                HistoryManager.Instance.AddcoinCount();
            }
        }
    }

    public void RemoveItem(int id)
    {
        if (tmpInventory.ContainsKey(id))
        {
            tmpInventory[id] -= 1;

            if (tmpInventory[id] <= 0)
            {
                tmpInventory.Remove(id);
                itemOrder.Remove(id);
            }
        }
    }

    /* Ư�� ��ȣ�ۿ� ���� Ȯ�ο�
public int GetItemCount(int id)
{
    if (tmpInventory.ContainsKey(id))
    {
        return tmpInventory[id];
    }
    return 0;
}

*/
}

