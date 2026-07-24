public interface IEnemyState
{
    // Called once the moment the enemy enters this state
    void EnterState(EnemyController enemy);

    // Called every frame while the enemy is in this state
    void UpdateState(EnemyController enemy);

    // Called once right before the enemy switches to a different state
    void ExitState(EnemyController enemy);
}