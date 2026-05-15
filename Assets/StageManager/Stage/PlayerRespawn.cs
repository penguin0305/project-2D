using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform spawnPoint;

    private string deathZoneTag = "DeathZone";

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(deathZoneTag))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = spawnPoint.position;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
