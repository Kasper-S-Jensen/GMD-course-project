using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
    }
}