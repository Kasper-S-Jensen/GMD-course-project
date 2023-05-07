using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private Animator _animator;
    private NavMeshAgent agent;

    private CharacterController controller;

    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (gameObject.CompareTag("Player"))
        {
            controller = gameObject.GetComponent<CharacterController>();
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        AnimateSpeed();
    }

    private void AnimateSpeed()
    {
        if (gameObject.CompareTag("Player"))
        {
            _animator.SetFloat(Speed, controller.velocity.magnitude);
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            _animator.SetFloat(Speed, agent.velocity.magnitude);
        }
    }

    public void AttackTrue()
    {
        _animator.SetBool(Attack, true);
    }

    public void AttackFalse()
    {
        _animator.SetBool(Attack, false);
    }
}