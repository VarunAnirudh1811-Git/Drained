using UnityEngine;

public class Target : MonoBehaviour
{
    public event System.Action<Target> DestroyEvent;

    private void OnDestroy()
    {
        DestroyEvent?.Invoke(this); 
    }

}
