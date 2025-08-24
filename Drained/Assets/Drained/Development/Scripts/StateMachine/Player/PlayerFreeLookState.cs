using System;
using System.Globalization;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.2f;
    private bool shouldFade; 

    /// Constructor
    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = false) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }
    public override void Enter()
    {
        stateMachine.InputReader.ToggleTargetingEvent += OnToggleTargeting;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.DodgeEvent += OnDodge;

        stateMachine.PlayerAnimator.SetFloat(FreeLookSpeedHash, 0f);

        if (shouldFade)
        {
            stateMachine.PlayerAnimator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            stateMachine.PlayerAnimator.Play(FreeLookBlendTreeHash);
        }
    }       

    public override void Update(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();

        HandleMove(movement * stateMachine.FreeLookMoveSpeed, deltaTime);

        if (stateMachine.InputReader.MoveInput == Vector2.zero)
        {
            stateMachine.PlayerAnimator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.PlayerAnimator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.ToggleTargetingEvent -= OnToggleTargeting;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }

    private void OnToggleTargeting()
    {
        if(!stateMachine.Targeter.SelectTarget()) return;

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;
        
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        Vector3 moveDirection = forward * stateMachine.InputReader.MoveInput.y + right * stateMachine.InputReader.MoveInput.x;

        return moveDirection;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.FreeLookRotationSpeed);
    }

    private void OnDodge()
    {
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MoveInput));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }
}
