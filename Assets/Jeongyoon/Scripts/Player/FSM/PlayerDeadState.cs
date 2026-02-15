using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
	public override void Enter(Player player)
	{
		player.Motor.StopAll();
	}
}
