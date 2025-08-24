using System;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private Vector3 dodgeDirectionInput;
    private float remainingDodgeDuration;
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgeDirectionInput) : base(stateMachine) 
    { 
        this.dodgeDirectionInput = dodgeDirectionInput;
    }

    public override void Enter()
    {
        remainingDodgeDuration = stateMachine.DodgeDuration;

        stateMachine.PlayerAnimator.SetFloat(DodgeForwardHash, dodgeDirectionInput.y, AnimatorDampTime, 0f);
        stateMachine.PlayerAnimator.SetFloat(DodgeRightHash, dodgeDirectionInput.y, AnimatorDampTime, 0f);
        stateMachine.PlayerAnimator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Update(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * dodgeDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgeDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;

        HandleMove(movement, deltaTime);

        FaceTarget();

        remainingDodgeDuration -= deltaTime;

        if (remainingDodgeDuration <= 0)
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
