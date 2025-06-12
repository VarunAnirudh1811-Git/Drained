using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.EnemyAnimator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Update(float deltaTime)
    {
        HandleMove(deltaTime);

        if (IsInChasingRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
        stateMachine.EnemyAnimator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit() { }
}
