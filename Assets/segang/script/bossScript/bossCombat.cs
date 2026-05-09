using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class bossCombat : NetworkBehaviour, IDamageable
{
    private bossStateMachine stateMachine;
    [Header("AttackPower")]
    public int monserAttackPower = 0;//보스 닿을때 데미지

    [Header("Knockback")]
    public float knockbackForce = 5f;//넉백힘

    [Header("체력")]
    public int bossHP = 5;//보스체력

    [Header("Blink")]
    public float blinkDuration = 0.2f;//깜빡임 지속시간
    public float blinkInterval = 0.05f;//깜빡임 간격

    Rigidbody2D rb;
    SpriteRenderer sr;

    bool isHit = false;//피격중복 방지

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
        if (!IsServer) return;
        if (bossHP <= 0) return;

        bossHP -= info.damage;
        Debug.Log("TakeDamage");

        // FloatingDamage 전파
        ShowFloatingDamageRpc(info.damage, transform.position, info.isCrit);

        if (bossHP <= 0)
        {
            bossDie();
            return;
        }

        if (info.knockback && info.attackerNetworkObjectId != 0)
        {
            var attackerObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[info.attackerNetworkObjectId];
            if (attackerObj != null)
                Knockback(attackerObj.transform.position);
        }

        HitClientRpc();
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
        if (IsServer)
            Knockback(playerTransform.position);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ShowFloatingDamageRpc(int damage, Vector3 position, bool isCrit)
    {
        FloatingDamageType type = isCrit ? FloatingDamageType.Crit : FloatingDamageType.Normal;
        FloatingDamageManager.Instance?.Show(damage, position, type);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void HitClientRpc()
    {
        if (!isHit)
        {
            isHit = true;
            StartCoroutine(BlinkCoroutine());
        }
    }

    void Knockback(Vector3 playerPos)
    {
        Vector2 dir = (transform.position - playerPos).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
    }

    void bossDie()
    {
        if (stateMachine == null) return;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; //물리 끄기
        stateMachine.deadSignal();
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

        sr.enabled = true;
        isHit = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        stateMachine = GetComponent<bossStateMachine>();
    }

    void Start()
    {

    }

    void Update()
    {

    }
}