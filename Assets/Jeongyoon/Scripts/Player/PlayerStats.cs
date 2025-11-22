using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHP = 5;
    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"Player Hit! HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
    }
}
