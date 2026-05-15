using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections.Generic;

using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class Portaltmp : NetworkBehaviour, IInteractable
{
    public Sprite PortalSprite;
    private SpriteRenderer spriteRenderer;

    private NetworkVariable<bool> isVisible = new NetworkVariable<bool>(false);
    private NetworkVariable<int> playersInteracted = new NetworkVariable<int>(0);
    private HashSet<ulong> confirmedPlayers = new HashSet<ulong>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            spriteRenderer.sprite = PortalSprite;
        }
    }

    public override void OnNetworkSpawn()
    {
        isVisible.OnValueChanged += (prev, next) => RefreshVisual(next);
        RefreshVisual(isVisible.Value);
    }

    private void RefreshVisual(bool visible)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = visible;
    }

    public void ActivatePortal()
    {
        if (!IsServer) return;
        isVisible.Value = true;
        playersInteracted.Value = 0;
        confirmedPlayers.Clear();
    }

    public void Interact(PlayerInteraction player)
    {
        if (!isVisible.Value) return;

        // 인벤토리 세션 데이터 전송 (필요 시)
        if (NetworkInventoryManager.Instance != null)
            NetworkInventoryManager.Instance.SendInventoryToSession(10);

        SubmitPortalInteractionServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitPortalInteractionServerRpc(ulong clientId)
    {
        if (confirmedPlayers.Contains(clientId)) return;

        confirmedPlayers.Add(clientId);
        playersInteracted.Value++;

        int connectedCount = NetworkManager.Singleton.ConnectedClients.Count;

        // 모든 플레이어가 포탈에 상호작용했다면
        if (playersInteracted.Value >= connectedCount)
        {
            // [핵심] 씬 이동 대신GameOverManager를 통해 전원에게 UI를 띄움
            if (GameOverManager.Instance != null)
            {
                GameOverManager.Instance.NotifyAllPlayersClearServer();
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if (isVisible != null)
            isVisible.OnValueChanged -= (prev, next) => RefreshVisual(next);
    }
}
/*~0514
public class Portaltmp : NetworkBehaviour, IInteractable
{
    public string EndingSceneName = "EndScene1217";
    public Sprite PortalSprite;

    private SpriteRenderer spriteRenderer;

    private NetworkVariable<bool> isVisible = new NetworkVariable<bool>(false);
    private NetworkVariable<int> playersInteracted = new NetworkVariable<int>(0);
    private HashSet<ulong> confirmedPlayers = new HashSet<ulong>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            spriteRenderer.sprite = PortalSprite;
        }
    }

    public override void OnNetworkSpawn()
    {
        isVisible.OnValueChanged += (prev, next) => RefreshVisual(next);
        RefreshVisual(isVisible.Value);
    }

    private void RefreshVisual(bool visible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = visible;
        }
    }

    public void ActivatePortal()
    {
        if (!IsServer) return;
        isVisible.Value = true;
        playersInteracted.Value = 0;
        confirmedPlayers.Clear();
    }

    public void Interact(PlayerInteraction player)
    {
        if (!isVisible.Value) return;

        if (NetworkInventoryManager.Instance != null)
        {
            NetworkInventoryManager.Instance.SendInventoryToSession(10);
        }

        SubmitPortalInteractionServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitPortalInteractionServerRpc(ulong clientId)
    {
        if (confirmedPlayers.Contains(clientId)) return;

        confirmedPlayers.Add(clientId);
        playersInteracted.Value++;

        Debug.Log($"상호작용 인원: {playersInteracted.Value} / {NetworkManager.Singleton.ConnectedClients.Count}");


        CheckAllPlayersConfirmed();
    }

    private void CheckAllPlayersConfirmed()
    {
        if (!IsServer) return;

        int connectedCount = NetworkManager.Singleton.ConnectedClients.Count;

        if (playersInteracted.Value >= connectedCount)
        {
            GoToEndingClientRpc();
        }
    }

[ClientRpc]
private void GoToEndingClientRpc()
{
    StartCoroutine(LeaveSequence());
}

private System.Collections.IEnumerator LeaveSequence()
{
    bool isHost = IsServer;

    if (NetworkInventoryManager.Instance != null)
        NetworkInventoryManager.Instance.DontSendInventoryToSession();

    if (NetworkManager.Singleton != null)
    {
        NetworkManager.Singleton.Shutdown();
        
        if (isHost && NetworkManager.Singleton.gameObject != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }

    yield return null;

    SceneManager.LoadScene(EndingSceneName);
}

    public override void OnNetworkDespawn()
    {
        if (isVisible != null)
            isVisible.OnValueChanged -= (prev, next) => RefreshVisual(next);
    }
}
*/