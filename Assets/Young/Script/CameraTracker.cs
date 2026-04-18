using UnityEngine;
using Unity.Netcode;
using YoungCameraFollow;

[RequireComponent(typeof(CameraFollow))]
public class CameraTracker : MonoBehaviour
{
    private CameraFollow cameraFollow;

    public float fixedZ = -10f;

    private void Awake()
    {
        cameraFollow = GetComponent<CameraFollow>();
    }

    private void Update()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsConnectedClient && NetworkManager.Singleton.LocalClient.PlayerObject != null)
        {
            cameraFollow.SetTarget(NetworkManager.Singleton.LocalClient.PlayerObject.transform);

            cameraFollow.offset.z = fixedZ;

            enabled = false;
        }
    }
}