using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayGameManager : NetworkBehaviour
{
    public Transform[] spawnPoints;

    public GameObject[] characterPrefabs;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnSinglePlayer(clientId);
        }

        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnClientSceneLoaded;
        }
    }

    private void OnClientSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (IsServer)
        {
            SpawnSinglePlayer(clientId);
        }
    }

    private void SpawnSinglePlayer(ulong clientId)
    {
        if (!IsServer) return;

        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            if (client.PlayerObject != null) return;
        }

            int spawnIndex = (int)clientId;
            int characterTypeIndex = (int)(clientId % (ulong)characterPrefabs.Length);

            GameObject selectedPrefab = characterPrefabs[characterTypeIndex];
            Transform spawnPoint = spawnPoints[spawnIndex % spawnPoints.Length];

        if (NetworkHistoryManager.Instance != null)
        {
            NetworkHistoryManager.Instance.RegisterPlayer(clientId);
        }

        GameObject playerInstance = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);


    }
}