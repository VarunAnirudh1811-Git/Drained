using System;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private Attack attack;
    private bool isForceApplied;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.WeaponDamage.SetAttack(attack.Damage, attack.KnockBack);
        stateMachine.PlayerAnimator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Update(float deltaTime)
    {
        HandleMove(deltaTime);

        FaceTarget();

        float attackNormalizedTime = GetAttackNormalizedTime(stateMachine.PlayerAnimator);
        if (attackNormalizedTime < 1f)
        {
            if (attack.ForceTime >= attackNormalizedTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(attackNormalizedTime);
            }

        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
        }
        previousFrameTime = attackNormalizedTime;
    }

    public override void Exit()
    {
    }

    private void TryComboAttack(float attackNormalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return;}
        if (attackNormalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachine,
                attack.ComboStateIndex
            )
        );

    }

    private void TryApplyForce()
    {
        if (isForceApplied) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.ForceAmount);
        isForceApplied = true;

    }
}
