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

	}

public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealthNet.Value = maxHealth;
        }

		CurrentHealth = currentHealthNet.Value;

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
				CurrentHealth = newValue;
	}
	
	public void ChangeHealth(int amount)
	{
		if (!IsServer) return;

		if (currentHealthNet.Value <= 0 && amount < 0)
			return;


		currentHealthNet.Value = Mathf.Clamp(currentHealthNet.Value + amount, 0, maxHealth);
        CurrentHealth = currentHealthNet.Value;
	}
}
