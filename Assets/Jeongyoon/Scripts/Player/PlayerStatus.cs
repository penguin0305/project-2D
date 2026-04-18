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
	public int CurrentHealth { get; private set; }

	public NetworkVariable<int> currentHealthNet = new NetworkVariable<int>();
	private void Awake()
	{
		MeleeATK = baseMeleeATK;
		RangeATK = baseRangeATK;
		CurrentHealth = maxHealth;

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


		//04-18 영웅_체력 동기화 수정
		int nextHealth = Mathf.Clamp(currentHealthNet.Value + amount, 0, maxHealth);
		currentHealthNet.Value = nextHealth;
		CurrentHealth = nextHealth;

		if (IsOwner)
		{
			if (NetworkHistoryManager.Instance != null)
			{
				NetworkHistoryManager.Instance.UpdateHPServerRpc(OwnerClientId, CurrentHealth);
			}
		}
	}
}
