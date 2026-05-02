using UnityEngine;

public class LocalPlayerAirborneState : LocalPlayerBaseState
{
	private bool isJumpLatched;
	private int currentJumpCount;

	private const float AirMoveSpeed = 5f;
	private const float JumpForce = 12f;
	private const int MaxJumpCount = 2;

	public override void Enter(LocalPlayer player)
	{
		isJumpLatched = false;
	}

	public override void Tick(LocalPlayer player)
	{
		HandleInput(player);
	}

	public override void FixedTick(LocalPlayer player)
	{
		CheckStateTransitions(player);
		Move(player);

		if (isJumpLatched)
			Jump(player);
	}

	public override void Exit(LocalPlayer player)
	{
		currentJumpCount = 0;
	}

	public void SetPreviousJump()
	{
		currentJumpCount = 1;
	}

	private void HandleInput(LocalPlayer player)
	{
		if (player.Input.ConsumeJumpPressed())
			isJumpLatched = true;
		if (player.Input.ConsumeInteractPressed())
			player.Interaction.TryInteract();
	}

	private void Move(LocalPlayer player)
	{
		player.Motor.SetVelocityX(player.Input.Move.x * AirMoveSpeed);
		player.Motor.UpdateFacingDirection(player.Input.Move.x);
	}

	private void Jump(LocalPlayer player)
	{
		if (currentJumpCount < MaxJumpCount)
		{
			player.Motor.SetVelocityY(JumpForce);
			currentJumpCount++;
			player.Audio.PlayJump();
		}
		isJumpLatched = false;
	}

	private void CheckStateTransitions(LocalPlayer player)
	{
		if (Mathf.Abs(player.Input.Move.y) > 0.1f && player.OverlapSensor.IsOnLadder)
		{
			player.ChangeState(player.Climb);
			return;
		}

		if (player.Motor.IsGrounded)
		{
			player.Audio.PlayLand();
			player.ChangeState(player.Grounded);
		}
	}
}
