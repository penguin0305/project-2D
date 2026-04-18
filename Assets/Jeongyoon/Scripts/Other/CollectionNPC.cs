using UnityEngine;

public class CollectionNPC : MonoBehaviour, IInteractable
{
	public void Interact(PlayerInteraction player)
	{
		Debug.Log("NPC Interact!");
		CollectionUIManager.Instance.Open();
	}
}