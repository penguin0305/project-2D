using UnityEngine;

public class LocalPlayerGroundState : LocalPlayerBaseState
{
	private bool isJumpLatched;
	private float stepTimer;

	private const float MoveSpeed = 5f;
	private const float DashSpeedMultiplier = 1.5f;
	private const float JumpForce = 12f;
	private const float WalkStepInterval = 0.4f;
	private const float DashStepInterval = 0.2f;

	public override void Enter(LocalPlayer player)
	{
		isJumpLatched = false;
		stepTimer = 0f;
	}

	public override void Tick(LocalPlayer player)
	{
		HandleInput(player);
	}

	public override void FixedTick(LocalPlayer player)
	{
		CheckStateTransitions(player);
		Move(player);
		Footstep(player);

		if (isJumpLatched)
			Jump(player);
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
		float speed = MoveSpeed;
		if (player.Input.DashHeld)
			speed *= DashSpeedMultiplier;

		player.Motor.SetVelocityX(player.Input.Move.x * speed);
		player.Motor.UpdateFacingDirection(player.Input.Move.x);
	}

	private void Jump(LocalPlayer player)
	{
		player.Motor.SetVelocityY(JumpForce);
		player.Audio.PlayJump();

		LocalPlayerAirborneState airborne = (LocalPlayerAirborneState)player.Airborne;
		airborne.SetPreviousJump();

		isJumpLatched = false;
	}

	private void CheckStateTransitions(LocalPlayer player)
	{
		if (player.Input.Move.y > 0.1f && player.OverlapSensor.IsOnLadder)
		{
			player.ChangeState(player.Climb);
			return;
		}

		if (!player.Motor.IsGrounded)
		{
			player.ChangeState(player.Airborne);
			return;
		}
	}

	private void Footstep(LocalPlayer player)
	{
		if (Mathf.Abs(player.Input.Move.x) < 0.1f)
		{
			stepTimer = 0f;
			return;
		}

		stepTimer += Time.fixedDeltaTime;
		float interval = player.Input.DashHeld ? DashStepInterval : WalkStepInterval;

		if (stepTimer >= interval)
		{
			player.Audio.PlayFootstep();
			stepTimer = 0f;
		}
	}
}
