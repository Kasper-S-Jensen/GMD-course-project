using StarterAssets.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class HunterEnemyAI : MonoBehaviour, IEnemyAI
{
    public GameEvent OnEnemyDeath;
    public int ExperienceOnDeath = 100;
    public LayerMask whatIsPlayer;


    //States
    public float sightRange, attackRange;
    public bool playerInAttackRange;
    private NavMeshAgent _agent;
    private AnimationController _animationController;

    private IEnemyAttackPlayer _enemyAttackPlayer;
    private Transform _player;
    private bool isQuitting;

    private void Awake()
    {
        _enemyAttackPlayer = GetComponent<IEnemyAttackPlayer>();
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _animationController = GetComponent<AnimationController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        CheckState();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckState();
    }

    private void OnDestroy()
    {
        if (isQuitting)
        {
            return;
        }

        OnEnemyDeath.Raise(ExperienceOnDeath);
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }


    //debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, sightRange);
    }

    //   public event EventHandler OnEnemyDeath;

    public void CheckState()
    {
        var position = transform.position;
        playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);

        if (!playerInAttackRange)
        {
            _animationController.AttackFalse();
            ChasePlayer();
        }

        if (playerInAttackRange)
        {
            AttackPlayer();
        }
    }


    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't movee
        _agent.SetDestination(transform.position);

        var lookAtTarget =
            new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        transform.LookAt(lookAtTarget);

        _enemyAttackPlayer.AttackPlayer();
    }
}