using UnityEngine;

public class enemyJumpController : MonoBehaviour
{
    public float heightDiff = 1.5f;     // y좌표 차이 기준
    public float checkDuration = 1.2f;  // x축 이동 없을 때 점프까지 시간
    public float jumpForce = 5f;        // 점프 힘
    public float jumpCooldown = 5f;     // 점프 쿨타임

    private Rigidbody2D rb;
    public Transform player;
    private float lastXPos;//전 프레임의 x좌표
    public float timer = 0f;//x축 이동을 감지하는 타이머
    private float cooldownTimer = 0f;//점프 쿹타임 타이머
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastXPos = transform.position.x;

        

    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (player == null) return; // 플레이어 없으면 행동하지 않음

        float currentX = transform.position.x;
        bool canJump = cooldownTimer <= 0f;

        // 플레이어가 적보다 높은 위치일 때만 점프 
        if (canJump)
        {
            bool playerIsHigher = player.position.y > transform.position.y;

            if (playerIsHigher)
            {
                float diff = Mathf.Abs(player.position.y - transform.position.y);

                if (diff >= heightDiff)
                {
                    Jump();
                    cooldownTimer = jumpCooldown;
                    Debug.Log("플레이어가 더 높음");//테스트용 로그
                    return;
                }
            }
        }

        //  x축 이동 없음 검사
        /*if (Mathf.Abs(currentX - lastXPos) > 0.05f)
        {
            timer = 0f;
            lastXPos = currentX;
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= checkDuration && canJump)
            {
                Jump();
                Debug.Log($"x축 {timer}초이동안함");
                cooldownTimer = jumpCooldown;
                timer = 0f;
            }
        }*///->오류 때문에 이상하게 작동함 수정 예정
    }
    void Jump()//점프함수
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
