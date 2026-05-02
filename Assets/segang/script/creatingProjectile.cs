using UnityEngine;

public class creatingProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 8f;

    private Transform player;
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector2 direction = (player.position - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 투사체 이동 방향 전달
        proj.GetComponent<projectileController>().Init(direction, projectileSpeed);
    }
}
