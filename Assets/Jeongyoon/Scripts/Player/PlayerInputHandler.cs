using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
	private PlayerMovement movement;
	private PlayerCombat combat;
	private PlayerInteraction interaction;

	private void Awake()
	{
		movement = GetComponent<PlayerMovement>();
		combat = GetComponent<PlayerCombat>();
		interaction = GetComponentInChildren<PlayerInteraction>();
	} 

	public void OnMove(InputAction.CallbackContext context)
	{
		Vector2 vec = context.ReadValue<Vector2>();
		movement.RequestMove(vec);
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.performed)
			movement.RequestJump();
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		if (context.performed)
			movement.RequestDash(true);
		if (context.canceled)
			movement.RequestDash(false);
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		if (context.started)
			combat.TryMeleeAttack();
		if (context.performed)
			Debug.Log("Attack: performed");
		if (context.canceled)
			Debug.Log("Attack: canceled");
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (context.performed)
			interaction.TryInteract();
	}

	public void OnUseItem1(InputAction.CallbackContext context)
	{
		if (context.performed)
			Debug.Log("Use Item 1: performed");
	}

	public void OnUseItem2(InputAction.CallbackContext context)
	{
		if (context.performed)
			Debug.Log("Use Item 2: performed");
	}
}
