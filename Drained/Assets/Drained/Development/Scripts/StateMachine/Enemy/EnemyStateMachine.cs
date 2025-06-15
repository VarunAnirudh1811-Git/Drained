using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field : SerializeField] public Animator EnemyAnimator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    [field: SerializeField] public float EnemySpeed { get; private set; }
    [field : SerializeField] public float PlayerChasingRange { get; private set; } = 5f;
    [field: SerializeField] public float AttackRange { get; private set; } = 1f;
    public GameObject Player { get; private set; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updateRotation = false;

        SwitchState(new EnemyIdleState(this));
        Debug.Log($"{gameObject.name} is running an EnemyStateMachine");
    }
}
