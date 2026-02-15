using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("slot")]
    public Transform slotPanel;
    public GameObject slot;

    [Header("ItemList")]
    public List<itemData> ItemList;

    private void OnEnable()
    {
        CreateInventory();
    }

    public void CreateInventory()
    {
        foreach (Transform child in slotPanel)
        {
            Destroy(child.gameObject);
        }

        if (InventoryManager.Instance == null) return;

        foreach ((int id, int count) in InventoryManager.Instance.tmpInventory)
        {
            itemData data = ItemList.Find(x => x.itemID == id);

            GameObject newSlot = Instantiate(slot, slotPanel);

            newSlot.GetComponent<InventorySlot>().SetItem(data.mItemImage, count);
        }
    }
}