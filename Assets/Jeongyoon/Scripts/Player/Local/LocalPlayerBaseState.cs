public abstract class LocalPlayerBaseState : ILocalPlayerState
{
	public virtual void Enter(LocalPlayer player) { }
	public virtual void Exit(LocalPlayer player) { }
	public virtual void Tick(LocalPlayer player) { }
	public virtual void FixedTick(LocalPlayer player) { }
}
