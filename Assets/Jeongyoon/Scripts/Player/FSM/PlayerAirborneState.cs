using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
	private bool isJumpLatched;
	private int currentJumpCount;

	private const float JumpForce = 12f;
	private const int MaxJumpCount = 2;

	public override void Enter(Player player)
	{
		isJumpLatched = false;
	}

	public override void Tick(Player player)
	{
		HandleInput(player);
	}

	public override void FixedTick(Player player)
	{
		CheckStateTransitions(player);

		Move(player);

		if (isJumpLatched)
			Jump(player);
	}

	public override void Exit(Player player)
	{
		currentJumpCount = 0;
	}

	public void SetPreviousJump()
	{
		currentJumpCount = 1;
	}

	private void HandleInput(Player player)
	{
		if (player.Input.ConsumeJumpPressed())
			isJumpLatched = true;
		if (player.Input.ConsumeAttackPressed())
		{
			if (player.Combat.CurrentMode == PlayerCombat.CombatMode.Melee)
				player.Combat.TryMeleeAttack();
			else if (player.Combat.CurrentMode == PlayerCombat.CombatMode.Range)
				player.Combat.TryRangeAttack();
		}
		if (player.Input.ConsumeInteractPressed())
			player.Interaction.TryInteract();
	}

	private void Move(Player player)
	{
		// 기존: AirMoveSpeed 상수 → player.Status.Speed 사용
		player.Motor.SetVelocityX(player.Input.Move.x * player.Status.Speed);
		player.Motor.UpdateFacingDirection(player.Input.Move.x);
	}

	private void Jump(Player player)
	{
		if (currentJumpCount < MaxJumpCount)
		{
			player.Motor.SetVelocityY(JumpForce);
			currentJumpCount++;
			player.Audio.PlayJump();
		}
		isJumpLatched = false;
	}

	private void CheckStateTransitions(Player player)
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