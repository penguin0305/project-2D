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
	private bool alreadyHit = false;
	private Vector2 direction;
	private ulong shooterNetworkObjectId;

	// public void Setup(int damage, Vector2 direction)
	public void NetworkSetup(int damage, Vector2 direction, ulong shooterNetworkObjectId)
	{
		this.shooterNetworkObjectId = shooterNetworkObjectId;
		SetupClientRpc(damage, direction);
	}

	// [ClientRpc]
	[Rpc(SendTo.ClientsAndHost)]
	private void SetupClientRpc(int damage, Vector2 direction)
	{
		Setup(damage, direction);
	}

	public void Setup(int damage, Vector2 direction)
	{
		this.damage = damage;
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

		// 기존: Enemy 처리
		if (collision.CompareTag("Enemy"))
		{
			var dummy = collision.GetComponent<enemyCombat>();
			if (dummy)
			{
				alreadyHit = true;
				dummy.OnHit(damage, transform);
				// Destroy(gameObject);
				GetComponent<NetworkObject>().Despawn();
			}
		}

		// 추가: 플레이어 PvP 처리
		var targetPlayer = collision.GetComponentInParent<Player>();
		if (targetPlayer != null && targetPlayer.NetworkObjectId != shooterNetworkObjectId)
		{
			alreadyHit = true;
			// targetPlayer.TakeDamageServerRpc(damage, 0f, true); // 서버에서 ServerRpc 호출 불가
			targetPlayer.TakeDamage(damage, 0f, true);
			GetComponent<NetworkObject>().Despawn();
		}
	}
}
