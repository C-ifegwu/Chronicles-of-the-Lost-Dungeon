using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Defense")]
    public bool isBlocking = false;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;

    private CharacterController characterController;
    private PlayerAnimator playerAnimator;
    private Animator animator; // Added to access the Animator directly for the block boolean
    private IAbility currentMeleeAbility;
    private IAbility currentRangedAbility;

    private float verticalVelocity;

    private void Start()
    {
        currentHealth = maxHealth;
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
        animator = GetComponent<Animator>(); // Fetch Animator for block state
        
        currentMeleeAbility = GetComponent<MeleeAbility>();
        currentRangedAbility = GetComponent<RangedAbility>();
        
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        HandleMovement();
        HandleDefense(); // Checks for right-click input
        HandleCombat();
    }

    private void HandleMovement()
    {
        if (InputManager.Instance == null) return;
        
        Vector2 moveInput = InputManager.Instance.MoveInput;
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Apply real physics and gravity
        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f; // Stick to the floor
            if (InputManager.Instance.JumpTriggered)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if (characterController != null)
        {
            float activeSpeed = InputManager.Instance.IsSprinting ? walkSpeed * sprintMultiplier : walkSpeed;
            Vector3 finalMovement = moveDirection * activeSpeed;
            finalMovement.y = verticalVelocity;
            
            characterController.Move(finalMovement * Time.deltaTime);
        }

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleDefense()
    {
        // Right-Click pressed (Shield Up)
        if (Input.GetMouseButtonDown(1)) 
        {
            isBlocking = true;
            if (animator != null)
            {
                animator.SetBool("IsBlocking", true);
            }
        }
        // Right-Click released (Shield Down)
        else if (Input.GetMouseButtonUp(1)) 
        {
            isBlocking = false;
            if (animator != null)
            {
                animator.SetBool("IsBlocking", false);
            }
        }
    }

    private void HandleCombat()
    {
        if (InputManager.Instance == null) return;

        // Optional: Prevent initiating new attacks while actively blocking
        if (isBlocking) return; 

        if (InputManager.Instance.MeleeTriggered && currentMeleeAbility != null) currentMeleeAbility.Execute();
        else if (InputManager.Instance.SpecialTriggered && currentRangedAbility != null) currentRangedAbility.Execute();
    }

    public void TakeDamage(int damageAmount)
    {
        // 1. Intercept the attack if the King is blocking
        if (isBlocking)
        {
            Debug.Log("Attack Blocked! No health lost.");
            
            // Trigger the visual reaction so the player feels the impact of the block
            if (playerAnimator != null)
            {
                playerAnimator.TriggerHit(); 
            }
            
            // Immediately exit the method before any health math occurs
            return; 
        }

        // 2. Normal damage logic
        currentHealth -= damageAmount;
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth > 0)
        {
            playerAnimator.TriggerHit();
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        playerAnimator.TriggerDeath();
        GameEvents.OnPlayerDied?.Invoke();
        Debug.Log("The King has fallen!");
        
        // Disable this script so the dead body cannot be driven by WASD anymore
        this.enabled = false;
    }
}