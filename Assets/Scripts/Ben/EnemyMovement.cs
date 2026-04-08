using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public float cardinalMoveChance = 0.33f;
    public float diagonalMoveChance = 0.33f;
    private float normalMoveChance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void FollowTarget(Transform target)
    {
        float randomValue = Random.value;
        normalMoveChance = 1 - (cardinalMoveChance + diagonalMoveChance);
        if (randomValue < normalMoveChance)
        {
            // Direct Path
            agent.SetDestination(target.transform.position);
        }
        else if (randomValue > normalMoveChance && randomValue < normalMoveChance + cardinalMoveChance)
        {
            // Cardinal Directions Only.
            Vector3 targetPosition = transform.position;

            Vector3 direction = target.position - transform.position;

            if (Mathf.Abs(direction.x) > 0.05f && Mathf.Abs(direction.z) > 0.05f)
            {
                if (Random.value < 0.5f)
                {
                    targetPosition.x = target.position.x + Mathf.Sign(direction.x) * 1f;
                }
                else
                {
                    targetPosition.z = target.position.z + Mathf.Sign(direction.z) * 1f;
                }
            }
            else if (Mathf.Abs(direction.x) >= 0.05f)
            {
                targetPosition.x = target.position.x + Mathf.Sign(direction.x) * 1f;
            }
            else if (Mathf.Abs(direction.z) >= 0.05f)
            {
                targetPosition.z = target.position.z + Mathf.Sign(direction.z) * 1f;
            }

            agent.SetDestination(targetPosition);
        }
        else if (randomValue < normalMoveChance + cardinalMoveChance + diagonalMoveChance)
        {
            // Diagonal Directions Only.
            Vector3 targetPosition = transform.position;
            Vector3 direction = target.position - transform.position;

            if (Mathf.Abs(direction.x) > 0.05f && Mathf.Abs(direction.z) > 0.05f)
            {
                float min = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.z)) + 1f;
                targetPosition.x += Mathf.Sign(direction.x) * min;
                targetPosition.z += Mathf.Sign(direction.z) * min;
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
        else
        {
            Debug.Log("MOVEMENT FAILED!!!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
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
