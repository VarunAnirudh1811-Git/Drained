using UnityEngine;

public class PlayerClimbingState : PlayerBaseState
{
    private readonly int ClimbingHash = Animator.StringToHash("Climb");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 Offset = new Vector3(0f, 2.325f, 0.6f); 
    public PlayerClimbingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.PlayerAnimator.CrossFadeInFixedTime(ClimbingHash, CrossFadeDuration);
    }
    public override void Update(float deltaTime)
    {
        if (GetAttackNormalizedTime(stateMachine.PlayerAnimator, "Climb") < 1f) { return; }

        stateMachine.Controller.enabled = false;
        stateMachine.transform.Translate(Offset, Space.Self);
        stateMachine.Controller.enabled = true;

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }
    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }
}
