using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
	[Header("Melee Attack")]
	[SerializeField] private Collider2D meleeCollider;
	[SerializeField] private float attackCooldown = 0.5f;
	private float lastMeleeAttackTime;
	public bool isMeleeAttacking;
	private Vector2 meleeColliderBaseOffset;

	[Header("Range Attack")]
	[SerializeField] private GameObject arrowPrefab;
	[SerializeField] private Transform muzzle;
	[SerializeField] private float rangeAttackCooldown = 0.4f;
	private float lastRangeAttackTime;

	public enum CombatMode
	{
		Melee,
		Range
	}

	public CombatMode CurrentMode { get; private set; } = CombatMode.Melee;

	private PlayerAnimator animator;
	private PlayerAudio audio;
	private Player player;

	private void Awake()
	{
		animator = GetComponent<PlayerAnimator>();
		audio = GetComponent<PlayerAudio>();
		player = GetComponent<Player>();
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

		if (animator.IsFacingRight)
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
		audio.PlayMelee();
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

	public void SwitchMode()
	{
		CurrentMode = (CurrentMode == CombatMode.Melee) ? CombatMode.Range : CombatMode.Melee;
	}
	public void TryRangeAttack()
	{
		if (Time.time < lastRangeAttackTime + rangeAttackCooldown)
			return;
		
		lastRangeAttackTime = Time.time;
		animator.DoMeleeAttack(); // tmp
		audio.PlayMelee(); // tmp
		SpawnProjectile();
	}

	public void SpawnProjectile()
	{
		GameObject arrowObject = Instantiate(arrowPrefab, muzzle.position, Quaternion.identity);
		Projectile arrow = arrowObject.GetComponent<Projectile>();

		Vector2 shootDir = player.Motor.IsFacingRight ? Vector2.right : Vector2.left;
		arrow.Setup(player.Status.RangeATK, shootDir);
	}
}
