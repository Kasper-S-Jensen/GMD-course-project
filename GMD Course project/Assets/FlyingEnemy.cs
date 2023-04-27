using StarterAssets.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour, IEnemyAI
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Attack = Animator.StringToHash("Attack");
    public GameEvent OnEnemyDeath;
    public int ExperienceOnDeath = 100;
    public LayerMask whatIsPlayer, whatIsGate;
    public GameObject explosion;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, gateInSightRange, gateInAttackRange;
    public float flyingHeight = 5f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private IEnemyAttackPlayer _enemyAttackPlayer;
    private IEnemyAttackTheGate _enemyAttackTheGate;


    private Transform _player;
    private Transform _theGate;
    private float initialFlyingHeight;

    private void Awake()
    {
        _enemyAttackPlayer = GetComponent<IEnemyAttackPlayer>();
        _enemyAttackTheGate = GetComponent<IEnemyAttackTheGate>();
        _player = GameObject.FindWithTag("Player").transform;
        _theGate = GameObject.FindWithTag("TheGate").transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.autoTraverseOffMeshLink = false;
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        initialFlyingHeight = transform.position.y;
    }

    private void Update()
    {
        // Animate();
        CheckState();
    }


    private void OnDestroy()
    {
        OnEnemyDeath.Raise(ExperienceOnDeath);
        DestroyEnemy();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, sightRange);
    }


    public void CheckState()
    {
        var position = transform.position;
        playerInSightRange = Physics.CheckSphere(position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);

        gateInSightRange = Physics.CheckSphere(position, sightRange, whatIsGate);
        gateInAttackRange = Physics.CheckSphere(position, attackRange, whatIsGate);

        if (gateInSightRange && gateInAttackRange && !playerInSightRange && !playerInAttackRange)
        {
            AttackGate();
        }

        if (!playerInSightRange && !playerInAttackRange && !gateInAttackRange)
        {
            StormTheGate();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
    }

    private void AttackGate()
    {
        Debug.Log("ghost fire");
        //Make sure enemy doesn't move
        MaintainFlyingHeight();

        var lookAtTarget =
            new Vector3(_theGate.transform.position.x, transform.position.y, _theGate.transform.position.z);
        transform.LookAt(lookAtTarget);
        _animator.SetBool(Attack, true);
        //  _enemyAttackTheGate.AttackGate(_agent);
        // _enemyAttackPlayer.AttackPlayer();
    }


    private void StormTheGate()
    {
        _animator.SetBool(Attack, false);
        FlyToDestination(_theGate.position);
    }


    private void ChasePlayer()
    {
        _animator.SetBool(Attack, false);
        FlyToDestination(_player.position);
    }

    private void FlyToDestination(Vector3 destination)
    {
        destination.y = initialFlyingHeight + flyingHeight;
        _agent.SetDestination(destination);

        // Maintain flying height
        transform.position = new Vector3(transform.position.x,
            initialFlyingHeight + flyingHeight,
            transform.position.z);
    }

    private void AttackPlayer()
    {
        MaintainFlyingHeight();

        transform.LookAt(_player);
        _animator.SetBool(Attack, true);
    }

    private void MaintainFlyingHeight()
    {
        // Maintain flying height
        transform.position = new Vector3(transform.position.x,
            initialFlyingHeight + flyingHeight,
            transform.position.z);
        _agent.SetDestination(new Vector3(transform.position.x,
            initialFlyingHeight + flyingHeight,
            transform.position.z));
    }

    //triggered by animation event
    private void FireProjectile()
    {
        _enemyAttackPlayer.AttackPlayer();
    }


    private void Animate()
    {
        _animator.SetFloat(Speed, _agent.velocity.magnitude);
    }


    private void DestroyEnemy()
    {
        var o = gameObject.transform;
        var explosionSpawnpoint = new Vector3(o.position.x, o.position.y + 1, o.position.z);

        var explosionObj = Instantiate(explosion, explosionSpawnpoint, o.transform.rotation);
        Destroy(explosionObj, 2f);


        Destroy(gameObject);
    }
}