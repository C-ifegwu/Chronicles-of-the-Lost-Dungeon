using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        // Ensure the agent is allowed to move
        if (enemy.agent != null && enemy.agent.isOnNavMesh)
        {
            enemy.agent.isStopped = false;
        }
    }

    public void UpdateState(EnemyController enemy)
    {
        if (enemy.target != null && enemy.agent != null && enemy.agent.isOnNavMesh)
        {
            // Constantly update the destination to the King's position
            enemy.agent.SetDestination(enemy.target.position);

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.position);
            
            // If the King runs away and gets further than 20 units, give up and go Idle
            if (distanceToPlayer > 20f)
            {
                enemy.ChangeState(new EnemyIdleState());
            }
        }
    }

    public void ExitState(EnemyController enemy)
    {
        // Clear the active path when leaving the Chase state
        if (enemy.agent != null && enemy.agent.isOnNavMesh)
        {
            enemy.agent.ResetPath();
        }
    }
}