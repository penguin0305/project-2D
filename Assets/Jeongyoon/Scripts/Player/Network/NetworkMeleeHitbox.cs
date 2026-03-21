using UnityEngine;

public class NetworkMeleeHitbox : MonoBehaviour
{
	private Player player;
	private NetworkPlayer networkPlayer;

	private void Awake()
	{
		player = GetComponentInParent<Player>();
		networkPlayer = GetComponentInParent<NetworkPlayer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!networkPlayer.IsOwner) return;
		if (!player.Combat.isMeleeAttacking) return;

		// 기존 적 처리
		if (collision.CompareTag("Enemy"))
		{
			var enemy = collision.GetComponent<enemyCombat>();
			if (enemy)
				enemy.OnHit(player.Status.MeleeATK, transform);
		}

		// 플레이어 PvP
		var targetNetPlayer = collision.GetComponentInParent<NetworkPlayer>();
		if (targetNetPlayer != null && targetNetPlayer != networkPlayer)
		{
			targetNetPlayer.TakeDamageServerRpc(player.Status.MeleeATK, 0.3f, true);
		}
	}
}
