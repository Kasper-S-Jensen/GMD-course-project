using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float projectileDamage = 1;

    public LayerMask whatIsGate;
    public GameEvent OnGateDamage;

    //  private int projectileLayer = 8;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    private void OnDestroy()
    {
        //explosion
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }

        if (collision.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamage(projectileDamage);
        }

        if (collision.gameObject.TryGetComponent<Health>(out var health))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                health.TakeDamage(projectileDamage);
            }

            if (whatIsGate == (whatIsGate | (1 << collision.gameObject.layer)))
            {
                health.TakeDamage(projectileDamage);
                OnGateDamage.Raise(projectileDamage);
            }
        }


        Destroy(gameObject);
    }
}