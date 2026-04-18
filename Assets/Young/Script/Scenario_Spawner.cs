using UnityEngine;

public class LocalSpawner : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform spawnPoint;

    private void Start()
    {
        if (PlayerPrefab != null && spawnPoint != null)
        {
            Instantiate(PlayerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}