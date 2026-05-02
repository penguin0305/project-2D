using UnityEngine;

public class bossAttacksController : MonoBehaviour
{
    Animator animator;
    public void playRandomAttack()
    {
        int rand = Random.Range(0, 2);

        switch (rand)
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
