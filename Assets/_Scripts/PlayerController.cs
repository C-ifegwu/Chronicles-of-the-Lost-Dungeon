using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    private IAbility currentMeleeAbility;
    private IAbility currentRangedAbility;
    private InputManager inputManager;

    private void Start()
    {
        currentHealth = maxHealth;
        inputManager = InputManager.Instance;
        
        currentMeleeAbility = GetComponent<MeleeAbility>();
        currentRangedAbility = GetComponent<RangedAbility>();
        
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
        
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }
    }

    private void HandleCombat()
    {
        if (inputManager == null) return;

        // Uses the new foolproof triggered variables
        if (inputManager.MeleeTriggered && currentMeleeAbility != null)
        {
            currentMeleeAbility.Execute();
        }
        else if (inputManager.RangedTriggered && currentRangedAbility != null)
        {
            currentRangedAbility.Execute();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameEvents.OnPlayerDied?.Invoke();
        Debug.Log("The King has fallen!");
    }
}