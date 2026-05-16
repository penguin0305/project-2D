using UnityEngine;
using Unity.Netcode;

public class attackHitboxController : NetworkBehaviour
{
    public int attackPower;

    void Start() { }
    void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            Debug.Log("서버없음");
            return;
        }

        var player = collision.GetComponentInParent<Player>();
        if (player != null)
        {
            var info = new DamageInfo
            {
                damage = attackPower,
                stunDuration = 0f,
                knockback = true,
                isCrit = false,
                attackerNetworkObjectId = NetworkObjectId
            };
            player.TakeDamage(info);
        }
    }
}