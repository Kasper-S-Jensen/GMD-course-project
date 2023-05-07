using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maximumHealth = 2;
    public GameEvent OnGateDestroyed;
    public GameObject explosion;

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

        DestroyEnemy();
        Destroy(gameObject);
    }

    private void DestroyEnemy()
    {
        var o = gameObject.transform;
        var explosionSpawnpoint = new Vector3(o.position.x, o.position.y + 1, o.position.z);
        var explosionObj = Instantiate(explosion, explosionSpawnpoint, o.transform.rotation);
        Destroy(explosionObj, 2f);
        Destroy(gameObject);
    }
}