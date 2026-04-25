using UnityEngine;

public class tesCreatingProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 8f;

    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("v"))//추격시작 테스트용 v키를 누르면 추적 시작->현재 추적 기능 없음
        {
            ThrowProjectile();
        }
    }
    public void ThrowProjectile()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 투사체 이동 방향 전달
        proj.GetComponent<projectileController>().Init(direction, projectileSpeed);
    }
}
