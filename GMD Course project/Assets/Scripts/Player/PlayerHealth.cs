using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maximumHealth;
    public GameEvent OnPlayerDamaged;
    public GameEvent OnPlayerHealed;
    public GameEvent OnPlayerDeath;
    public GameEvent OnUpdateScore;

    private void Start()
    {
        currentHealth = maximumHealth;
    }

    public void TakeDamage(float damageAmount)
    {
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
        OnPlayerDeath.Raise();
    }

    public void StartHealing(float healingAmount)
    {
        if (currentHealth < maximumHealth)
        {
            currentHealth += healingAmount;
            OnPlayerHealed.Raise(currentHealth);
            OnUpdateScore.Raise((int) -healingAmount * 10);
        }
    }

    public void StopHealing()
    {
    }
}