public interface ILocalPlayerState
{
	void Enter(LocalPlayer player);
	void Exit(LocalPlayer player);
	void Tick(LocalPlayer player);
	void FixedTick(LocalPlayer player);
}
