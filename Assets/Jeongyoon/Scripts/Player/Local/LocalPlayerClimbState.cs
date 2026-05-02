using UnityEngine;

public class LocalPlayerClimbState : LocalPlayerBaseState
{
	private float climbSpeed = 5f;
	private bool isJumpLatched;

	public override void Enter(LocalPlayer player)
	{
		player.Motor.StopAll();
		player.Motor.SetGravityScale(0f);
		isJumpLatched = false;
	}

	public override void Tick(LocalPlayer player)
	{
		HandleInput(player);
	}

	public override void FixedTick(LocalPlayer player)
	{
		CheckStateTransitions(player);
		Climb(player);
	}

	public override void Exit(LocalPlayer player)
	{
		player.Motor.RestoreDefaultGravity();
	}

	private void HandleInput(LocalPlayer player)
	{
		if (player.Input.ConsumeJumpPressed())
			isJumpLatched = true;
	}

	private void Climb(LocalPlayer player)
	{
		player.Motor.SetVelocityY(player.Input.Move.y * climbSpeed);
	}

	private void CheckStateTransitions(LocalPlayer player)
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
