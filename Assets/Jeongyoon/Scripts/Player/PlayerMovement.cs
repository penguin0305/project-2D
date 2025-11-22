using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Move")]
	[SerializeField] private float moveSpeed = 5f;
	public Vector2 moveInput;

	[Header("Jump")]
	[SerializeField] private float jumpForce = 12f;
	[SerializeField] private int maxJumpCount = 2;
	private int currentJumpCount;
	private bool jumpRequested;

	[Header("GroundCheck")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.2f;
	[SerializeField] private LayerMask groundLayer;
	public bool isGrounded;

	[Header("Dash")]
	[SerializeField] private float dashSpeedMultiplier = 1.5f;
	private bool isDashing;

	[Header("Ladder")]
	[SerializeField] private float climbSpeed = 4f;
	private bool isOnLadderZone = false;
	private bool isClimbing = false;

	private float originalGravity;

	private Rigidbody2D rb;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		originalGravity = rb.gravityScale;
	}

	private void Start()
	{
		currentJumpCount = 0;
		jumpRequested = false;
	}

	private void Update()
	{
		CheckGround();

		if (jumpRequested)
		{
			Jump();
			jumpRequested = false;
		}
	}

	private void FixedUpdate()
	{
		if (isClimbing)
		{
			MoveVertical();
			return;
		}
		MoveHorizontal();
	}

	private void CheckGround()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		if (isGrounded)
			currentJumpCount = 0;
	}

	public void RequestMove(Vector2 input)
	{
		moveInput = input;
	}

	public void RequestJump()
	{
		if (isClimbing)
		{
			StopClimb();
			return;
		}
		jumpRequested = true;
	}

	public void RequestDash(bool isPressed)
	{
		isDashing = isPressed;
	}

	private void MoveHorizontal()
	{
		float speed = moveSpeed;

		if (isDashing)
			speed *= dashSpeedMultiplier;
		rb.linearVelocityX = moveInput.x * speed;
	}

	private void MoveVertical()
	{
		rb.linearVelocityY = moveInput.y * climbSpeed;
	}

	private void Jump()
	{
		if (isGrounded || currentJumpCount < maxJumpCount)
		{
			rb.linearVelocityY = jumpForce;
			currentJumpCount++;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Ladder ladder = other.GetComponent<Ladder>();
		if (ladder != null)
			isOnLadderZone = true;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		Ladder ladder = other.GetComponent<Ladder>();
		if (ladder != null)
		{
			isOnLadderZone = false;

			if (isClimbing)
				StopClimb();
		}
	}

	public void StartClimb()
	{
		if (!isOnLadderZone)
			return;

		isClimbing = true;
		rb.gravityScale = 0f;
		rb.linearVelocity = Vector2.zero;
	}

	public void StopClimb()
	{
		isClimbing = false;
		rb.gravityScale = originalGravity;
	}
}
