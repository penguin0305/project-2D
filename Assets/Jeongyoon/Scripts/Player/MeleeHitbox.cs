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

		// 기존 적 처리
		if (collision.CompareTag("Enemy"))
		{
			var enemy = collision.GetComponent<enemyCombat>();
			if (enemy)
				enemy.OnHit(damage, transform);
		}

		if (collision.CompareTag("Boss"))
		{
			var boss = collision.GetComponent<bossCombat>();
			if (boss)
				boss.OnHit(damage, transform);
		}
		// 플레이어 PvP
		// NetworkMeleeHitbox: var targetNetPlayer = collision.GetComponentInParent<NetworkPlayer>();
		var targetPlayer = collision.GetComponentInParent<Player>();
		if (targetPlayer != null && targetPlayer != player)
		{
			// NetworkMeleeHitbox: targetNetPlayer.TakeDamageServerRpc(...)
			targetPlayer.TakeDamageServerRpc(damage, 0.3f, true, isCrit);
		}
	}
}