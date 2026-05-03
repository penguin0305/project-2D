using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
	private Player player;

	private void Awake()
	{
		player = GetComponentInParent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// NetworkMeleeHitbox: if (!networkPlayer.IsOwner) return;
		if (!player.IsOwner) return;
		if (!player.Combat.isMeleeAttacking) return;

		// 치명타 계산 (공격자 기준)
		int damage = player.Status.MeleeATK;
		bool isCrit = UnityEngine.Random.value < Mathf.Clamp(player.Status.CritRate, 0f, 1f);
		if (isCrit)
			damage = Mathf.Max(1, Mathf.RoundToInt(damage * player.Status.CritDamage));

		ulong attackerId = player.NetworkObjectId;
		var info = DamageInfo.Melee(damage, isCrit, attackerId);

		var damageable = collision.GetComponentInParent<IDamageable>();
		if (damageable != null && !ReferenceEquals(damageable, player))
			damageable.TakeDamage(info);
	}
}