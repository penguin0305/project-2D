using System.Collections.Generic;
using UnityEngine;
public class InventoryUI : MonoBehaviour
{

    public static InventoryUI Instance;

    [Header("slot")]
    public Transform slotPanel;
    public InventorySlot[] slot;

    [Header("ItemList")]
    public List<itemData> ItemList;

    private void OnEnable()
    {
        slot = slotPanel.GetComponentsInChildren<InventorySlot>();
        UpdateInventory();
    }
    public void UpdateInventory()
    {

        if (NetworkInventoryManager.Instance == null) return;
        if (NetworkInventoryManager.Instance.tmpInventory == null) return;

        for (int i = 0; i < slot.Length; i++)
        {
            slot[i].DeleteSlot();
        }

        int slotIndex = 0;

        foreach (var item in NetworkInventoryManager.Instance.tmpInventory)
        {
            int id = item.Key;
            int amount = item.Value;

            itemData data = ItemList.Find(x => x.collectionInfo.collectionId == id);

            if (data != null && slotIndex < slot.Length)
            {
                slot[slotIndex].SetItem(data, amount);
                slotIndex++;
            }
        }
    }
}