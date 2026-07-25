using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        if (enemy.agent != null && enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
    }

    public void UpdateState(EnemyController enemy)
    {
        if (enemy.target != null && enemy.agent != null && enemy.agent.isOnNavMesh)
        {
            enemy.agent.SetDestination(enemy.target.position);

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.position);
            
            if (distanceToPlayer <= enemy.agent.stoppingDistance)
            {
                enemy.ChangeState(new EnemyAttackState());
            }
            else if (distanceToPlayer > 20f)
            {
                enemy.ChangeState(new EnemyIdleState());
            }
        }
    }

    public void ExitState(EnemyController enemy)
    {
        if (enemy.agent != null && enemy.agent.isOnNavMesh) enemy.agent.ResetPath();
    }
}