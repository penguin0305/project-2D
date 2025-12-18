using UnityEngine;
using System.Collections;
public class enemyCombat : MonoBehaviour
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats ps = collision.GetComponent<PlayerStats>();
        if (ps != null)

            ps.TakeDamage(monserAttackPower);
    }

    public void OnHit(int damage, Transform playerTransform)
    {
        eController.currentHealth -= damage;//데미지 입는것에 제한없음 (무적시간 없음)
        
        if(eController.currentHealth<=0)
        {
            HistoryManager.Instance.AddDefeatCount();
            eController.die();
        }
        if (!isHit)//중복처리 방지
        {
            isHit = true;

            Knockback(playerTransform);
            StartCoroutine(BlinkCoroutine());
        }
        Debug.Log("TakeDamage");
    }
    public void OnHit(int damage)
    {
        eController.currentHealth -= damage;
        if (eController.currentHealth <= 0)
        {
            eController.die();
        }
    }
    void Knockback(Transform playertransform)
    {
        Vector2 dir = (transform.position - playertransform.position).normalized;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
