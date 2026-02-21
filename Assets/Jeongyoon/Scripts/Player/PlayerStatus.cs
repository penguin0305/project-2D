using UnityEngine;
public class PlayerStatus : MonoBehaviour
{
	[SerializeField] private int maxHealth = 20;
	private int baseMeleeATK = 5;
	private int baseRangeATK = 3;
	private int baseArmor = 0;
	public int CurrentHealth { get; private set; }
	public int MeleeATK { get; private set; }
	public int RangeATK { get; private set; }
	public int Armor { get; private set; }

	private void Start()
	{
		CurrentHealth = maxHealth;
		MeleeATK = baseMeleeATK;
		RangeATK = baseRangeATK;
	}
	public void ChangeHealth(int amount)
	{
		if (CurrentHealth <= 0 && amount < 0)
			return;

		CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
	}
}
