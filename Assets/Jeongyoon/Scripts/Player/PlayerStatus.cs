using UnityEngine;
using Unity.Netcode;

public class PlayerStatus : NetworkBehaviour
{
	[SerializeField] private int maxHealth = 20;
	private int baseMeleeATK = 5;
	private int baseRangeATK = 3;
	private int baseArmor = 0;
	public int CurrentHealth { get; private set; }
	public int MeleeATK { get; private set; }
	public int RangeATK { get; private set; }
	public int Armor { get; private set; }

	private void Awake()
	{
		CurrentHealth = maxHealth;
		MeleeATK = baseMeleeATK;
		RangeATK = baseRangeATK;

		if (IsOwner)
		{
			if (NetworkHistoryManager.Instance != null)
			{
				NetworkHistoryManager.Instance.UpdateHPServerRpc(OwnerClientId, CurrentHealth);
			}
		}
	}
	public void ChangeHealth(int amount)
	{
		if (CurrentHealth <= 0 && amount < 0)
			return;

		CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);

		if (IsOwner)
		{
			if (NetworkHistoryManager.Instance != null)
			{
				NetworkHistoryManager.Instance.UpdateHPServerRpc(OwnerClientId, CurrentHealth);
			}
		}
	}
}
