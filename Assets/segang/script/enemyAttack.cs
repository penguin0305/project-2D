using UnityEngine;
using Unity.Netcode;

public class EnemyAttack : NetworkBehaviour
{
    [Header("공격패턴이 있는 모든 몬스터가 사용가능한 스크립트")]
    public float attackCooldown = 2f;

    private float lastAttackTime = 0f;
    private bool playerInRange = false;
    private Transform player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        // 서버만 공격 로직 실행
        if (!IsServer) return;

        if (playerInRange && player != null)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackClientRpc();  // 클라이언트들에게 공격 알림
                lastAttackTime = Time.time;
            }
        }
    }

    // 모든 클라이언트에서 애니메이션 실행
    [ClientRpc]
    void AttackClientRpc()
    {
        animator.SetTrigger("isAttacking");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return; // 서버만 감지

        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지");
            playerInRange = true;
            player = collision.transform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsServer) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지해제");
            playerInRange = false;
            player = null;
        }
    }
}
