using UnityEngine;
using Unity.Netcode;
public class bossStateMachine : NetworkBehaviour
{
    public enum BossState
    {
        Idle,
        SelectTarget,
        Move,
        Attack,
        Wait,
        Dead
    }

    internal itemDropController dropper;
    public BossState currentState;
    private bool targetTest = false;//테스트용 변수
    private bool isDead = false;//테스트용 변수
    private bool dieOnce = false;//죽는 애니메이션 1번 나오는 용도
    private Animator animator;
    private bossTargetSelector targetSelector;
    private bossMovement movement;
    private bossAttacksController bossAttacksController;
    //private BossPatternController pattern;

    private float timer;
    public float waitTime = 2f;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        dropper = GetComponent<itemDropController>();
        targetSelector = GetComponent<bossTargetSelector>();
        movement = GetComponent<bossMovement>();
        bossAttacksController = GetComponent<bossAttacksController>();
        //pattern = GetComponent<BossPatternController>();

        if (IsServer)
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
        if (!IsServer) return;

        if (isDead && currentState != BossState.Dead)
        {
            ChangeState(BossState.Dead);
            return;
        }

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
            case BossState.Dead:
                if (!dieOnce)
                {
                    DeadClientRpc();
                    if (IsServer)
                    {
                        Portaltmp portal = Object.FindAnyObjectByType<Portaltmp>();
                        portal.ActivateVisual();
                        if (dropper != null)
                            dropper.DropItems();
                    }
                    dieOnce = true;
                }
                break;
        }
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;
        timer = 0f;
        UpdateStateClientRpc(newState);
    }
    [ClientRpc]
    void UpdateStateClientRpc(BossState newState)
    {
        currentState = newState;
    }
    [ClientRpc]
    void DeadClientRpc()
    {
        animator.SetTrigger("isDead");
    }
    public void deadSignal()
    {
        if (!IsServer) return;
        isDead = true;
    }
    public void despawnForAnimation()
    {
        NetworkObject.Despawn();
    }
}
