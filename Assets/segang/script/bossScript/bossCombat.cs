using UnityEngine;
using System.Collections;
using Unity.Netcode;
public class bossCombat : NetworkBehaviour
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
            var status = collision.GetComponent<PlayerStatus>();
            if (status != null)
            {
                status.ChangeHealth(-monserAttackPower);
            }
        }
    }

    public void OnHit(int damage, Transform playerTransform)
    {
        if (!IsServer) return;//고쳐야 될거 같음
        if (bossHP <= 0) return; // 죽으면 이후 데미지 무시
        bossHP -= damage;
        Debug.Log("TakeDamage");
        if (bossHP <= 0)
        {
            bossDie();
            return;
        }
        Knockback(playerTransform.position);
        HitClientRpc();
    }
    [ClientRpc]
    void HitClientRpc()
    {
        if (!isHit)
        {
            isHit = true;
            StartCoroutine(BlinkCoroutine());
        }
    }
    /*public void OnHit(int damage)
    {
        bossHP -= damage;
        if (bossHP <= 0)
        {
            bossDie();
        }
    }*/
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}