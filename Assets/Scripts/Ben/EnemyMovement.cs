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
            // Move around circle of player

            // THIS CODE IS BROKEN BUT IT STILL WORKS GOOD AT WHAT IT IS TRYING TO DO (Which is making the enemy move around the player)

            Vector3 center = target.position;
            Vector3 offset = transform.position - center;

            float angle = 90 * Time.deltaTime;

            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            Vector3 rotatedOffset = new Vector3(
                offset.x * cos - offset.z * sin,
                0f,
                offset.x * sin + offset.z * cos
            );

            Vector3 targetPosition = center + rotatedOffset.normalized * offset.magnitude;

            agent.SetDestination(targetPosition);
        }
        else if (randomValue < normalMoveChance + cardinalMoveChance + diagonalMoveChance)
        {
            // Diagonal Directions Only.
            Vector3 targetPosition = target.position;

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
