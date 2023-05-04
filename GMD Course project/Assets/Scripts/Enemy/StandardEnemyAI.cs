using StarterAssets.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class StandardEnemyAI : MonoBehaviour, IEnemyAI
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    public GameEvent OnEnemyDeath;
    public int ExperienceOnDeath = 100;
    public LayerMask whatIsPlayer, whatIsGate;
    public GameObject explosion;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, gateInSightRange, gateInAttackRange;

    private NavMeshAgent _agent;
    private Animator _animator;
    private IEnemyAttackPlayer _enemyAttackPlayer;

    private Transform _player, _theGate;

    private void Awake()
    {
        _enemyAttackPlayer = GetComponent<IEnemyAttackPlayer>();
        _player = GameObject.FindWithTag("Player").transform;
        _theGate = GameObject.FindWithTag("TheGate").transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }


    private void Start()
    {
        CheckState();
    }

    private void Update()
    {
        Animate();
        CheckState();
    }

    private void OnDestroy()
    {
        OnEnemyDeath.Raise(ExperienceOnDeath);
        DestroyEnemy();
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


    public void CheckState()
    {
        var position = transform.position;
        PlayerRange(position);
        GateRange(position);

        if (gateInSightRange && gateInAttackRange && !playerInSightRange && !playerInAttackRange)
        {
            AttackGate();
        }

        if (!playerInSightRange && !playerInAttackRange)
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
        //Make sure enemy doesn't move
        _agent.SetDestination(transform.position);

        var lookAtTarget =
            new Vector3(_theGate.transform.position.x, transform.position.y, _theGate.transform.position.z);
        transform.LookAt(lookAtTarget);

        _enemyAttackPlayer.AttackPlayer();
    }


    private void StormTheGate()
    {
        _agent.SetDestination(_theGate.position);
    }


    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        _agent.SetDestination(transform.position);
        var lookAtTarget =
            new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        transform.LookAt(lookAtTarget);

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