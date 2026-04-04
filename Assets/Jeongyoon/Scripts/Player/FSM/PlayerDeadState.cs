using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
	public override void Enter(Player player)
	{
		player.Motor.StopAll();

		var rb = player.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			rb.bodyType = RigidbodyType2D.Static;
		}
	}
}
