using UnityEngine;

public class PlayerStunnedState : PlayerBaseState
{
	private float stunDuration = 0.5f;
	private float elapsed;

	public void SetDuration(float duration) => stunDuration = duration;

	public override void Enter(Player player)
	{
		Debug.Log("Player is Stunned!");
		elapsed = 0f;

		player.Motor.StopHorizontal();
	}

	public override void Tick(Player player)
	{
		elapsed += Time.deltaTime;
		CheckStateTransitions(player);
	}

	private void CheckStateTransitions(Player player)
	{
		if (elapsed >= stunDuration)
		{
			if (player.Motor.IsGrounded)
				player.ChangeState(player.Grounded);
			else
				player.ChangeState(player.Airborne);
		}
	}
}
