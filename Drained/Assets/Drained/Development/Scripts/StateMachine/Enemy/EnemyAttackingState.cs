using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int RightPunchHash = Animator.StringToHash("PunchRight");
    private const float TransitionDuration = 0.1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.WeaponDamage.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockBack);

        stateMachine.EnemyAnimator.CrossFadeInFixedTime(RightPunchHash, TransitionDuration);
    }

    public override void Update(float deltaTime)
    {
        FacePlayer();

        if (GetAttackNormalizedTime(stateMachine.EnemyAnimator) >= 1)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit()
    {
    }

}
