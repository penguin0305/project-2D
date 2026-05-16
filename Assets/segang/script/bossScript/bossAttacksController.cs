using UnityEngine;
using Unity.Netcode;
public class bossAttacksController : NetworkBehaviour
{
    Animator animator;
    [Header("보스용Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public int projectileCount = 8;
    public void playRandomAttack()
    {
        if (!IsServer) return;
        int rand = Random.Range(0, 2);
        PlayAttackClientRpc(rand);
    }
    [ClientRpc]
    void PlayAttackClientRpc(int attackIndex)
    {
        switch (attackIndex)
        {
            case 0:
                attack1();
                break;
            case 1:
                attack2();
                break;
        }
    }
    void attack1() 
    {
        animator.SetTrigger("attack1");
        Debug.Log("공격1번"); 
    }
    void attack2()
    {
        animator.SetTrigger("attack2");
        Debug.Log("공격2번");
    }
    void Fire8Directions()
    {
        if (!IsServer) return;
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = angleStep * i;

            float rad = angle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(
                Mathf.Cos(rad),
                Mathf.Sin(rad));
            float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, rotZ);
            GameObject proj = Instantiate(
                projectilePrefab,
                transform.position,
                rot); //rot추가

            projectileForBoss projectile =
                proj.GetComponent<projectileForBoss>();

            projectile.Init(dir, projectileSpeed);

            proj.GetComponent<NetworkObject>().Spawn();
        }
    }
    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
