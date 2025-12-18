using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public event System.Action OnDeath;
    public event System.Action<int> OnCheckHP;

    [SerializeField] private int maxHP = 5;

    private int currentHP;

    private PlayerHitFeedback hitFeedback;

    private void Awake()
    {
        hitFeedback = GetComponent<PlayerHitFeedback>();
    }

    private void Start()
    {
        currentHP = maxHP;
        OnCheckHP?.Invoke(currentHP);
        HistoryManager.Instance.UpdateHP(currentHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log($"Player Hit! HP: {currentHP}");

        hitFeedback.PlayHitFeedback();

        OnCheckHP?.Invoke(currentHP);
        HistoryManager.Instance.UpdateHP(currentHP);

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
