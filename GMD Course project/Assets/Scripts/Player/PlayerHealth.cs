using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maximumHealth = 2;
    public GameEvent OnPlayerDamaged;
    public GameEvent OnPlayerDeath;

    private void Start()
    {
        currentHealth = maximumHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("Player hurt!");
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            Die();
        }

        OnPlayerDamaged.Raise(currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player died!");
        OnPlayerDeath.Raise();
    }
}