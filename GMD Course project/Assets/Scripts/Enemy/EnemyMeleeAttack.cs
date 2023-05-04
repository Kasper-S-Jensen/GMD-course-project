using StarterAssets.Interfaces;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttackPlayer
{
    private static readonly int Attack = Animator.StringToHash("Attack");

    //Attacking
    private Animator _animator;
    private bool alreadyAttacked;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void AttackPlayer()
    {
        alreadyAttacked = true;
        _animator.SetBool(Attack, alreadyAttacked);
    }
}