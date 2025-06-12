using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController CharacterController;
    [SerializeField] private float drag = 0.3f;

    private Vector3 impact;
    private Vector3 dampingVelocity;
    private float verticalVelocity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        if (verticalVelocity <= 0 && CharacterController.isGrounded)
        {
            // Apply small velocity
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // Apply gravity
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag); 
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}

