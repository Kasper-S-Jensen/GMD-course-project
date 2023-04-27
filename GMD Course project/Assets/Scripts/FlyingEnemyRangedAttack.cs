using StarterAssets.Interfaces;
using UnityEngine;

public class FlyingEnemyRangedAttack : MonoBehaviour, IEnemyAttackPlayer
{
    //Attacking
    public GameObject projectile;
    public Transform barrelEnd;
    private Transform bulletContainer;

    private void Awake()
    {
        bulletContainer = GameObject.FindWithTag("BulletContainer").transform;
    }

    public void AttackPlayer()
    {
        var currentProjectile = Instantiate(projectile, barrelEnd.position, Quaternion.identity,
            bulletContainer.transform);
        var rb = currentProjectile.GetComponent<Rigidbody>();
        Physics.IgnoreCollision(currentProjectile.GetComponent<Collider>(), GetComponent<Collider>());
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        rb.AddForce(transform.up * 4f, ForceMode.Impulse);
    }
}