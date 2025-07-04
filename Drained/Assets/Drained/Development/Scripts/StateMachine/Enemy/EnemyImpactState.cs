using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("RibHit");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 1f;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.EnemyAnimator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Update(float deltaTime)
    {
        HandleMove(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit()
    {   
    }   
}
