using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public event System.Action OnDeath;
    public event System.Action<int> OnCheckHP;

    [SerializeField] private int maxHP = 5;

    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
        OnCheckHP.Invoke(currentHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        OnCheckHP.Invoke(currentHP);
        Debug.Log($"Player Hit! HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log("Player Died");
        OnDeath?.Invoke();
    }
}
