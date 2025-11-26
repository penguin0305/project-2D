using UnityEngine;

public class enemyCombat : MonoBehaviour
{
    public enemyController eController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats ps = collision.GetComponent<PlayerStats>();
        if (ps != null)
            ps.TakeDamage(0);
    }

    public void OnHit(int damage)
    {
        eController.currentHealth -= damage;
        Debug.Log("TakeDamage");
    }

    private void Awake()
    {
        eController = GetComponent<enemyController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
