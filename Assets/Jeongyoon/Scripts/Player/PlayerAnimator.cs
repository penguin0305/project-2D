using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	private Animator animator;
	private PlayerMovement movement;
	private SpriteRenderer spriteRenderer;

	private float moveSpeed;
	public bool isFacingRight;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		movement = GetComponent<PlayerMovement>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void LateUpdate()
	{
		CheckSpeed();
		CheckGrounded();

		UpdateFacing();
	}

	private void CheckSpeed()
	{
		moveSpeed = Mathf.Abs(movement.moveInput.x);
		animator.SetFloat("Speed", moveSpeed);
	}

	private void CheckGrounded()
	{
		animator.SetBool("IsGrounded", movement.isGrounded);
	}

	public void DoMeleeAttack()
	{
		animator.SetTrigger("MeleeAttack");
	}

	private void UpdateFacing()
	{
		if (movement.moveInput.x > 0f)
		{
			spriteRenderer.flipX = false;
			isFacingRight = true;
		}
		else if (movement.moveInput.x < 0f)
		{
			spriteRenderer.flipX = true;
			isFacingRight = false;
		}
	}
}