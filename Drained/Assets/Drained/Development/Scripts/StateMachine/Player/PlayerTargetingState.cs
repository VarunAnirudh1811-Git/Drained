using System;
using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
    private float AnimatorDampTime = 0.1f;
    private float CrossFadeDuration = 0.2f;

    /// Constructor
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.ToggleTargetingEvent += OnToggleTargeting;
        stateMachine.PlayerAnimator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Update(float deltaTime)
    {
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CaluclateMovement();
        HandleMove(movement * stateMachine.TargetingMoveSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();

    }

    public override void Exit()
    {
        stateMachine.InputReader.ToggleTargetingEvent -= OnToggleTargeting;
    }

    private void OnToggleTargeting()    
    {
        stateMachine.Targeter.ClearTarget();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CaluclateMovement()
    {
        Vector3 movement = new Vector3();

        movement+= stateMachine.transform.right * stateMachine.InputReader.MoveInput.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MoveInput.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputReader.MoveInput.y == 0)
        {
            stateMachine.PlayerAnimator.SetFloat(TargetingForwardHash, 0, AnimatorDampTime, deltaTime);
        }
        else 
        {
            float yvalue = stateMachine.InputReader.MoveInput.y > 0 ? 1 : -1;
            stateMachine.PlayerAnimator.SetFloat(TargetingForwardHash, yvalue, AnimatorDampTime, deltaTime);
        }

        if (stateMachine.InputReader.MoveInput.x == 0)
        {
            stateMachine.PlayerAnimator.SetFloat(TargetingRightHash, 0, AnimatorDampTime, deltaTime);
        }
        else
        {
            float xvalue = stateMachine.InputReader.MoveInput.x > 0 ? 1 : -1;
            stateMachine.PlayerAnimator.SetFloat(TargetingRightHash, xvalue, AnimatorDampTime, deltaTime);
        }
    }
}
