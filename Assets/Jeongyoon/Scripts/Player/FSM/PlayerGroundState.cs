using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
	private bool isJumpLatched;
	private float stepTimer;

	private const float MoveSpeed = 5f;
	private const float DashSpeedMultiplier = 1.5f;
	private const float JumpForce = 12f;
	private const float WalkStepInterval = 0.4f;
	private const float DashStepInterval = 0.2f;

	public override void Enter(Player player)
	{
		isJumpLatched = false;
		stepTimer = 0f;
	}

	public override void Tick(Player player)
	{
		HandleInput(player);
	}

	public override void FixedTick(Player player)
	{
		CheckStateTransitions(player);

		Move(player);
		Footstep(player);

		if (isJumpLatched)
			Jump(player);
	}

	private void HandleInput(Player player)
	{
		if (player.Input.ConsumeJumpPressed())
			isJumpLatched = true;
		if (player.Input.ConsumeAttackPressed())
			player.Combat.TryMeleeAttack();
		if (player.Input.ConsumeInteractPressed())
			player.Interaction.TryInteract();
		if (player.Input.ConsumeUseItem1Pressed())
			player.BombPlacer.TryPlaceBomb();
	}

	private void Move(Player player)
	{
		float speed = MoveSpeed;
		if (player.Input.DashHeld)
			speed *= DashSpeedMultiplier;
		
		player.Motor.SetVelocityX(player.Input.Move.x * speed);
	}

	private void Jump(Player player)
	{
		player.Motor.SetVelocityY(JumpForce);
		player.Audio.PlayJump();
		
		PlayerAirborneState ariborne = (PlayerAirborneState)player.Airborne;
		ariborne.SetPreviousJump();

		isJumpLatched = false;
	}

	private void CheckStateTransitions(Player player)
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

	private void Footstep(Player player)
	{
		if (Mathf.Abs(player.Input.Move.x) < 0.1f)
		{
			stepTimer = 0f;
			return;
		}

		stepTimer  += Time.fixedDeltaTime;
		float interval = player.Input.DashHeld ? DashStepInterval : WalkStepInterval;

		if (stepTimer >= interval)
		{
			player.Audio.PlayFootstep();
			stepTimer = 0f;
		}
	}
}
