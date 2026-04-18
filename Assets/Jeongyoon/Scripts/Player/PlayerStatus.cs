using UnityEngine;
using Unity.Netcode;

public class PlayerStatus : NetworkBehaviour
{
	[SerializeField] private int maxHealth = 20;
	private int baseMeleeATK = 5;
	private int baseRangeATK = 3;
	private int baseArmor = 0;
	public int MeleeATK { get; private set; }
	public int RangeATK { get; private set; }
	public int Armor { get; private set; }

	private void Awake()
	{
		MeleeATK = baseMeleeATK;
		RangeATK = baseRangeATK;

		if (IsOwner)
		{
			if (NetworkPlayerUI.Instance != null)
			{
				NetworkPlayerUI.Instance.UpdateHP(currentHealthNet.Value);
			}
			currentHealthNet.OnValueChanged += OnChangeHealth;
		}
	}

	public override void OnNetworkDespawn()
	{
		if (IsOwner)
		{
			currentHealthNet.OnValueChanged -= OnChangeHealth;
		}
	}

	private void OnChangeHealth(int oldValue, int newValue)
	{
		if (NetworkPlayerUI.Instance != null)
		{
			NetworkPlayerUI.Instance.UpdateHP(newValue);
		}
	}
	public void ChangeHealth(int amount)
	{
		if (!IsServer) return;

		if (currentHealthNet.Value <= 0 && amount < 0)
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
