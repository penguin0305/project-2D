using UnityEngine;
using YoungCameraFollow;

public class SceneSetup : MonoBehaviour
{
    public Transform spawnPoint;
    public MapLoader mapLoader;
    public CameraFollow camerafollow;

    void Start()
    {
        GameObject playerInstance = Instantiate(GameSceneManager.Instance.playerPrefab, spawnPoint.position, Quaternion.identity);

        camerafollow.target = playerInstance.transform;

        mapLoader.PlayerTransform = playerInstance.transform;
    }
}