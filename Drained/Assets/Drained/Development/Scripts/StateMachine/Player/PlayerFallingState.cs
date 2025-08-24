using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Fall");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 momentum;
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;

        stateMachine.PlayerAnimator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
        stateMachine.LedgeDetector.OnLedgeDetected += HandleLedgeDetect;
    }
    public override void Update(float deltaTime)
    {
        HandleMove(momentum, deltaTime);

        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }
    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetected -= HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    }
}
