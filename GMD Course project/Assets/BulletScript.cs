using UnityEngine;
using UnityEngine.AI;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update

    public float knockbackDistance = 2f;
    public float knockbackDuration = 0.5f;
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
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            var navMeshAgent = collision.gameObject.GetComponent<NavMeshAgent>();

            if (navMeshAgent != null)
            {
            }

            Destroy(gameObject);
        }
    }
}