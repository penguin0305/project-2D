using Unity.Netcode;
using UnityEngine;
using YoungCameraFollow;

public class NetworkCameraSetup : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Camera mainCamera = Camera.main;

            if (mainCamera != null)
            {
                CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();

                if (cameraFollow != null)
                {
                    cameraFollow.SetTarget(transform);
                }
            }
        }
    }
}