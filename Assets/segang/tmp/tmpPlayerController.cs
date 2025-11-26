using UnityEngine;

public class tmpPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isJumping = false; // 공중인지 여부 체크

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // --- 이동 ---
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D, ← →
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // --- 점프 ---
        // y속도가 거의 0이면 바닥에 있다고 간주
        bool isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.01f;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
