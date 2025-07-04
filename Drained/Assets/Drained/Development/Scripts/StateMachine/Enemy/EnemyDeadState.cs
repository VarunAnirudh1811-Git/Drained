using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        stateMachine.WeaponDamage.gameObject.SetActive(false);
        GameObject.Destroy(stateMachine.Target);
    }

    public override void Update(float deltaTime)
    {
    }

    public override void Exit() {}
}
