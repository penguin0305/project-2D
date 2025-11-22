using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
	[Header("Melee Attack")]
	[SerializeField] private Collider2D meleeCollider;
	[SerializeField] private float attackCooldown = 0.5f;
	private float lastMeleeAttackTime;
	public bool isMeleeAttacking;
	private Vector2 meleeColliderBaseOffset;

	private PlayerAnimator animator;

	private void Awake()
	{
		animator = GetComponent<PlayerAnimator>();
		meleeColliderBaseOffset = meleeCollider.offset;
	}

	private void Start()
	{
		meleeCollider.enabled = false;
	}

	private void Update()
	{
		UpdateMeleeColliderDirection();
	}

	private void UpdateMeleeColliderDirection()
	{
		Vector2 offset = meleeColliderBaseOffset;

		if (animator.isFacingRight)
			offset.x = Mathf.Abs(meleeColliderBaseOffset.x);
		else
			offset.x = -Mathf.Abs(meleeColliderBaseOffset.x);
		
		meleeCollider.offset = offset;
	}

	public void TryMeleeAttack()
	{
		if (Time.time < lastMeleeAttackTime + attackCooldown)
			return;

		lastMeleeAttackTime = Time.time;
		animator.DoMeleeAttack();
		StartCoroutine(MeleeAttack());
	}

	private IEnumerator MeleeAttack()
	{
		isMeleeAttacking = true;
		meleeCollider.enabled = true;

		yield return new WaitForSeconds(0.2f);

		meleeCollider.enabled = false;
		isMeleeAttacking = false;
	}
}
