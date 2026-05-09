using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerInputState : MonoBehaviour
{
	// State (poll every frame)
	public Vector2 Move
	{
		get;
		private set;
	}
	public bool DashHeld
	{
		get;
		private set;
	}

	// Event (consume)
	private bool jumpPressed;
	private bool attackPressed;
	private bool interactPressed;
	private bool useItem1Pressed;
	private bool useItem2Pressed;
	private bool toggleStatPressed;

	public void OnMove(InputAction.CallbackContext context)
	{
		Move = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.performed)
			jumpPressed = true;
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		if (context.performed)
			DashHeld = true;
		if (context.canceled)
			DashHeld = false;
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		if (context.started)
			attackPressed = true;
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (context.performed)
			interactPressed = true;
	}

	public void OnUseItem1(InputAction.CallbackContext context)
	{
		if (context.performed)
			useItem1Pressed = true;
	}

	public void OnUseItem2(InputAction.CallbackContext context)
	{
		if (context.performed)
			useItem2Pressed = true;
	}

	public void OnToggleStat(InputAction.CallbackContext context)
	{
		if (context.performed)
			toggleStatPressed = true;
	}

	// Event-Consume API (call from player.cs once per frame - polling)

	public bool ConsumeJumpPressed()	=> Consume(ref jumpPressed);
	public bool ConsumeAttackPressed()	=> Consume(ref attackPressed);
	public bool ConsumeInteractPressed()	=> Consume(ref interactPressed);
	public bool ConsumeUseItem1Pressed()	=> Consume(ref useItem1Pressed);
	public bool ConsumeUseItem2Pressed()	=> Consume(ref useItem2Pressed);
	public bool ConsumeToggleStatPressed()	=> Consume(ref toggleStatPressed);

	private static bool Consume(ref bool flag)
	{
		if (!flag)
			return false;
		flag = false;
		return true;
	}
}
