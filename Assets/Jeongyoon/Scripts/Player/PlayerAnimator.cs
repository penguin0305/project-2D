using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Player player;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		player = GetComponent<Player>();
	}

	private void LateUpdate()
	{
		CheckDeath();

		if (player.Status.CurrentHealth <= 0)
			return;

		if (player.IsOwner)
		{
			// 기존: 로컬 입력 직접 읽기 (그대로 유지)
			float moveX = player.Input.Move.x;
			animator.SetFloat("Speed", Mathf.Abs(moveX));
			animator.SetBool("IsGrounded", player.Motor.IsGrounded);

			if (moveX > 0f)
				spriteRenderer.flipX = false;
			else if (moveX < 0f)
				spriteRenderer.flipX = true;
		}
		else
		{
			// 기존: 로컬 입력만 읽어서 비소유자 애니메이션 안 됐음
			// → PlayerSync에서 동기화된 값 읽기
			animator.SetFloat("Speed", Mathf.Abs(player.Sync.moveX.Value));
			animator.SetBool("IsGrounded", player.Sync.isGrounded.Value);
			spriteRenderer.flipX = player.Sync.isFacingLeft.Value;
		}
	}

	public void DoMeleeAttack()
	{
		animator.SetTrigger("MeleeAttack");
	}

	public void DoRangeAttack()
	{
		animator.SetTrigger("RangeAttack");
	}

	public void DoDamageAnim()
	{
		animator.SetTrigger("Damage");
	}

	private void CheckDeath()
	{
		animator.SetBool("Death", player.Status.CurrentHealth <= 0);
	}
}