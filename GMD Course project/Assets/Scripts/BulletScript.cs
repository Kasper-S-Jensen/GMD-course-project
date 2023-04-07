using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float projectileDamage=1;

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
        if (collision.gameObject.TryGetComponent<Health>(out var health))
        {
           health.TakeDamage(projectileDamage);
        }
      //  Destroy(gameObject);
    }
}