using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Player player;
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
		CheckDeath();
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

	public void DoRangeAttack()
	{
		animator.SetTrigger("RangeAttack");
	}

	public void CheckDeath()
	{
		animator.SetBool("Death", player.Status.CurrentHealth <= 0);
	}

	public void DoDamageAnim()
	{
		animator.SetTrigger("Damage");
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