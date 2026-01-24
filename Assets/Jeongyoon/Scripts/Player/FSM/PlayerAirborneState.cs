using System;
using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
	private bool isJumpLatched;

	private const float AirMoveSpeed = 5f;
	private const float JumpForce = 12f;

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
		player.Motor.SetVelocityX(player.Input.Move.x * AirMoveSpeed);
	}

	private void Jump(Player player)
	{
		player.Motor.SetVelocityY(JumpForce);
		isJumpLatched = false;
	}

	private void CheckStateTransitions(Player player)
	{
		if (!player.Motor.IsGrounded)
		{
			player.ChangeState(player.Airborne);
			return;
		}
	}
}
