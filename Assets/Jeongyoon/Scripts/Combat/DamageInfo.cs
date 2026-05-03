using Unity.Netcode;
using UnityEngine;

public struct DamageInfo : INetworkSerializable
{
	public int damage;
	public float stunDuration;
	public bool knockback;
	public bool isCrit;
	public ulong attackerNetworkObjectId;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		serializer.SerializeValue(ref damage);
		serializer.SerializeValue(ref stunDuration);
		serializer.SerializeValue(ref knockback);
		serializer.SerializeValue(ref isCrit);
		serializer.SerializeValue(ref attackerNetworkObjectId);
	}

	// 근거리 공격
	public static DamageInfo Melee(int damage, bool isCrit, ulong attackerId) => new DamageInfo
	{
		damage = damage,
		stunDuration = 0.3f,
		knockback = true,
		isCrit = isCrit,
		attackerNetworkObjectId = attackerId
	};

	// 원거리 공격
	public static DamageInfo Range(int damage, bool isCrit) => new DamageInfo
	{
		damage = damage,
		stunDuration = 0f,
		knockback = true,
		isCrit = isCrit,
		attackerNetworkObjectId = 0
	};
}