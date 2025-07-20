using UnityEngine;
using UnityEngine.AI;

public class GenericWaypointMovement : MonoBehaviour
{
    [Header("Waypoints to Follow")]
    [SerializeField] private Transform[] waypoints;

    [Header("Movement Settings")]
    [SerializeField] private float arrivalThreshold = 0.5f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints != null && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else
        {
            Debug.LogWarning("No waypoints assigned!", this);
        }
    }

    private void Update()
    {
        if (waypoints.Length == 0 || agent.pathPending) return;

        float distance = Vector3.Distance(agent.transform.position, waypoints[currentWaypointIndex].position);
        if (distance <= arrivalThreshold)
        {
            AdvanceToNextWaypoint();
        }
    }

    private void AdvanceToNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }
}
