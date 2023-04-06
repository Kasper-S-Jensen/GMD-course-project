using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int Speed = Animator.StringToHash("Speed");
    public GameEvent OnEnemyDeath;
    public int ExperienceOnDeath = 100;
    public LayerMask whatIsGround, whatIsPlayer;

    public GameObject explosion;

    //patrolling
    public Vector3 walkPoint;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    public GameObject projectile;
    public Transform barrelEnd;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private NavMeshAgent _agent;


    private Animator _animator;

    // public Transform[] waypoints;
    private Transform _player;

    private Transform _theGate;

    private WaveSpawner _waveSpawner;
    private bool alreadyAttacked;
    private Transform bulletContainer;
    private bool walkPointSet;

    private int waypointIndex;


    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _theGate = GameObject.FindWithTag("TheGate").transform;
        bulletContainer = GameObject.FindWithTag("BulletContainer").transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _waveSpawner = GetComponentInParent<WaveSpawner>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        /*
        if (OnEnemyDeath == null)
        {
            OnEnemyDeath = new UnityEventInt();
        }
        */

        CheckState();
    }

    // Update is called once per frame
    private void Update()
    {
        Animate();
        //  UpdateDestination();
        //Check for sight and attack range
        CheckState();
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


    private void StormTheGate()
    {
        _agent.SetDestination(_theGate.position);
        if (Vector3.Distance(_agent.transform.position, _theGate.transform.position) < 1)
        {
            DestroyEnemy();
        }


        //  IterateWaypointIndex();
    }

    private void UpdateDestination()
    {
        //  target = waypoints[waypointIndex].position;
        _agent.SetDestination(_theGate.position);
    }

    private void IterateWaypointIndex()
    {
        // if (transform.position.z.Equals(_theGate)) waypointIndex++;

        // if (waypointIndex == waypoints.Length) waypointIndex = 0;
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

        if (!alreadyAttacked)
        {
            //Attack code here
            var rb = Instantiate(projectile, barrelEnd.position, Quaternion.identity, bulletContainer.transform)
                .GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 16f, ForceMode.Impulse);
            rb.AddForce(transform.up * 4f, ForceMode.Impulse);

            //End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        // IterateWaypointIndex();
    }


    private void Animate()
    {
        _animator.SetFloat(Speed, _agent.velocity.magnitude);
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void DestroyEnemy()
    {
        _waveSpawner.waves[_waveSpawner.currentWaveIndex].enemiesLeft--;
        var o = gameObject.transform;
        var explosionSpawnpoint = new Vector3(o.position.x, o.position.y + 1, o.position.z);

        var explosionObj = Instantiate(explosion, explosionSpawnpoint, o.transform.rotation);
        Destroy(explosionObj, 2f);

        OnEnemyDeath.Raise(ExperienceOnDeath);
        //OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs {ExperienceOnDeath = ExperienceOnDeath});


        Destroy(gameObject);
    }

    /*public class OnEnemyDeathEventArgs : EventArgs
    {
        public float ExperienceOnDeath;
    }*/
}