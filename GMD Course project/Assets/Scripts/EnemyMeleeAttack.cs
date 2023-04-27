using StarterAssets.Interfaces;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttackPlayer
{
    private static readonly int Attack = Animator.StringToHash("Attack");

    //Attacking
    public float timeBetweenAttacks;
    private Animator _animator;
    private bool alreadyAttacked;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void AttackPlayer()
    {
        // if (!alreadyAttacked)
        //  {
        //End of attack code

        alreadyAttacked = true;
        _animator.SetBool(Attack, alreadyAttacked);
        //    Invoke(nameof(ResetAttack), timeBetweenAttacks);
        // }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        _animator.SetBool(Attack, alreadyAttacked);
    }
}