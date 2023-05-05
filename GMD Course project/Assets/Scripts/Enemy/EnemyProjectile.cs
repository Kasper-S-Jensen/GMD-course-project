using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float projectileDamage = 1;

    public LayerMask whatIsGate;
    public GameEvent OnGateDamage;

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamage(projectileDamage);
        }

        if (collision.gameObject.TryGetComponent<Health>(out var health))
        {
            //only hit gate not enemies
            if (whatIsGate == (whatIsGate | (1 << collision.gameObject.layer)))
            {
                health.TakeDamage(projectileDamage);
                OnGateDamage.Raise(projectileDamage);
            }
        }

        Destroy(gameObject);
    }
}