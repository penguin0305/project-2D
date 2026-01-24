using UnityEngine;

public class PlayerBombPlacer : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float cooldown = 10f;

    [Header("Placement")]
    [SerializeField] private Transform placePoint;     // PlayerMovement groundCheck
    [SerializeField] private float yOffset = 0.1f;

    private PlayerMovement movement;
    private float nextAvailableTime;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public void TryPlaceBomb()
    {
        if (Time.time < nextAvailableTime) return;

        if (movement == null || !movement.isGrounded) return;

        Vector3 pos = placePoint != null ? placePoint.position : transform.position;
        pos.y += yOffset;

        Instantiate(bombPrefab, pos, Quaternion.identity);
        nextAvailableTime = Time.time + cooldown;
    }

    public float CooldownRemaining => Mathf.Max(0f, nextAvailableTime - Time.time);
}
