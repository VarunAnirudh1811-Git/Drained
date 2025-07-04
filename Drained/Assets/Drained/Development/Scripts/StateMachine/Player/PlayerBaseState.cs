using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }   

    protected void HandleMove(float deltaTime)
    {
        // Constructor with 1 input
        HandleMove(Vector3.zero, deltaTime);
    }

    protected void HandleMove(Vector3 motion, float deltaTime)
    {
        // Constructor with 2 inputs
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 targetDirection = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        targetDirection.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(targetDirection);
    }

    protected void ReturnToLocomotion()
    {
        if (stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
