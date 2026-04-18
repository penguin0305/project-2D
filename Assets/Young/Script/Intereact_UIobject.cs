using UnityEngine;
using UnityEngine.Events;

public class UIInteractable : MonoBehaviour, IInteractable
{
    public GameObject targetUI;

    public UnityEvent onInteract;

    private bool isUIOpen = false;

    public void Interact(PlayerInteraction interactor)
    {
        if (targetUI == null) return;

        isUIOpen = !isUIOpen;
        targetUI.SetActive(isUIOpen);

        if (isUIOpen)
        {
            onInteract?.Invoke();
        }
    }
}