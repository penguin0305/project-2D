using UnityEngine;
using Unity.Netcode;
public class creatingProjectile : NetworkBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ThrowProjectile()
    {
        if (!IsServer) return;
        Transform target = FindClosestPlayer();
        if (target == null) return;
        Vector2 direction = (target.position - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<NetworkObject>().Spawn(true);
        // 투사체 이동 방향 전달
        proj.GetComponent<projectileController>().Init(direction, projectileSpeed);
    }
    Transform FindClosestPlayer()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject == null) continue;

            Transform playerTransform = client.PlayerObject.transform;

            float dist = Vector2.Distance(firePoint.position, playerTransform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = playerTransform;
            }
        }

        return closest;
    }
}
