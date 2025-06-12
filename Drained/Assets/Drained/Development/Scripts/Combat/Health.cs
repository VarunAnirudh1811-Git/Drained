using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth; 

    }

    public void DealDamage(int damage)
    {
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        Debug.Log($"Health: {currentHealth}/{maxHealth}");
    }

}
