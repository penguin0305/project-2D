// using UnityEngine;
using Unity.Netcode;
using UnityEngine;

// public class Projectile : MonoBehaviour
public class Projectile : NetworkBehaviour
{
	[SerializeField] private float speed = 8f;
	[SerializeField] private float lifetime = 2f;
	[SerializeField] private float visualOffset = -45f;
	private int damage;
	private bool isCrit;
	private bool alreadyHit = false;
	private Vector2 direction;
	private ulong shooterNetworkObjectId;

	// public void Setup(int damage, Vector2 direction)
	public void NetworkSetup(int damage, bool isCrit, Vector2 direction, ulong shooterNetworkObjectId)
	{
		this.shooterNetworkObjectId = shooterNetworkObjectId;
		SetupClientRpc(damage, isCrit, direction);
	}

	// [ClientRpc]
	[Rpc(SendTo.ClientsAndHost)]
	private void SetupClientRpc(int damage, bool isCrit, Vector2 direction)
	{
		Setup(damage, isCrit, direction);
	}

	public void Setup(int damage, bool isCrit, Vector2 direction)
	{
		this.damage = damage;
		this.isCrit = isCrit;
		this.direction = direction;

		float angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle + visualOffset);

		// Destroy(gameObject, lifetime);
		if (IsServer)
			Invoke(nameof(DespawnSelf), lifetime);
	}

	private void DespawnSelf()
	{
		GetComponent<NetworkObject>().Despawn();
	}

	void Update()
	{
		transform.Translate(this.direction * speed * Time.deltaTime, Space.World);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!IsServer) return;
		if (alreadyHit) return;

		var info = DamageInfo.Range(damage, isCrit);

		var damageable = collision.GetComponentInParent<IDamageable>();
		if (damageable != null)
		{
			// 발사한 플레이어 제외
			var targetPlayer = damageable as Player;
			if (targetPlayer != null && targetPlayer.NetworkObjectId == shooterNetworkObjectId)
				return;

			alreadyHit = true;
			damageable.TakeDamage(info);
			GetComponent<NetworkObject>().Despawn();
		}
	}
}