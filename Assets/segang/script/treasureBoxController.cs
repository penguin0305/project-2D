using UnityEngine;
using Unity.Netcode;

public class treasureBoxController : NetworkBehaviour, IInteractable
{
    private bool isActivated = false;

    public void Interact(PlayerInteraction player)
    {
        if (isActivated) return;

        // 상호작용을 시도한 플레이어 검증 후 요청
        var networkObject = player.GetComponentInParent<NetworkObject>();
        if (networkObject != null && networkObject.IsLocalPlayer)
        {
            ActivateRpc();
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void ActivateRpc()
    {
        var stageManager = FindAnyObjectByType<StageManager>();
        if (isActivated) return;
        isActivated = true;

        VisualUpdateRpc();

        if (stageManager != null)
        {
           
            var mapControl = GetComponentInParent<MapControl>();

            if (mapControl != null)
            {
                stageManager.AddProgress(mapControl.NetworkObjectId);
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    private void VisualUpdateRpc()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;
        }
    }
}
