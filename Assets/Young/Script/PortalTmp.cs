using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Portaltmp : NetworkBehaviour, IInteractable
{
    public string EndingSceneName = "EndScene1217";
    public Sprite PortalSprite;

    private SpriteRenderer spriteRenderer;

    private NetworkVariable<bool> isVisible = new NetworkVariable<bool>(false);
    /*
    private NetworkVariable<int> playersInteracted = new NetworkVariable<int>(0);
    private HashSet<ulong> confirmedPlayers = new HashSet<ulong>();
    */
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            spriteRenderer.sprite = PortalSprite;
        }
    }

    public void ActivateVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            isVisible = true;
        }
    }


    public void Interact(PlayerInteraction player)
    {
        if (!isVisible)
        {
            Debug.Log("비활성화");
            return;
        }

        if (NetworkInventoryManager.Instance != null)
        {
            NetworkInventoryManager.Instance.SendInventoryToSession(10);
        }

        GoToEnding();
    }
    /*
    [ServerRpc(RequireOwnership = false)]
    private void ConfirmPortalEntryServerRpc(ulong clientId)
    {
        if (confirmedPlayers.Contains(clientId)) return;

        confirmedPlayers.Add(clientId);
        playersInteracted.Value++;

        SetPlayerVisibilityClientRpc(clientId, false);

        CheckAllPlayersIn();
    }

    private void CheckAllPlayersIn()
    {
        if (!IsServer) return;

        int totalPlayers = NetworkManager.Singleton.ConnectedClients.Count;

        if (playersInteracted.Value >= totalPlayers)
        {
            ToEnding();
        }
    }
    */

    private void GoToEnding()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();

            if (NetworkManager.Singleton.gameObject != null)
            {
                Destroy(NetworkManager.Singleton.gameObject);
            }
        }
        SceneManager.LoadScene(EndingSceneName);

    }

}