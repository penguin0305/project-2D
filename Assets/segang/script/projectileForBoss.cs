using UnityEngine;
using Unity.Netcode;
public class projectileForBoss : NetworkBehaviour
{
    private Vector2 direction;
    private float speed;
    public float lifeTime = 5f;         // 투사체 유지 시간
    public int projectileDamage;
    private float spawnTime;
    private bool alreadyHit = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer && Time.time >= spawnTime + lifeTime)
        {
            NetworkObject.Despawn();
        }
    }
    private void FixedUpdate()
    {
        if (!IsServer) return;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
    public void Init(Vector2 dir, float spd)
    {
        direction = dir.normalized;
        speed = spd;

        spawnTime = Time.time;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            return;
        }
        if (alreadyHit) return;
        var player = collision.GetComponentInParent<Player>();
        if (player != null)
        {
            Debug.Log("충돌");
            var info = new DamageInfo
            {
                damage = projectileDamage,
                stunDuration = 0f,
                knockback = true,
                isCrit = false,
                attackerNetworkObjectId = NetworkObjectId
            };
            alreadyHit = true;
            player.TakeDamage(info);
            NetworkObject.Despawn();
        }
    }
}
