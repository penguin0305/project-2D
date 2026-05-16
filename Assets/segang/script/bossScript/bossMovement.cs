using UnityEngine;
using Unity.Netcode;
public class bossMovement : NetworkBehaviour
{
    public float speed = 3f;
    private bool movingRight = true;
    Animator animator;
    public void MoveToTarget(Transform target)
    {
        if (!IsServer) return;
        if (target == null) return;
        else if(target.transform.position.x>this.transform.position.x)//타겟이 오른쪽에 있을때 오른쪽으로 이동 
        {
            animator.SetBool("isIdling", false);
            animator.SetBool("isWalking",true);
            movingRight = true;
            float dir = movingRight ? 1f : -1f;
            //Debug.Log(target + "오른쪽에있음 오른쪽으로 이동.");
            transform.rotation = movingRight ? Quaternion.Euler(new Vector3(0, 0, 0)) : Quaternion.Euler(new Vector3(0, 180, 0));
            transform.position += new Vector3(dir * speed * Time.deltaTime, 0, 0);
        }
        else //타겟이 왼쪽일때 왼쪽으로 이동
        {
            animator.SetBool("isIdling", false);
            animator.SetBool("isWalking", true);
            movingRight = false;
            float dir = movingRight ? 1f : -1f;
            //Debug.Log(target + "왼쪽에있음 왼쪽으로 이동.");
            transform.rotation = movingRight ? Quaternion.Euler(new Vector3(0, 0, 0)) : Quaternion.Euler(new Vector3(0, 180, 0));
            transform.position += new Vector3(dir * speed * Time.deltaTime, 0, 0);
        }

    }
    public bool IsInAttackRange(Transform target)
    {
        if (!IsServer) return false;
        if (target == null) return false;

        return Vector2.Distance(transform.position, target.position) < 5;
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
    private void FixedUpdate()
    {
        
    }
}
