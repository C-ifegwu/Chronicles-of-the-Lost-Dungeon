using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private float attackTimer = 0f;
    private float attackCooldown = 2f; 

    public void EnterState(EnemyController enemy)
    {
        if (enemy.agent != null && enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;
        attackTimer = attackCooldown; 
    }

    public void UpdateState(EnemyController enemy)
    {
        if (enemy.target == null) return;

        // Rotate to face the King
        Vector3 direction = (enemy.target.position - enemy.transform.position).normalized;
        direction.y = 0;
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            
            // Check the dropdown choice!
            if (enemy.currentCombatStyle == EnemyController.CombatStyle.Laser)
            {
                enemy.FireLaser();
                Debug.Log(enemy.gameObject.name + " fired a laser!");
            }
            else if (enemy.currentCombatStyle == EnemyController.CombatStyle.Melee)
            {
                enemy.PerformMeleeAttack();
                Debug.Log(enemy.gameObject.name + " swung a melee weapon!");
            }
        }

        // Return to chase if the King runs away
        if (Vector3.Distance(enemy.transform.position, enemy.target.position) > enemy.agent.stoppingDistance)
        {
            enemy.ChangeState(new EnemyChaseState());
        }
    }

    public void ExitState(EnemyController enemy)
    {
        if (enemy.agent != null && enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
    }
}