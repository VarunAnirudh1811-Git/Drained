using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
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
    protected bool IsInChasingRange()
    {
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= Mathf.Pow(stateMachine.PlayerChasingRange, 2);
    }

}
