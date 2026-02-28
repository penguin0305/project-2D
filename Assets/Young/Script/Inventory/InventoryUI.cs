using System.Collections.Generic;
using UnityEngine;
public class InventoryUI : MonoBehaviour
{
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

        if (InventoryManager.Instance == null) return;
        if (InventoryManager.Instance.tmpInventory == null) return;

        for (int i = 0; i < slot.Length; i++)
        {
            slot[i].DeleteSlot();
        }

        int slotIndex = 0;

        foreach ((int id, int amount) in InventoryManager.Instance.tmpInventory)
        {
            itemData data = ItemList.Find(x => x.itemID == id);

            if (data != null)
            {
                slot[slotIndex].SetItem(data, amount);
                slotIndex++;
            }
        }
    }
}