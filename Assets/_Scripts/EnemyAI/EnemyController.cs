using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Targeting")]
    public Transform target; // This will be the King

    // The AI Navigation component
    [HideInInspector] public NavMeshAgent agent;
    
    // The current active behavior
    private IEnemyState currentState;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

        // Automatically find the player if not set in the inspector
        if (target == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                target = player.transform;
            }
        }
        // Start the enemy in the Idle State
        ChangeState(new EnemyIdleState());
    }

    private void Update()
    {
        // Run whatever state is currently active
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    /// <summary>
    /// Swaps the active state cleanly.
    /// </summary>
    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        
        currentState = newState;
        currentState.EnterState(this);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        Destroy(gameObject); // Simply remove the enemy for now
    }
}