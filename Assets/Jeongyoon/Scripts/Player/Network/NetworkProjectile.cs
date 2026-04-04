/*
using Unity.Netcode;
using UnityEngine;

public class NetworkProjectile : NetworkBehaviour
{
	private Projectile projectile;
	private int damage;
	private ulong shooterNetworkObjectId;

	private void Awake()
	{
		projectile = GetComponent<Projectile>();
	}

	public void NetworkSetup(int damage, Vector2 direction, ulong shooterNetworkObjectId)
	{
		this.damage = damage;
		this.shooterNetworkObjectId = shooterNetworkObjectId;
		SetupClientRpc(damage, direction);
	}

	[ClientRpc]
	private void SetupClientRpc(int damage, Vector2 direction)
	{
		projectile.Setup(damage, direction);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!IsServer) return;

		var targetNetPlayer = collision.GetComponentInParent<NetworkPlayer>();
		if (targetNetPlayer != null && targetNetPlayer.NetworkObjectId != shooterNetworkObjectId)
		{
			targetNetPlayer.TakeDamageServerRpc(damage, 0f, true);
			GetComponent<NetworkObject>().Despawn();
		}
	}
}
*/
