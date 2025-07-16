using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    public event Action OnTakeDamage;
    public event Action OnDie;

    private int currentHealth;
    private bool isInvulnerable;

    public bool isDead => currentHealth <= 0;
    private void Start()
    {
        currentHealth = maxHealth; 

    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage)
    {
        if (currentHealth == 0) { return; }

        if (isInvulnerable) { return; }

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        OnTakeDamage?.Invoke();

        if (currentHealth == 0) 
        {
            OnDie?.Invoke();
        }

        Debug.Log($"Health: {currentHealth}/{maxHealth}");
    }

}
