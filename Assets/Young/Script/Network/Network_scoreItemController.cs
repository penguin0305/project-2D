using UnityEngine;
using Unity.Netcode;
public class NetworkscoreItemController : NetworkBehaviour

{
    public int itemScore;
    public itemData data;

    private void Awake()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsSpawned) return;

        NetworkObject playerNetObj = collision.GetComponent<NetworkObject>();

        if (collision.CompareTag("Player"))
        {
            if (playerNetObj != null && playerNetObj.IsOwner) 
                {
                //data.itemQuantity++;//>>나중에 데이터를 복사본으로 받아서 따로 관리 필요
                //Debug.Log(data.itemQuantity);

                //inventory UI 추가
                NetworkInventoryManager.Instance.AddItem(data);
    
                RequestDespawnItemServerRpc();
                }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestDespawnItemServerRpc()
    {
        if (NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn(true);
        }
    }
}
