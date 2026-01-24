using UnityEngine;

public abstract class PlayerBaseState : IPlayerState
{
	public virtual void Enter(Player player)
	{
	}

	public virtual void Exit(Player player)
	{
	}

	public virtual void Tick(Player player)
	{
	}

	public virtual void FixedTick(Player player)
	{
	}
}
