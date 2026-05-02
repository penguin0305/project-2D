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
		animator.DoMeleeAttack();           // 로컬 Owner 실행
		player.Sync.MeleeAttackAnimRpc();   // 기존: 없었음, 비소유자에게 전파
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
		animator.DoRangeAttack();           // 로컬 Owner 실행
		player.Sync.RangeAttackAnimRpc();   // 기존: 없었음, 비소유자에게 전파
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

		// 치명타 계산 (공격자 기준)
		int damage = player.Status.RangeATK;
		bool isCrit = UnityEngine.Random.value < Mathf.Clamp(player.Status.CritRate, 0f, 1f);
		if (isCrit)
			damage = Mathf.Max(1, Mathf.RoundToInt(damage * player.Status.CritDamage));

		arrowObject.GetComponent<Projectile>().NetworkSetup(damage, isCrit, shootDir, shooterId);
	}
}