using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    // Strategy Pattern references
    private IAbility currentMeleeAbility;
    private IAbility currentRangedAbility;
    private InputManager inputManager;

    private void Start()
    {
        currentHealth = maxHealth;
        inputManager = InputManager.Instance;
        
        // Fetch abilities attached to the player (Strategy Pattern)
        currentMeleeAbility = GetComponent<MeleeAbility>();
        currentRangedAbility = GetComponent<RangedAbility>();
        
        // Update the UI immediately
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        HandleMovement();
        HandleCombat();
    }

    private void HandleMovement()
    {
        if (inputManager == null) return;
        
        Vector2 moveInput = inputManager.MoveInput;
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        
        // Simple translation for now; we will upgrade to Rigidbody/CharacterController later
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Face the direction of movement
        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }
    }

    private void HandleCombat()
    {
        if (inputManager == null) return;

        if (inputManager.IsMeleeAttacking && currentMeleeAbility != null)
        {
            currentMeleeAbility.Execute();
        }
        else if (inputManager.IsRangedAttacking && currentRangedAbility != null)
        {
            currentRangedAbility.Execute();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        
        // Fire event to update the UI
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Fire event to show Game Over screen
        GameEvents.OnPlayerDied?.Invoke();
        Debug.Log("The King has fallen!");
    }
}