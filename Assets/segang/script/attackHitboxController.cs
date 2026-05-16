using UnityEngine;
using Unity.Netcode;
public class attackHitboxController : NetworkBehaviour
{
    public int attackPower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
        {
            return;
        }
        var status = collision.GetComponent<PlayerStatus>();
        if (status != null)
        {
            status.ChangeHealth(-attackPower);
        }
    }
}
