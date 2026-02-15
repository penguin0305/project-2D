using UnityEngine;
public class PlayerStatus : MonoBehaviour
{
	[SerializeField] private int maxHealth = 20;
	public int CurrentHealth { get; private set; }

	private void Start()
	{
		CurrentHealth = maxHealth;
	}
	public void ChangeHealth(int amount)
	{
		if (CurrentHealth <= 0 && amount < 0)
			return;

		CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
	}
}
