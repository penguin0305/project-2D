using UnityEngine;
using Unity.Netcode;

public class LocalSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    private void Update()
    {
        if (NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsConnectedClient &&
            NetworkManager.Singleton.LocalClient.PlayerObject != null)
        {
            NetworkManager.Singleton.LocalClient.PlayerObject.transform.position = spawnPoint.position;

            enabled = false;
        }
    }
}