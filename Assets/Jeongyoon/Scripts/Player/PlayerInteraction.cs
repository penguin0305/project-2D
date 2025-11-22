using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentTarget;

    private void Awake()
    {
        var collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
        currentTarget = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
		{
			currentTarget = interactable;
		}
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (currentTarget == interactable)
        {
            currentTarget = null;
        }
    }

    public void TryInteract()
	{
		if (currentTarget != null)
		{
			currentTarget.Interact(this);
		}
	}

    public bool HasTarget()
	{
		return currentTarget != null;
	}
}
