using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class enemyCombat : NetworkBehaviour, IDamageable
{
    public enemyController eController;
    [Header("AttackPower")]
    public int monserAttackPower = 0;//피격데미지=몬스터 공격력

    [Header("Knockback")]
    public float knockbackForce = 5f;//넉백 힘

    [Header("Blink")]
    public float blinkDuration = 0.2f;//깜빡임 지속시간
    public float blinkInterval = 0.05f;//깜빡임 간격

    Rigidbody2D rb;
    SpriteRenderer sr;

    bool isHit = false;//피격처리 중복 방지

    //네트워크 수정
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
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                var info = new DamageInfo
                {
                    damage = monserAttackPower,
                    stunDuration = 0f,
                    knockback = true,
                    isCrit = false,
                    attackerNetworkObjectId = NetworkObjectId
                };
                player.TakeDamage(info);
            }
        }
    }

    // IDamageable 구현
    public void TakeDamage(DamageInfo info)
    {
        if (!IsServer)
        {
            TakeDamageServerRpc(info);
            return;
        }

        ApplyDamage(info);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void TakeDamageServerRpc(DamageInfo info)
    {
        ApplyDamage(info);
    }

    private void ApplyDamage(DamageInfo info)
    {
        if (eController.currentHealth <= 0) return;

        eController.currentHealth -= info.damage;

        // FloatingDamage 전파
        ShowFloatingDamageRpc(info.damage, transform.position);

        if (eController.currentHealth <= 0)
        {
            if (NetworkHistoryManager.Instance != null)
                NetworkHistoryManager.Instance.AddDefeatCountServerRpc(Unity.Netcode.NetworkManager.Singleton.LocalClientId);

            eController.die();
            return;
        }

        if (info.knockback && info.attackerNetworkObjectId != 0)
        {
            var attackerObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[info.attackerNetworkObjectId];
            if (attackerObj != null)
                Knockback(attackerObj.transform);
        }

        if (!isHit)
        {
            isHit = true;
            HitClientRpc();
        }

        Debug.Log("TakeDamage");
    }

    // 기존 OnHit 유지 (하위 호환)
    public void OnHit(int damage, Transform playerTransform)
    {
        var info = new DamageInfo
        {
            damage = damage,
            knockback = true,
            isCrit = false,
            attackerNetworkObjectId = 0
        };
        TakeDamage(info);
    }

    public void OnHit(int damage)
    {
        var info = new DamageInfo { damage = damage };
        TakeDamage(info);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ShowFloatingDamageRpc(int damage, Vector3 position)
    {
        FloatingDamageManager.Instance?.Show(damage, position, FloatingDamageType.Normal);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void HitClientRpc()
    {
        if (!isHit)
        {
            isHit = true;
            StartCoroutine(BlinkCoroutine());
        }
    }

    void Knockback(Transform playerTransform)
    {
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        rb.linearVelocity = Vector2.zero;//기존 속도 제거
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
    }

    IEnumerator BlinkCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        sr.enabled = true;//깜빡임 종료후 렌더러 활성화
        isHit = false;
    }

    private void Awake()
    {
        eController = GetComponent<enemyController>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start() { }
    void Update() { }
}