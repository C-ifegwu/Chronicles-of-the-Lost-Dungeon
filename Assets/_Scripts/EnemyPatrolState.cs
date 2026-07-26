using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IEnemyState
{
    private float patrolRadius = 10f; // How far they will wander from their current spot
    private float waitTime = 2.5f;    // How many seconds they stand still after reaching a point
    private float waitTimer = 0f;
    private bool isWaiting = false;

    public void EnterState(EnemyController enemy)
    {
        SetNewPatrolDestination(enemy);
    }

    public void UpdateState(EnemyController enemy)
    {
        // 1. CONSTANT THREAT CHECK: Did we see or hear the King?
        if (enemy.target != null)
        {
            // Switch to your chase state (ensure this matches the exact name of your chase script!)
            enemy.ChangeState(new EnemyChaseState());
            return;
        }

        // 2. PATROL BEHAVIOR
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                SetNewPatrolDestination(enemy);
            }
        }
        else
        {
            // Check if we have arrived at our random destination
            if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
            {
                isWaiting = true;
                waitTimer = 0f;
            }
        }
    }

    public void ExitState(EnemyController enemy)
    {
        // Stop walking instantly when exiting the patrol state (e.g., when chasing the King)
        if (enemy.agent != null && enemy.agent.isOnNavMesh)
        {
            enemy.agent.ResetPath();
        }
    }

    private void SetNewPatrolDestination(EnemyController enemy)
    {
        if (enemy.agent == null || !enemy.agent.isOnNavMesh) return;

        // Pick a random direction and distance
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += enemy.transform.position;
        
        NavMeshHit hit;
        // Check if that random point is actually a walkable spot on the NavMesh floor
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            enemy.agent.SetDestination(hit.position);
        }
    }
}