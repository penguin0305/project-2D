using System.Collections;
using Unity.Netcode;
using UnityEngine;

//public class PlayerCombat : MonoBehaviour
public class PlayerCombat : NetworkBehaviour
{
	[Header("Melee Attack")]
	[SerializeField] private Collider2D meleeCollider;
	[SerializeField] private float attackCooldown = 0.5f;
	private float lastMeleeAttackTime;
	public bool isMeleeAttacking;
	private Vector2 meleeColliderBaseOffset;

	[Header("Range Attack")]
	[SerializeField] private GameObject arrowPrefab;
	// [SerializeField] protected GameObject arrowPrefab;
	[SerializeField] private Transform muzzle;
	// [SerializeField] protected Transform muzzle;
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
	// protected Player player;

	private void Awake()
	// protected virtual void Awake()
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

		//if (animator.IsFacingRight)
		if (player.Motor.IsFacingRight)
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
		animator.DoRangeAttack(); // tmp
		audio.PlayMelee(); // tmp
		SpawnProjectile();
	}

	// public virtual void SpawnProjectile()
	public void SpawnProjectile()
	{
		if (IsServer)
			SpawnProjectileOnServer(muzzle.position, player.Motor.IsFacingRight);
		else
			SpawnProjectileServerRpc(muzzle.position, player.Motor.IsFacingRight);
	}

	[Rpc(SendTo.Server)]
	private void SpawnProjectileServerRpc(Vector3 spawnPosition, bool facingRight)
	{
		SpawnProjectileOnServer(spawnPosition, facingRight);
	}

	private void SpawnProjectileOnServer(Vector3 spawnPosition, bool facingRight)
	{
		GameObject arrowObject = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
		arrowObject.GetComponent<NetworkObject>().Spawn(true);

		Vector2 shootDir = facingRight ? Vector2.right : Vector2.left;
		ulong shooterId = GetComponent<NetworkObject>().NetworkObjectId;

		arrowObject.GetComponent<Projectile>().NetworkSetup(player.Status.RangeATK, shootDir, shooterId);
	}
}
