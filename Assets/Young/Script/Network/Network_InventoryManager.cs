using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkInventoryManager : NetworkBehaviour

{
    public static NetworkInventoryManager Instance;
    public Dictionary<int, int> tmpInventory = new Dictionary<int, int>();
    public List<int> itemOrder = new List<int>();


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddItem(itemData item)
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

        Debug.Log($"{item.mItemType} ȹ��");

        ulong myClientId = NetworkManager.Singleton.LocalClientId;

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.UpdateInventory();
        }

        if (NetworkHistoryManager.Instance != null)
        {
            if (item.itemID == 1001)
            {
                NetworkHistoryManager.Instance.AddCoinServerRpc(myClientId);
            }
        }
    }

    public void RemoveItem(int id)
    {

        if (!IsOwner) return;

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

    public void SendInventoryToSession()
    {
        if (PlayerSession.Instance == null) return;

        foreach (var item in tmpInventory)
        {
            int itemID = item.Key;
            int count = item.Value;

            PlayerSession.Instance.UpdateItem(itemID, 1, count, 0);
        }

        tmpInventory.Clear();
        itemOrder.Clear();
    }

    public void DontSendInventoryToSession()
    {
        if (PlayerSession.Instance == null) return;

        tmpInventory.Clear();
        itemOrder.Clear();
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

