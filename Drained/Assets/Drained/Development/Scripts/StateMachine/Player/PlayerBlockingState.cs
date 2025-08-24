using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockingHash = Animator.StringToHash("Block");
    private const float CrossFadeDuration = 0.1f;
    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.PlayerAnimator.CrossFadeInFixedTime(BlockingHash, CrossFadeDuration);
    }
    public override void Update(float deltaTime)
    {
        HandleMove(deltaTime);

        if (!stateMachine.InputReader.IsBlocking)
        {
            ReturnToLocomotion();
            return;
        }
    }
    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }

}
