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
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnPlayers;
        }
    }

    private void SpawnPlayers(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        int spawnIndex = 0;

        foreach (ulong clientId in clientsCompleted)
        {

            int characterTypeIndex = (int)(clientId % (ulong)characterPrefabs.Length);

            GameObject selectedPrefab = characterPrefabs[characterTypeIndex];

            Transform spawnPoint = spawnPoints[spawnIndex % spawnPoints.Length];

            GameObject playerInstance = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

            spawnIndex++;
        }
    }
}