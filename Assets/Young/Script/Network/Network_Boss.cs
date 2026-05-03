using UnityEngine;
using Unity.Netcode;

public class Network_Boss : enemyController
{

    public override void die()
    {
        if (!IsServer) return;

        InteractPortal[] portals = Object.FindObjectsByType<InteractPortal>(FindObjectsSortMode.None);

        foreach (var portal in portals)
        {
            portal.ActivatePortal();
        }

        if (dropper != null)
        {
            dropper.DropItems();
        }

        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn(true);
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }
}