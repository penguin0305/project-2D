using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteraction player)
    {
        PlayerMovement movement = player.GetComponentInParent<PlayerMovement>();
        if (movement != null)
        {
            movement.StartClimb(); // PlayerMovement에서 클라임 시작
        }
    }
}