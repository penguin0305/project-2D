/*
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerCombat : PlayerCombat
{
	protected override void Awake()
	{
		base.Awake();
	}

	public override void SpawnProjectile()
	{
		SpawnProjectileServerRpc(muzzle.position, player.Motor.IsFacingRight);
	}

	[ServerRpc]
	private void SpawnProjectileServerRpc(Vector3 spawnPosition, bool facingRight)
	{
		GameObject arrowObject = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
		arrowObject.GetComponent<NetworkObject>().Spawn(true);

		Vector2 shootDir = facingRight ? Vector2.right : Vector2.left;
		ulong shooterId = GetComponent<NetworkObject>().NetworkObjectId;
		arrowObject.GetComponent<NetworkProjectile>().NetworkSetup(player.Status.RangeATK, shootDir, shooterId);
	}
}
*/
