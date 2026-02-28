using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    [Header("공격패턴이 있는 모든 몬스터가 사용가능한 스크립트")]
    public float attackCooldown = 2f;   // 공격 쿨타임
    private float lastAttackTime = 0f;
    private bool playerInRange = false;
    private Transform player;
    Animator animator;
    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && player != null)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("isAttacking");
                lastAttackTime = Time.time;
            }
        }
    }
    /*void Attack()
    {
        Debug.Log("몬스터 공격!");
        // 플레이어에게 데미지 주기
        //공견판정 추가 해야함
    }*/ //애니메이션과 연동되는 공격판정을 위해 따로 구현 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            playerInRange = true;
            player = collision.transform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지해제");
            playerInRange = false;
            player = null;
        }
    }
}
