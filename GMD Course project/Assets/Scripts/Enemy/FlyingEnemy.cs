using StarterAssets.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour, IEnemyAI
{
    public GameEvent OnEnemyDeath;
    public int ExperienceOnDeath = 100;
    public LayerMask whatIsPlayer, whatIsGate;
    public GameObject explosion;

    public float sightRange, attackRange, flyingHeight = 5f;
    public bool playerInSightRange, playerInAttackRange, gateInSightRange, gateInAttackRange;

    private NavMeshAgent _agent;
    private AnimationController _animationController;
    private IEnemyAttackPlayer _enemyAttackPlayer;
    private IEnemyAttackTheGate _enemyAttackTheGate;

    private Transform _player, _theGate;
    private float initialFlyingHeight;
    private bool isQuitting;

    private void Awake()
    {
        _enemyAttackPlayer = GetComponent<IEnemyAttackPlayer>();
        _enemyAttackTheGate = GetComponent<IEnemyAttackTheGate>();
        _player = GameObject.FindWithTag("Player").transform;
        _theGate = GameObject.FindWithTag("TheGate").transform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoTraverseOffMeshLink = false;
        _animationController = GetComponent<AnimationController>();
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        initialFlyingHeight = transform.position.y;
    }

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
        DestroyEnemy();
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    //for debugging
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
        PlayerRange(position);
        GateRange(position);

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

    private void PlayerRange(Vector3 position)
    {
        playerInSightRange = Physics.CheckSphere(position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);
    }

    private void GateRange(Vector3 position)
    {
        gateInSightRange = Physics.CheckSphere(position, sightRange, whatIsGate);
        gateInAttackRange = Physics.CheckSphere(position, attackRange, whatIsGate);
    }

    private void AttackGate()
    {
        //Make sure enemy doesn't move while attacking
        MaintainFlyingHeight();

        var theGateTransform = _theGate.transform;
        var lookAtTarget =
            new Vector3(theGateTransform.position.x, transform.position.y, theGateTransform.position.z);
        transform.LookAt(lookAtTarget);

        _animationController.AttackTrue();
    }


    private void StormTheGate()
    {
        _animationController.AttackFalse();
        FlyToDestination(_theGate.position);
    }


    private void ChasePlayer()
    {
        _animationController.AttackFalse();
        FlyToDestination(_player.position);
    }


    private void FlyToDestination(Vector3 destination)
    {
        //flying logic
        destination.y = initialFlyingHeight + flyingHeight;
        _agent.SetDestination(destination);

        // Maintain flying height
        var position = transform.position;
        position = new Vector3(position.x,
            initialFlyingHeight + flyingHeight,
            position.z);
        transform.position = position;
    }

    private void AttackPlayer()
    {
        MaintainFlyingHeight();

        transform.LookAt(_player);
        _animationController.AttackTrue();
    }

    private void MaintainFlyingHeight()
    {
        // Maintain flying height at all times
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


    private void DestroyEnemy()
    {
        var o = gameObject.transform;
        var explosionSpawnpoint = new Vector3(o.position.x, o.position.y + 1, o.position.z);
        var explosionObj = Instantiate(explosion, explosionSpawnpoint, o.transform.rotation);
        Destroy(explosionObj, 2f);
        Destroy(gameObject);
    }
}