using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float explodeDelay = 3f;
    [SerializeField] private float explosionVisibleTime = 0.1f;

    [Header("Explosion")]
    [SerializeField] private float explodeRadius = 2.5f;
    [SerializeField] private int damageToEnemy = 5;
    [SerializeField] private int damageToPlayer = 1;

    [Header("Layers")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask playerLayer;

    private SpriteRenderer sr;
    private Vector3 baseScale;
    private Color baseColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
        baseColor = sr.color;
    }

    private void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explodeDelay);
        Explode();
    }

    private void Explode()
    {
        StartCoroutine(ShowExplosionVisual());

        Collider2D[] enemyHits = Physics2D.OverlapCircleAll(
            transform.position,
            explodeRadius,
            enemyLayer
        );

        foreach (var col in enemyHits)
        {
            var enemy = col.GetComponent<enemyCombat>();
            if (enemy != null)
            {
                Debug.Log(
                    $"[Bomb] Enemy hit: {col.name}, Damage = {damageToEnemy}",
                    col
                );

                enemy.OnHit(damageToEnemy);
            }
        }

        Collider2D[] playerHits = Physics2D.OverlapCircleAll(
            transform.position,
            explodeRadius,
            playerLayer
        );

        foreach (var col in playerHits)
        {
            var playerStats = col.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                Debug.Log(
                    $"[Bomb] Player hit: {col.name}, Damage = {damageToPlayer}",
                    col
                );

                playerStats.TakeDamage(damageToPlayer);
            }
        }
    }

    private IEnumerator ShowExplosionVisual()
    {
        sr.color = Color.red;

        float baseDiameter = baseScale.x;
        float explosionDiameter = explodeRadius * 2f;
        float scaleMultiplier = explosionDiameter / baseDiameter;

        transform.localScale = baseScale * scaleMultiplier;

        yield return new WaitForSecondsRealtime(explosionVisibleTime);
        Destroy(gameObject);
    }
}
