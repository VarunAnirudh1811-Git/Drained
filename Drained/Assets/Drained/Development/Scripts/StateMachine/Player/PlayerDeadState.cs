using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.WeaponDamage.gameObject.SetActive(false);
    }

    public override void Update(float deltaTime)
    {
    }

    public override void Exit() { }
}
