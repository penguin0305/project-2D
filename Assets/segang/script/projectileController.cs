using UnityEngine;
using Unity.Netcode;
public class projectileController : NetworkBehaviour
{
    private Vector2 direction;
    private float speed;
    [Header("적 투사체용 스크립트 회전오프셋은 기본 이미지가 오른쪽방향이면 0")]
    public float rotationOffset = 0f;   // 에셋 방향 보정
    public float lifeTime = 5f;         // 투사체 유지 시간
    public int projectileDamage;
    private float spawnTime;
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
        // 방향에 맞게 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            Debug.Log("서버없음");
            return;
        }

        if (collision.CompareTag("Player"))
        {
            Debug.Log("충돌");
            var status = collision.GetComponent<PlayerStatus>();
            if (status != null)
            {
                status.ChangeHealth(-projectileDamage);
            }
        }
    }
}
