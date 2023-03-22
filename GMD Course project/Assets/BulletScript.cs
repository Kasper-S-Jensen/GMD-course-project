using System.Collections;
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
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            var navMeshAgent = collision.gameObject.GetComponent<NavMeshAgent>();

            if (navMeshAgent != null)
            {
                var direction = collision.transform.position - transform.position;
                direction.Normalize();

                StartCoroutine(Knockback(navMeshAgent, direction));
            }

            // Destroy(gameObject);
        }
    }

    private IEnumerator Knockback(NavMeshAgent navMeshAgent, Vector3 direction)
    {
        navMeshAgent.isStopped = true;

        var elapsedTime = 0f;

        while (elapsedTime < knockbackDuration)
        {
            var t = elapsedTime / knockbackDuration;
            var distance = Mathf.Lerp(0f, knockbackDistance, t);

            navMeshAgent.Move(direction * (distance * 0.1f));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        navMeshAgent.isStopped = false;
    }
}