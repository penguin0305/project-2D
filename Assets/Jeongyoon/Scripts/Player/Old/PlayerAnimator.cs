using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Player player;

	private float moveSpeed;
	public bool IsFacingRight { get; private set; } = true;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		player = GetComponent<Player>();
	}

	private void LateUpdate()
	{
		CheckSpeed();
		CheckGrounded();

		UpdateFacing();
	}

	private void CheckSpeed()
	{
		float moveInputX = Mathf.Abs(player.Input.Move.x);
                animator.SetFloat("Speed", moveInputX);
	}

	private void CheckGrounded()
	{
		animator.SetBool("IsGrounded", player.Motor.IsGrounded);
	}

	public void DoMeleeAttack()
	{
		animator.SetTrigger("MeleeAttack");
	}

	private void UpdateFacing()
	{
		float moveX = player.Input.Move.x;

		if (moveX > 0f)
		{
			spriteRenderer.flipX = false;
			IsFacingRight = true;
		}
		else if (moveX < 0f)
		{
			spriteRenderer.flipX = true;
			IsFacingRight = false;
		}
	}
}