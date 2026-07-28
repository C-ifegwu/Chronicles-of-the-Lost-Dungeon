using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IEnemyState
{
    private float waitTime = 2.5f;    // How many seconds they stand still after reaching a point
    private float waitTimer = 0f;
    private bool isWaiting = false;
    
    // The anchor point so they never drift away
    private Vector3 startPosition; 

    public void EnterState(EnemyController enemy)
    {
        // Save the exact spot you placed them in the Unity Editor
        startPosition = enemy.transform.position; 
        
        SetNewPatrolDestination(enemy);
    }

    public void UpdateState(EnemyController enemy)
    {
        // 1. CONSTANT THREAT CHECK: Did we see or hear the King?
        if (enemy.target != null)
        {
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
        // Stop walking instantly when exiting the patrol state
        if (enemy.agent != null && enemy.agent.isOnNavMesh)
        {
            enemy.agent.ResetPath();
        }
    }

    private void SetNewPatrolDestination(EnemyController enemy)
    {
        if (enemy.agent == null || !enemy.agent.isOnNavMesh) return;

        // Pick a random direction using the radius you set in the Inspector
        Vector3 randomDirection = Random.insideUnitSphere * enemy.patrolRadius;
        
        // Add it to the START position so they orbit their spawn point
        randomDirection += startPosition; 
        
        NavMeshHit hit;
        // Check if that random point is actually a walkable spot on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out hit, enemy.patrolRadius, 1))
        {
            enemy.agent.SetDestination(hit.position);
        }
    }
}