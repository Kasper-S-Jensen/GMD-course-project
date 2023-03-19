using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent _agent;

    private Vector3 target;

    private int waypointIndex;

    // Start is called before the first frame update
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

    // Update is called once per frame
    private void Update()
    {
        //  if (Vector3.Distance(transform.position, target) < 1)
        //  {
        //      IterateWaypointIndex();
        UpdateDestination();
        //  }
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