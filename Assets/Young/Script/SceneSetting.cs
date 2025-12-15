using UnityEngine;
using SupanthaPaul;
// using SupanthaPaul; // CameraFollow 스크립트가 있는 네임스페이스 (필요시 주석 해제)

public class SceneSetup : MonoBehaviour
{
    public Transform spawnPoint;
    private MapLoader mloader;

    void Start()
    {
        if (GameSceneManager.Instance != null && GameSceneManager.Instance.playerPrefab != null)
        {

            GameObject playerInstance = Instantiate(GameSceneManager.Instance.playerPrefab, spawnPoint.position, Quaternion.identity);

            mloader = FindAnyObjectByType<MapLoader>();

            CameraFollow camScript = FindObjectOfType<CameraFollow>();

            if (camScript != null)
            {
                camScript.target = playerInstance.transform;
            }

            // 251127 MapLoader가 플레이어 위치를 받을 수 있게 추가 - 원영
            if (mloader != null)
                mloader.PlayerTransform = playerInstance.transform;
        }
        else
        {
            Debug.LogError("SceneSetting Error");
        }
    }
}