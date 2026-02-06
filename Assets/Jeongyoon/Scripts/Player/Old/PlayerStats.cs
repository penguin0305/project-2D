using UnityEngine;
using VContainer;
public class PlayerStats : MonoBehaviour
{
    public event System.Action OnDeath;
    public event System.Action<int> OnCheckHP;

    private EquipStats _equipStats;
    private PlayerData _playerData;
    [Inject]
    public void Construct(EquipStats equipData, PlayerData playerData)
    {
        _playerData = playerData;
        _equipStats = equipData;

        ApplyEquipment();
    }
    private void ApplyEquipment() { }

    [SerializeField] private int maxHP = 20;

    private int currentHP;
    public int CurrentHP => currentHP;

    private PlayerHitFeedback hitFeedback;

    private void Awake()
    {
        hitFeedback = GetComponent<PlayerHitFeedback>();
    }

    private void Start()
    {
        currentHP = maxHP;
        OnCheckHP?.Invoke(currentHP);
        //HistoryManager.Instance.UpdateHP(currentHP);
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
