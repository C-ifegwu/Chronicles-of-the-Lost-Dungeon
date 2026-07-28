using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Stamina System")]
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] private float staminaRegenRate = 15f;
    [SerializeField] private float meleeStaminaCost = 15f;
    [SerializeField] private float specialStaminaCost = 40f;

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
    private Animator animator; 
    private IAbility currentMeleeAbility;
    private IAbility currentSpecialAbility; 

    private float verticalVelocity;

    private void Start()
    {
        // --- NEW: Pull max health from the StatManager if it exists ---
        if (StatManager.Instance != null)
        {
            maxHealth = (int)StatManager.Instance.currentMaxHealth;
        }
        
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
        animator = GetComponent<Animator>(); 
        
        currentMeleeAbility = GetComponent<MeleeAbility>();
        currentSpecialAbility = GetComponent<SpecialAbility>(); 
        
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        HandleMovement();
        HandleDefense(); 
        HandleStamina();
        HandleCombat();
    }

    private void HandleMovement()
    {
        if (InputManager.Instance == null) return;
        
        Vector2 moveInput = InputManager.Instance.MoveInput;
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f; 
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
        if (Input.GetMouseButtonDown(1)) 
        {
            isBlocking = true;
            if (animator != null) animator.SetBool("IsBlocking", true);
        }
        else if (Input.GetMouseButtonUp(1)) 
        {
            isBlocking = false;
            if (animator != null) animator.SetBool("IsBlocking", false);
        }
    }

    private void HandleStamina()
    {
        if (!isBlocking && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }
    }

    private void HandleCombat()
    {
        if (InputManager.Instance == null) return;
        if (isBlocking) return; 

        if (InputManager.Instance.MeleeTriggered && currentMeleeAbility != null)
        {
            if (currentStamina >= meleeStaminaCost)
            {
                currentStamina -= meleeStaminaCost;
                currentMeleeAbility.Execute();
                MakeNoise(8f); 
            }
            else
            {
                Debug.Log("Not enough stamina for a melee attack!");
            }
        }
        else if (InputManager.Instance.SpecialTriggered && currentSpecialAbility != null)
        {
            if (currentStamina >= specialStaminaCost)
            {
                currentStamina -= specialStaminaCost;
                currentSpecialAbility.Execute();
                MakeNoise(15f); 
            }
            else
            {
                Debug.Log("Not enough stamina for a shield bash!");
            }
        }
    }

    private void MakeNoise(float noiseRadius)
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, noiseRadius);
        foreach (Collider col in nearbyColliders)
        {
            if (col.CompareTag("Enemy") || col.transform.root.CompareTag("Enemy"))
            {
                EnemyController enemy = col.GetComponentInParent<EnemyController>();
                if (enemy != null)
                {
                    enemy.HearNoise(transform);
                }
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isBlocking)
        {
            Debug.Log("Attack Blocked! No health lost and no flinch.");
            return; 
        }

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
        
        this.enabled = false;
    }
}