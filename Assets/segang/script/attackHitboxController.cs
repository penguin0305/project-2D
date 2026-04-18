using UnityEngine;

public class attackHitboxController : MonoBehaviour
{
    public int attackPower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)

            // p.ApplyDamage(attackPower, 0.2f, true);
            p.TakeDamage(attackPower, 0.2f, true);
    }
}
