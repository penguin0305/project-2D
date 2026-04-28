using UnityEngine;

public class LocalPlayerAnimator : MonoBehaviour
{
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private LocalPlayer player;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		player = GetComponent<LocalPlayer>();
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

	private void UpdateFacing()
	{
		float moveX = player.Input.Move.x;

		if (moveX > 0f)
			spriteRenderer.flipX = false;
		else if (moveX < 0f)
			spriteRenderer.flipX = true;
	}
}
