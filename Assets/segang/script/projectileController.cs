using UnityEngine;
using Unity.Netcode;

public class projectileController : NetworkBehaviour
{
    private Vector2 direction;
    private float speed;
    [Header("이 발사체의 스크립트 회전오프셋으로 기본 이미지가 오른쪽을 바라보면 0")]
    public float rotationOffset = 0f;   // 회전 보정 오프셋
    public float lifeTime = 5f;         // 발사체 생존 시간
    public int projectileDamage;
    private float spawnTime;

    void Start() { }

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

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            return;
        }

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
            player.TakeDamage(info);
            NetworkObject.Despawn();
        }
    }
}