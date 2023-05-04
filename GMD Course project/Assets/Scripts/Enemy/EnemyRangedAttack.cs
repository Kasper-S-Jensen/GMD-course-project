using StarterAssets.Interfaces;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour, IEnemyAttackPlayer
{
    //Attacking
    public float timeBetweenAttacks;
    public GameObject projectile;
    public Transform barrelEnd;
    private bool alreadyAttacked;
    private Transform bulletContainer;

    private void Awake()
    {
        bulletContainer = GameObject.FindWithTag("BulletContainer").transform;
    }

    public void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            var currentProjectile = Instantiate(projectile, barrelEnd.position, Quaternion.identity,
                bulletContainer.transform);
            var rb = currentProjectile.GetComponent<Rigidbody>();
            Physics.IgnoreCollision(currentProjectile.GetComponent<Collider>(), GetComponent<Collider>());
            rb.AddForce(transform.forward * 16f, ForceMode.Impulse);
            //End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}