    using StarterAssets.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
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
    private IEnemyAttackTheGate _enemyAttackTheGate;

    private Transform _player;
    private Transform _theGate;

    private void Awake()
    {
        _enemyAttackPlayer = GetComponent<IEnemyAttackPlayer>();
        _enemyAttackTheGate = GetComponent<IEnemyAttackTheGate>();
        _player = GameObject.FindWithTag("Player").transform;
        _theGate = GameObject.FindWithTag("TheGate").transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        CheckState();
    }

    // Update is called once per frame
    private void Update()
    {
        Animate();

        CheckState();
    }

    private void OnDestroy()
    {
        OnEnemyDeath.Raise(ExperienceOnDeath);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, sightRange);
    }

    //   public event EventHandler OnEnemyDeath;

    private void CheckState()
    {
        var position = transform.position;
        playerInSightRange = Physics.CheckSphere(position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);

        gateInSightRange = Physics.CheckSphere(position, sightRange, whatIsGate);
        gateInAttackRange = Physics.CheckSphere(position, attackRange, whatIsGate);

        if (gateInSightRange && gateInAttackRange && !playerInSightRange && !playerInAttackRange)
        {
            Debug.Log("ATTACKING GATE");
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

    private void AttackGate()
    {
        //Make sure enemy doesn't move
        _agent.SetDestination(transform.position);

        var lookAtTarget =
            new Vector3(_theGate.transform.position.x, transform.position.y, _theGate.transform.position.z);
        transform.LookAt(lookAtTarget);

        //  _enemyAttackTheGate.AttackGate(_agent);
        _enemyAttackPlayer.AttackPlayer();
    }


    private void StormTheGate()
    {
        _agent.SetDestination(_theGate.position);
        /*if (Vector3.Distance(_agent.transform.position, _theGate.transform.position) < 1)
        {
            DestroyEnemy();
        }*/


        //  IterateWaypointIndex();
    }


    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
        //  IterateWaypointIndex();
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


        //OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs {ExperienceOnDeath = ExperienceOnDeath});

        Destroy(gameObject);
    }
}