using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    public float damage = 1f;

    // Start is called before the first frame update
    private void Start()
    {
    }


    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
    }
}