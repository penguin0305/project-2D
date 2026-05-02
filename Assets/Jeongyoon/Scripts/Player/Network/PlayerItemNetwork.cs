using Unity.Netcode;
 
public struct PlayerItemNetwork : INetworkSerializable
{
	public int eid;
	public int enhance_level;
	public int dup_count;
	public int enhance_fail_count;
 
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		serializer.SerializeValue(ref eid);
		serializer.SerializeValue(ref enhance_level);
		serializer.SerializeValue(ref dup_count);
		serializer.SerializeValue(ref enhance_fail_count);
	}
}
 