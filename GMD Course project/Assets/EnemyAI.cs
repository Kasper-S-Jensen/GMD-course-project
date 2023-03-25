using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int Speed = Animator.StringToHash("Speed");
    public Transform[] waypoints;
    private NavMeshAgent _agent;
    private Animator _animator;

    private Vector3 target;

    private int waypointIndex;

    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        //    waypoints[0] = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        //  if (Vector3.Distance(transform.position, target) < 1)
        //  {
        //      IterateWaypointIndex();
        UpdateDestination();
        animate();
        //  }
    }

    private void animate()
    {
        _animator.SetFloat(Speed, _agent.velocity.magnitude);
        Debug.Log(_agent.velocity.magnitude);
    }

    private void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        _agent.SetDestination(target);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length) waypointIndex = 0;
    }
}