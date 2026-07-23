using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 15f;

    private CharacterController characterController;
    private IAbility currentMeleeAbility;
    private IAbility currentRangedAbility;

    private void Start()
    {
        currentHealth = maxHealth;
        characterController = GetComponent<CharacterController>();
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
        if (InputManager.Instance == null) return;
        
        Vector2 moveInput = InputManager.Instance.MoveInput;
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (characterController != null && moveDirection != Vector3.zero)
        {
            // Apply slight artificial gravity to keep the CharacterController grounded
            moveDirection.y = -0.1f; 
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // Lock rotation strictly to the Y axis
        moveDirection.y = 0; 
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleCombat()
    {
        if (InputManager.Instance == null) return;

        if (InputManager.Instance.MeleeTriggered && currentMeleeAbility != null)
        {
            currentMeleeAbility.Execute();
        }
        else if (InputManager.Instance.RangedTriggered && currentRangedAbility != null)
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