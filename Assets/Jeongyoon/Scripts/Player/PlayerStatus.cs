using UnityEngine;
using Unity.Netcode;

public class PlayerStatus : NetworkBehaviour
{
	private int baseMaxHealth = 100;
	private int baseMeleeATK = 5;
	private int baseRangeATK = 3;
	private int baseArmor = 0;
	private float baseSpeed = 0;

	private int bonusMaxHealth = 0;
	private int bonusMeleeATK = 0;
	private int bonusRangeATK = 0;
	private int bonusArmor = 0;
	private float bonusSpeed = 0;

	public int MaxHealth => baseMaxHealth + bonusMaxHealth;
	public int MeleeATK { get; private set; }
	public int RangeATK { get; private set; }
	public int Armor { get; private set; }
	public float Speed { get; private set; }
	public int CurrentHealth { get; private set; }

	public NetworkVariable<int> currentHealthNet = new NetworkVariable<int>(
		0,
		NetworkVariableReadPermission.Everyone,
		NetworkVariableWritePermission.Server
	);

	private void Awake()
	{
		RecalculateStats();
	}

	public override void OnNetworkSpawn()
	{
		currentHealthNet.OnValueChanged += OnChangeHealth;
	}

	public override void OnNetworkDespawn()
	{
		currentHealthNet.OnValueChanged -= OnChangeHealth;
	}

	private void OnChangeHealth(int oldValue, int newValue)
	{
		CurrentHealth = newValue;

		if (IsOwner && NetworkPlayerUI.Instance != null)
			NetworkPlayerUI.Instance.UpdateHP(newValue);
	}

	public void ApplyBonus(BonusStatResult bonus)
	{
		if (!IsServer) return;

		bonusMaxHealth = bonus.maxHP;
		bonusMeleeATK  = (int)bonus.meleeATK;
		bonusRangeATK  = (int)bonus.rangeATK;
		bonusArmor     = (int)bonus.armor;
		bonusSpeed     = bonus.speed;

		RecalculateStats();

		currentHealthNet.Value = MaxHealth;
		CurrentHealth = MaxHealth;

		SyncBonusClientRpc(bonusMaxHealth, bonusMeleeATK, bonusRangeATK, bonusArmor, bonusSpeed);
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void SyncBonusClientRpc(int maxHP, int meleeATK, int rangeATK, int armor, float speed)
	{
		bonusMaxHealth = maxHP;
		bonusMeleeATK  = meleeATK;
		bonusRangeATK  = rangeATK;
		bonusArmor     = armor;
		bonusSpeed     = speed;
		RecalculateStats();
		CurrentHealth  = currentHealthNet.Value;
	}

	private void RecalculateStats()
	{
		MeleeATK = baseMeleeATK + bonusMeleeATK;
		RangeATK = baseRangeATK + bonusRangeATK;
		Armor    = baseArmor    + bonusArmor;
		Speed    = baseSpeed    + bonusSpeed;
	}

	public void ChangeHealth(int amount)
	{
		if (!IsServer) return;

		if (currentHealthNet.Value <= 0 && amount < 0)
			return;

		int nextHealth = Mathf.Clamp(currentHealthNet.Value + amount, 0, MaxHealth);
		currentHealthNet.Value = nextHealth;
		CurrentHealth = nextHealth;

		if (IsOwner)
		{
			if (NetworkHistoryManager.Instance != null)
				NetworkHistoryManager.Instance.UpdateHPServerRpc(OwnerClientId, CurrentHealth);
		}
	}
}