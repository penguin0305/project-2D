using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Portaltmp : MonoBehaviour, IInteractable
{
    public string EndingSceneName = "EndScene1217";
    public Sprite PortalSprite;

    private SpriteRenderer spriteRenderer;
    private bool isVisible = false;

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