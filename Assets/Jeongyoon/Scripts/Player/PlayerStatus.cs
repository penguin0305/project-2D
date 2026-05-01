using UnityEngine;

public class PlayerStatus : MonoBehaviour
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

	private PlayerSync sync;

	private void Awake()
	{
		sync = GetComponent<PlayerSync>();
		RecalculateStats();
	}

	private void Start()
	{
		sync.health.OnValueChanged += OnChangeHealth;
	}

	private void OnDestroy()
	{
		if (sync != null)
			sync.health.OnValueChanged -= OnChangeHealth;
	}

	private void OnChangeHealth(int oldValue, int newValue)
	{
		CurrentHealth = newValue;
	}

	public void ApplyBonus(BonusStatResult bonus)
	{
		if (!sync.IsServer)
			return;

		bonusMaxHealth = bonus.maxHP;
		bonusMeleeATK  = (int)bonus.meleeATK;
		bonusRangeATK  = (int)bonus.rangeATK;
		bonusArmor     = (int)bonus.armor;
		bonusSpeed     = bonus.speed;

		RecalculateStats();

		sync.health.Value = MaxHealth;
		CurrentHealth = MaxHealth;

		sync.SyncBonusRpc(bonusMaxHealth, bonusMeleeATK, bonusRangeATK, bonusArmor, bonusSpeed);
	}

	public void ApplySyncedBonus(int maxHP, int meleeATK, int rangeATK, int armor, float speed)
	{
		bonusMaxHealth = maxHP;
		bonusMeleeATK  = meleeATK;
		bonusRangeATK  = rangeATK;
		bonusArmor     = armor;
		bonusSpeed     = speed;
		RecalculateStats();
		CurrentHealth  = sync.health.Value;
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
		if (!sync.IsServer)
			return;

		if (sync.health.Value <= 0 && amount < 0)
			return;

		int nextHealth = Mathf.Clamp(sync.health.Value + amount, 0, MaxHealth);
		sync.health.Value = nextHealth;
		CurrentHealth = nextHealth;

		if (sync.IsOwner)
		{
			if (NetworkHistoryManager.Instance != null)
				NetworkHistoryManager.Instance.UpdateHPServerRpc(sync.OwnerClientId, CurrentHealth);
		}
	}
}