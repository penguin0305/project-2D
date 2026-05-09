using UnityEngine;

// 기존: NetworkBehaviour
// public class PlayerStatus : NetworkBehaviour
public class PlayerStatus : MonoBehaviour
{
	private int baseMaxHealth = 100;
	private int baseMeleeATK = 5;
	private int baseRangeATK = 3;
	private int baseArmor = 0;
	private float baseSpeed = 4f;
	private float baseCritRate = 0.05f;
	private float baseCritDamage = 1.5f;

	private int bonusMaxHealth = 0;
	private int bonusMeleeATK = 0;
	private int bonusRangeATK = 0;
	private int bonusArmor = 0;
	private float bonusSpeed = 0;
	private float bonusCritRate = 0f;
	private float bonusCritDamage = 0f;

	public int MaxHealth => baseMaxHealth + bonusMaxHealth;
	public int MeleeATK { get; private set; }
	public int RangeATK { get; private set; }
	public int Armor { get; private set; }
	public float Speed { get; private set; }
	public float CritRate { get; private set; }
	public float CritDamage { get; private set; }
	public int CurrentHealth { get; private set; }

	// 기존: NetworkVariable이 여기 있었음
	// public NetworkVariable<int> currentHealthNet = new NetworkVariable<int>(
	//     0,
	//     NetworkVariableReadPermission.Everyone,
	//     NetworkVariableWritePermission.Server
	// );
	// → PlayerSync.health로 이동

	private PlayerSync sync;

	private void Awake()
	{
		sync = GetComponent<PlayerSync>();
		RecalculateStats();
	}

	// 기존: OnNetworkSpawn에서 콜백 등록
	// public override void OnNetworkSpawn()
	// {
	//     currentHealthNet.OnValueChanged += OnChangeHealth;
	// }
	private void Start()
	{
		sync.health.OnValueChanged += OnChangeHealth;
	}

	// 기존: OnNetworkDespawn에서 콜백 해제
	// public override void OnNetworkDespawn()
	// {
	//     currentHealthNet.OnValueChanged -= OnChangeHealth;
	// }
	private void OnDestroy()
	{
		if (sync != null)
			sync.health.OnValueChanged -= OnChangeHealth;
	}

	private void OnChangeHealth(int oldValue, int newValue)
	{
		CurrentHealth = newValue;

		// 기존: NetworkPlayerUI.Instance.UpdateHP(newValue) 직접 호출
		// → NetworkPlayerUI가 PlayerSync.health에 직접 콜백 등록하는 방식으로 변경
	}

	public void ApplyBonus(BonusStatResult bonus)
	{
		// 기존: if (!IsServer) return;
		if (!sync.IsServer) return;

		bonusMaxHealth = bonus.maxHP;
		bonusMeleeATK  = (int)bonus.meleeATK;
		bonusRangeATK  = (int)bonus.rangeATK;
		bonusArmor     = (int)bonus.armor;
		bonusSpeed     = bonus.speed;
		bonusCritRate  = bonus.critRate;
		bonusCritDamage = bonus.critDamage;

		RecalculateStats();

		// 기존: currentHealthNet.Value = MaxHealth;
		sync.health.Value = MaxHealth;
		CurrentHealth = MaxHealth;

		sync.SyncBonusRpc(bonusMaxHealth, bonusMeleeATK, bonusRangeATK, bonusArmor, bonusSpeed, bonusCritRate, bonusCritDamage);
	}

	public void ApplySyncedBonus(int maxHP, int meleeATK, int rangeATK, int armor, float speed, float critRate, float critDamage)
	{
		bonusMaxHealth  = maxHP;
		bonusMeleeATK   = meleeATK;
		bonusRangeATK   = rangeATK;
		bonusArmor      = armor;
		bonusSpeed      = speed;
		bonusCritRate   = critRate;
		bonusCritDamage = critDamage;
		RecalculateStats();
		CurrentHealth   = sync.health.Value;
	}

	private void RecalculateStats()
	{
		MeleeATK   = baseMeleeATK  + bonusMeleeATK;
		RangeATK   = baseRangeATK  + bonusRangeATK;
		Armor      = baseArmor     + bonusArmor;
		Speed      = baseSpeed     + bonusSpeed;
		CritRate   = baseCritRate  + bonusCritRate;
		CritDamage = baseCritDamage + bonusCritDamage;
	}

	public void ChangeHealth(int amount)
	{
		// 기존: if (!IsServer) return;
		if (!sync.IsServer) return;

		if (sync.health.Value <= 0 && amount < 0)
			return;

		// 기존: currentHealthNet.Value
		int nextHealth = Mathf.Clamp(sync.health.Value + amount, 0, MaxHealth);
		sync.health.Value = nextHealth;
		CurrentHealth = nextHealth;

		if (NetworkHistoryManager.Instance != null && NetworkHistoryManager.Instance.IsSpawned)
        {
            NetworkHistoryManager.Instance.UpdateHPServerRpc(sync.OwnerClientId, CurrentHealth);
        }
	}
}