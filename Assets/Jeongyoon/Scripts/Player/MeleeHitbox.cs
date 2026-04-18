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

		// 기존 적 처리
		if (collision.CompareTag("Enemy"))
		{
			var enemy = collision.GetComponent<enemyCombat>();
			if (enemy)
				enemy.OnHit(player.Status.MeleeATK, transform);
		}

		// 플레이어 PvP
		// NetworkMeleeHitbox: var targetNetPlayer = collision.GetComponentInParent<NetworkPlayer>();
		var targetPlayer = collision.GetComponentInParent<Player>();
		if (targetPlayer != null && targetPlayer != player)
		{
			// NetworkMeleeHitbox: targetNetPlayer.TakeDamageServerRpc(...)
			targetPlayer.TakeDamageServerRpc(player.Status.MeleeATK, 0.3f, true);
		}
	}
}
