using UnityEngine;
using Unity.Netcode;
public class turningOffCollision : NetworkBehaviour 
{
    public Rigidbody2D rb;

    [Header("바닥 충돌용 콜라이더")]
    public Collider2D solidCollider;

    int groundLayer;

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer("Ground");  
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool landed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        if (landed) return;

        if (collision.gameObject.layer == groundLayer)
        {
            landed = true;

            // 물리 멈춤
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // 더 이상 물리 계산 안함
            rb.bodyType = RigidbodyType2D.Kinematic;

            // 일반 충돌 제거
            solidCollider.enabled = false;
        }
    }
}
