using System;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangingHash = Animator.StringToHash("Hang");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 ledgeForward;
    private Vector3 closestPoint;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine) 
    {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }
    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);

        stateMachine.Controller.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.Controller.enabled = true;

        stateMachine.PlayerAnimator.CrossFadeInFixedTime(HangingHash, CrossFadeDuration);  
    }
    public override void Update(float deltaTime)
    {
        if (stateMachine.InputReader.MoveInput.y > 0f)
        {
            stateMachine.SwitchState(new PlayerClimbingState(stateMachine));
            return;
        }
        else if (stateMachine.InputReader.MoveInput.y < 0f)
        {
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }
        
    }
    public override void Exit()
    {
        
    }
    
}
