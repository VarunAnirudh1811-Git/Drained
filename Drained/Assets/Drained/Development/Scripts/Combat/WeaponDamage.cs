using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;
    private int damage;
    private float knockBack;
    private List<Collider> hitColliders = new List<Collider>();

    private void OnEnable()
    {
        hitColliders.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider) { return; }

        if(hitColliders.Contains(other)) { return; }

        hitColliders.Add(other);

        if (other.TryGetComponent(out Health health))
        {
            health.DealDamage(damage);
        }
        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 force = (other.transform.position - playerCollider.transform.position).normalized * knockBack;
            forceReceiver.AddForce(force);
        }

    }  
    
    public void SetAttack (int damage, float knockBack)
    {
        this.damage = damage;
        this.knockBack = knockBack;
    }
    
}
