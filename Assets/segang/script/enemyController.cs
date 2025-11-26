using UnityEngine;

public class enemyController : MonoBehaviour
{
    public enemyCombat eCombat;
    private itemDropController dropper;
    Animator animator;
    private float timer = 0f;
    private float walkTime = 2f;       // 걷는 시간
    private float idleTime = 0.5f;     //x축 이동 바꾸기전 가만히 있는 시간
    private bool movingRight = true;
    private bool isWalking = true;
    [Header("기본 설정")]
    public float moveSpeed = 2f;         // 이동 속도
    public float detectRange = 5f;       // 플레이어 감지 거리
    public int maxHealth = 10;          // 최대 체력


    [Header("상태")]
    public int currentHealth;//현재체력
    public Transform player;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;//처리 판정용 변수
    private bool isChasing = false;//->현재 추적 기능은 안넣음
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
       
        spriteRenderer = GetComponent<SpriteRenderer>();
        dropper = GetComponent<itemDropController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        patrol();
        if (Input.GetKeyDown("v"))//추격시작 테스트용 v키를 누르면 추적 시작->현재 추적 기능 없음
        {
            Debug.Log("추적시작");//작동확인용 로그
        }
        if (currentHealth <= 0)//->k누르면 즉시 처치됨 확인용 코드
        {
            die();
        }
    }
    private void patrol()
    {
        timer += Time.deltaTime;

        if (isWalking)
        {
            // ----- 걷는 중 -----
            float dir = movingRight ? 1f : -1f;

            transform.position += new Vector3(dir * moveSpeed * Time.deltaTime, 0, 0);

            if (timer >= walkTime)        // 걷는 시간 끝
            {
                isWalking = false;        // Idle 상태로 변경
                timer = 0f;
            }
        }
        else
        {
            // ----- Idle 중 -----
            if (timer >= idleTime)        // Idle 종료
            {
                // 방향 전환
                movingRight = !movingRight;
                spriteRenderer.flipX = !spriteRenderer.flipX;

                // 다시 걷기 시작
                isWalking = true;
                timer = 0f;
            }
        }

        // Animator에도 전달 가능
        animator.SetBool("isWalking", isWalking);
    }//맨 위 walkTime초만큼 걷고 idleTime만큼 가만히있음 이후 방향을 바꾸고 동일한 과정 실행

    private void chasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // 좌우 방향 전환
        if (direction.x != 0)
            spriteRenderer.flipX = direction.x < 0;
    }//현재 추적기능 없음
    private void die()
    {
        Debug.Log($"{gameObject.name} 사망");
        dropper.DropItems();//아이템 드랍 함수 인스펙터창에서 프리팹과 드랍가중치 설정가능
        Destroy(gameObject, 0.1f);
    }
}
