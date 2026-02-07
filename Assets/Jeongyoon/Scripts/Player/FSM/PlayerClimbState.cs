using UnityEngine;

public class PlayerClimbState : PlayerBaseState
{
	private float climbSpeed = 5f;
	private float checkRadius = 0.3f;
	private bool isJumpLatched;

	public override void Enter(Player player)
	{
		player.Motor.StopAll();
		player.Motor.SetGravityScale(0f);
		isJumpLatched = false;
	}

	public override void Tick(Player player)
	{
		HandleInput(player);
	}

	public override void FixedTick(Player player)
	{
		CheckStateTransitions(player);
		Climb(player);
	}
	public override void Exit(Player player)
	{
		player.Motor.RestoreDefaultGravity();
	}

	private void HandleInput(Player player)
	{
		if (player.Input.ConsumeJumpPressed())
			isJumpLatched = true;
	}

	private void Climb(Player player)
	{
		player.Motor.SetVelocityY(player.Input.Move.y * climbSpeed);
	}

	private void CheckStateTransitions(Player player)
	{
		if (isJumpLatched || !player.OverlapSensor.IsOnLadder)
		{
			player.ChangeState(player.Airborne);
			return;
		}

		if (player.Motor.IsGrounded && player.Input.Move.y < -0.1f)
		{
			player.Audio.PlayLand();
			player.ChangeState(player.Grounded);
			return;
		}
	}
}
