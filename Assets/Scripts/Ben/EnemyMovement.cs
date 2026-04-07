using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void FollowTarget(Transform target)
    {
        float randomValue = Random.value;
        if (Random.value < 0.33f)
        {
            // Direct Path
            agent.SetDestination(target.transform.position);
        }
        else if (Random.value > 0.66f)
        {
            // Cardinal Directions Only.
            Vector3 targetPosition = transform.position;

            Vector3 direction = target.position - transform.position;

            if (Mathf.Abs(direction.x) > 0.05f && Mathf.Abs(direction.z) > 0.05f)
            {
                if (Random.value < 0.5f)
                {
                    targetPosition.x = target.position.x;
                }
                else
                {
                    targetPosition.z = target.position.z;
                }
            }
            else if (Mathf.Abs(direction.x) >= 0.05f)
            {
                targetPosition.x = target.position.x;
            }
            else if (Mathf.Abs(direction.z) >= 0.05f)
            {
                targetPosition.z = target.position.z;
            }

            agent.SetDestination(targetPosition);
        }
        else
        {
            // Diagonal Directions Only.
            Vector3 targetPosition = transform.position;
            Vector3 direction = target.position - transform.position;

            if (Mathf.Abs(direction.x) > 0.05f && Mathf.Abs(direction.z) > 0.05f)
            {
                targetPosition.x = target.position.x;
                targetPosition.z = target.position.z;
            }
            else if (Mathf.Abs(direction.x) >= 0.05f)
            {
                targetPosition.x = target.position.x;
                targetPosition.z += Random.value;
            }
            else if (Mathf.Abs(direction.z) >= 0.05f)
            {
                targetPosition.z = target.position.z;
                targetPosition.x += Random.value;
            }

            agent.SetDestination(targetPosition);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (agent != null && agent.hasPath)
        {
            Vector3 previousPoint = transform.position;

            foreach (Vector3 corner in agent.path.corners)
            {
                Gizmos.DrawLine(previousPoint, corner);
                previousPoint = corner;
            }
        }
    }
}
