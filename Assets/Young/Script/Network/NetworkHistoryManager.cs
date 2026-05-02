using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections.Generic;


public class PlayerSessionData
{
    public int currentHP;
    public int coinCount = 0;
    public int tempcoinCount = 0;
    public int DefeatCount = 0;
}

public class NetworkHistoryManager : NetworkBehaviour
{
    public static NetworkHistoryManager Instance;

    private Dictionary<ulong, PlayerSessionData> sessionData = new Dictionary<ulong, PlayerSessionData>();
    public PlayerSessionData mySessionData = new PlayerSessionData();

    public NetworkVariable<float> playTime = new NetworkVariable<float>(0f);
    public NetworkVariable<bool> isGameActive = new NetworkVariable<bool>(true);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

            foreach (var client in NetworkManager.Singleton.ConnectedClientsIds)
            {
                RegisterPlayer(client);
            }
            NetworkManager.Singleton.OnClientConnectedCallback += RegisterPlayer;
        }
    }

    private void Update()
    {
        if (IsServer)
        {
            if (isGameActive.Value)
            {
                playTime.Value += Time.deltaTime;
            }
        }
    }

    public void RegisterPlayer(ulong clientId)
    {
        if (IsServer && !sessionData.ContainsKey(clientId))
        {
            sessionData.Add(clientId, new PlayerSessionData());
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void AddDefeatCountServerRpc(ulong clientId)
    {
        if (sessionData.ContainsKey(clientId))
        {
            sessionData[clientId].DefeatCount++;
            SendSyncDataToClient(clientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateHPServerRpc(ulong clientId, int hp)
    {
        if (sessionData.ContainsKey(clientId))
        {
            sessionData[clientId].currentHP = hp;
            SendSyncDataToClient(clientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddCoinServerRpc(ulong clientId)
    {
        if (sessionData.ContainsKey(clientId))
        {
            sessionData[clientId].coinCount++;
            sessionData[clientId].tempcoinCount++;
            SendSyncDataToClient(clientId);
        }

        else
        {
            Debug.LogWarning($"Client {clientId} is not registered in sessionData!");
        }
    }

    private void SendSyncDataToClient(ulong clientId)
    {
        PlayerSessionData data = sessionData[clientId];

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { clientId } }
        };
        SyncSessionDataClientRpc(data.currentHP, data.coinCount, data.tempcoinCount, data.DefeatCount, clientRpcParams);
    }

    [ClientRpc]
    private void SyncSessionDataClientRpc(int hp, int coin, int tempCoin, int defeatCount, ClientRpcParams clientRpcParams = default)
    {
        mySessionData.currentHP = hp;
        mySessionData.coinCount = coin;
        mySessionData.tempcoinCount = tempCoin;
        mySessionData.DefeatCount = defeatCount;

        if (NetworkPlayerUI.Instance != null)
        {
            NetworkPlayerUI.Instance.UpdateHP(mySessionData.currentHP);
        }
    }
    public void GameClear()
    {
        if (IsServer)
        {
            isGameActive.Value = false;
        }
    }
    public void ResetData()
    {
        if (IsServer)
        {
            sessionData.Clear();

            playTime.Value = 0f;
            isGameActive.Value = true;
            ResetLocalDataClientRpc();
        }
    }

    [ClientRpc]
    private void ResetLocalDataClientRpc()
    {
        mySessionData = new PlayerSessionData();
    }
}