using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour
{
	[Header("Physics Settings")]
	[SerializeField] private float defaultGravity = 3f;

	[Header("Ground Detection")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.2f;
	[SerializeField] private LayerMask groundLayer;

	private Rigidbody2D rb;
	public bool IsGrounded { get; private set; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		if (defaultGravity <= 0f)
			defaultGravity = rb.gravityScale;
	}

	public void DetectGrounded()
	{
		IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
	}
	
	public void SetGravityScale(float gravity) => rb.gravityScale = gravity;
	public void RestoreDefaultGravity() => rb.gravityScale = defaultGravity;
	
	public void SetVelocityX(float x) => rb.linearVelocityX = x;
	public void SetVelocityY(float y) => rb.linearVelocityY = y;
	public void SetVelocity(float x, float y) => rb.linearVelocity = new Vector2(x, y);
	public void SetVelocity(Vector2 vec) => rb.linearVelocity = vec;

	public void StopHorizontal() => rb.linearVelocityX = 0f;
	public void StopVertical() => rb.linearVelocityY = 0f;
	public void StopAll() => rb.linearVelocity = Vector2.zero;

	public void ApplyImpulse(Vector2 impulse) => rb.AddForce(impulse, ForceMode2D.Impulse);
}
