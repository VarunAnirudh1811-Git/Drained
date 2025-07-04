using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    public event Action OnTakeDamage;
    public event Action OnDie;

    private int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth; 

    }

    public void DealDamage(int damage)
    {
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        OnTakeDamage?.Invoke();

        if (currentHealth == 0) 
        {
            OnDie?.Invoke();
        }

        Debug.Log($"Health: {currentHealth}/{maxHealth}");
    }

}
