using System;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.EnemyAnimator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Update(float deltaTime)
    {
        if (!IsInChasingRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if (IsInAttackingRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

            FacePlayer();

        MoveToPlayer(deltaTime);

        stateMachine.EnemyAnimator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit() 
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        if(stateMachine.NavMeshAgent.isOnNavMesh)
        {
            stateMachine.NavMeshAgent.SetDestination(stateMachine.Player.transform.position);

            HandleMove(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.EnemySpeed, deltaTime);
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.Controller.velocity;
    }

    protected bool IsInAttackingRange()
    {
        if (stateMachine.Player.isDead) { return false; }

        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= Mathf.Pow(stateMachine.AttackRange, 2);
    }
}
