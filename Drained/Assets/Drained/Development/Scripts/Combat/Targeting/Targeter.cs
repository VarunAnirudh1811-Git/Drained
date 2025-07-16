using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using System;

public class Targeter : MonoBehaviour
{
    [field: SerializeField] private Camera PlayerCamera;
    [field: SerializeField] private CinemachineTargetGroup CineTargetGroup;

    private List<Target> targetsInRange = new List<Target>();
    public Target CurrentTarget { get; private set;}

    private void Start()
    {
       PlayerCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Target>(out Target target)) 
        {
            targetsInRange.Add(target);
            target.DestroyEvent += RemoveTarget;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        RemoveTarget(target);   

    }

    public bool SelectTarget()
    {
        if (targetsInRange.Count == 0) return false;

        Target closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Target target in targetsInRange)
        {
            Vector2 screenPoint = PlayerCamera.WorldToViewportPoint(target.transform.position);

            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue; // Target is outside the screen bounds
            }

            Vector2 toCenter = screenPoint - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestDistance)
            {
                closestDistance = toCenter.sqrMagnitude;
                closestTarget = target;
            }
        }

        if (closestTarget == null) return false;

        CurrentTarget = closestTarget;
        CineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void ClearTarget()
    {
        if (CurrentTarget == null) return;

        CineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;   
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            CineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        targetsInRange.Remove(target);
    }
}
