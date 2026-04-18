using Unity.Netcode;
using UnityEngine;

namespace Multiplay
{
    public class NetworkPlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject networkPlayerPrefab;
        [SerializeField] private Transform[] spawnPoints;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                SpawnPlayers();
            }
        }

        private void SpawnPlayers()
        {
            int index = 0;

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Transform spawnPoint = spawnPoints[index % spawnPoints.Length];

                GameObject playerInstance = Instantiate(networkPlayerPrefab, spawnPoint.position, spawnPoint.rotation);

                var networkObject = playerInstance.GetComponent<NetworkObject>();
                networkObject.SpawnAsPlayerObject(clientId);

                index++;
            }
        }
    }
}