using StarterAssets.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class HunterEnemyAI : MonoBehaviour, IEnemyAI
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Attack = Animator.StringToHash("Attack");
    public GameEvent OnEnemyDeath;
    public int ExperienceOnDeath = 100;
    public LayerMask whatIsPlayer;
    public GameObject explosion;

    //States
    public float sightRange, attackRange;
    public bool playerInAttackRange;
    private NavMeshAgent _agent;


    private Animator _animator;
    private IEnemyAttackPlayer _enemyAttackPlayer;
    private Transform _player;

    private void Awake()
    {
        _enemyAttackPlayer = GetComponent<IEnemyAttackPlayer>();
        _player = GameObject.FindWithTag("Player").transform;
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

    //   public event EventHandler OnEnemyDeath;

    public void CheckState()
    {
        var position = transform.position;
        playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);

        if (!playerInAttackRange)
        {
            _animator.SetBool(Attack, false);
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