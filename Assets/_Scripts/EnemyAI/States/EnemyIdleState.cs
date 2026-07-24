using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private float scanTimer = 0f;
    private float scanInterval = 1f; // Check for the player every 1 second

    public void EnterState(EnemyController enemy)
    {
        // Tell the navigation system to stop moving
        if (enemy.agent != null && enemy.agent.isOnNavMesh)
        {
            enemy.agent.isStopped = true;
        }
        scanTimer = 0f;
    }

    public void UpdateState(EnemyController enemy)
    {
        // Wait for the interval, then check distance to the King
        scanTimer += Time.deltaTime;
        if (scanTimer >= scanInterval)
        {
            scanTimer = 0f; // Reset timer

            if (enemy.target != null)
            {
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.position);
                
                // If the King is within 15 units, switch to the Chase State!
                if (distanceToPlayer < 15f)
                {
                    enemy.ChangeState(new EnemyChaseState());
                }
            }
        }
    }

    public void ExitState(EnemyController enemy)
    {
        // Allow the navigation system to move again as we exit Idle
        if (enemy.agent != null && enemy.agent.isOnNavMesh)
        {
            enemy.agent.isStopped = false;
        }
    }
}