using System;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 momentum;
    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0; 

        stateMachine.PlayerAnimator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
        stateMachine.LedgeDetector.OnLedgeDetected += HandleLedgeDetect;
    }
    public override void Update(float deltaTime)
    {
        HandleMove(momentum, deltaTime);

        if (stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
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
