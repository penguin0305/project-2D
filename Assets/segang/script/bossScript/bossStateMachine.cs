using UnityEngine;

public class bossStateMachine : MonoBehaviour
{
    public enum BossState
    {
        Idle,
        SelectTarget,
        Move,
        Attack,
        Wait
    }

    public BossState currentState;
    private bool targetTest = false;//테스트용 변수
    private bossTargetSelector targetSelector;
    private bossMovement movement;
    private bossAttacksController bossAttacksController;
    //private BossPatternController pattern;

    private float timer;
    public float waitTime = 2f;

    void Start()
    {
        targetSelector = GetComponent<bossTargetSelector>();
        movement = GetComponent<bossMovement>();
        bossAttacksController = GetComponent<bossAttacksController>();
        //pattern = GetComponent<BossPatternController>();

        ChangeState(BossState.Idle);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetTest = true;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case BossState.Idle:
                if (targetSelector.HasTarget())
                    ChangeState(BossState.SelectTarget);
                break;

            case BossState.SelectTarget:
                targetSelector.SelectRandomTarget();
                ChangeState(BossState.Move);
                break;

            case BossState.Move:
                movement.MoveToTarget(targetSelector.target);

                if (movement.IsInAttackRange(targetSelector.target))
                ChangeState(BossState.Attack);
                break;

            case BossState.Attack:
                bossAttacksController.playRandomAttack();
                ChangeState(BossState.Idle);
                break;

            /*case BossState.Pattern:
                pattern.PlayRandomPattern();
                ChangeState(BossState.Wait);
                break;

            case BossState.Wait:
                timer += Time.deltaTime;
                if (timer > waitTime)
                    ChangeState(BossState.SelectTarget);
                break;*/
        }
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;
        timer = 0f;
    }
}
