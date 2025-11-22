using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField] private int damage = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats ps = collision.GetComponent<PlayerStats>();
        if (ps != null)
        {
            ps.TakeDamage(damage);
        }
    }
}
