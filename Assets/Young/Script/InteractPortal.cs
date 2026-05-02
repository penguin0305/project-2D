using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class InteractPortal : NetworkBehaviour, IInteractable
{
    [SerializeField] public Sprite OnPortal;
    [SerializeField] public Sprite OffPortal;
    [SerializeField] public string EndingSceneName = "EndScene1217";
    
    private StageManager stageManager;
    private SpriteRenderer spriteRenderer;

    private NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false);
    
    private NetworkVariable<int> playersInPortal = new NetworkVariable<int>(0);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        stageManager = FindAnyObjectByType<StageManager>();
    }

    public override void OnNetworkSpawn()
    {
        OnPortalStateChanged(false, isActivated.Value);
        isActivated.OnValueChanged += OnPortalStateChanged;

        if (IsServer)
        {
            stageManager.OnStageClear += ActivatePortalServer;
        }
    }

    private void OnPortalStateChanged(bool previousValue, bool newValue)
    {
        spriteRenderer.sprite = newValue ? OnPortal : OffPortal;
    }

    private void ActivatePortalServer(List<itemData> tmp)
    {
        if (IsServer) isActivated.Value = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer || !isActivated.Value) return;

        if (collision.CompareTag("Player"))
        {
ulong clientId = collision.GetComponent<NetworkObject>().OwnerClientId;
SetPlayerVisibilityClientRpc(clientId, false);

            playersInPortal.Value++;
            CheckAllPlayersIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsServer || !isActivated.Value) return;

        if (collision.CompareTag("Player"))
        {
            playersInPortal.Value = Mathf.Max(0, playersInPortal.Value - 1);
        }
    }

    private void CheckAllPlayersIn()
    {
        int totalPlayers = NetworkManager.Singleton.ConnectedClients.Count;

        if (playersInPortal.Value >= totalPlayers)
        {
            ToEnding();
        }
    }

[ClientRpc]
private void SetPlayerVisibilityClientRpc(ulong clientId, bool isVisible)
{
    if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var networkClient))
    {
        var playerObject = networkClient.PlayerObject;
        if (playerObject != null)
        {
            var renderers = playerObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers)
            {
                r.enabled = isVisible;
            }
        }
    }
}

public void Interact(PlayerInteraction player)
    {
        if (IsServer && isActivated.Value)
        {
            Debug.Log($"현재 인원: {playersInPortal.Value}");
        }
    }

    private void ToEnding()
    {
        if (!IsServer) return;

        if (NetworkHistoryManager.Instance != null)
        {
            NetworkHistoryManager.Instance.GameClear();
        }

        NetworkManager.Singleton.SceneManager.LoadScene(EndingSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public override void OnNetworkDespawn()
    {
        isActivated.OnValueChanged -= OnPortalStateChanged;
        if (IsServer && stageManager != null)
        {
            stageManager.OnStageClear -= ActivatePortalServer;
        }
    }
}