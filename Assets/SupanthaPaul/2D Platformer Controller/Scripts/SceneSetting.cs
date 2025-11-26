using UnityEngine;
using SupanthaPaul;
// using SupanthaPaul; // CameraFollow 스크립트가 있는 네임스페이스 (필요시 주석 해제)

public class SceneSetup : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        if (GameSceneManager.Instance != null && GameSceneManager.Instance.playerPrefab != null)
        {

            GameObject playerInstance = Instantiate(GameSceneManager.Instance.playerPrefab, spawnPoint.position, Quaternion.identity);


            CameraFollow camScript = FindObjectOfType<CameraFollow>();

            if (camScript != null)
            {
                camScript.target = playerInstance.transform;
            }
        }
        else
        {
            Debug.LogError("SceneSetting Error");
        }
    }
}