using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maximumHealth = 2;
    public GameEvent OnGateDestroyed;

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
    }

    private void Die()
    {
        if (gameObject.CompareTag("TheGate"))
        {
            OnGateDestroyed.Raise();
            return;
        }

        Destroy(gameObject);
    }
}