using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Stamina System")]
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] private float staminaRegenRate = 10f; // Adjusted for better pacing
    [SerializeField] private float meleeStaminaCost = 15f;
    [SerializeField] private float specialStaminaCost = 40f;
    
    // --- NEW: Exhaustion Delay Variables ---
    [Tooltip("How many seconds to wait after an attack before stamina starts recovering.")]
    [SerializeField] private float staminaRegenDelay = 1.2f; 
    private float regenTimer = 0f;

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
    
    private IInteractable currentInteractable = null;
    private float verticalVelocity;
    private Transform mainCameraTransform;

    private void Start()
    {
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
        
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
        
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        HandleMovement();
        HandleDefense(); 
        HandleStamina();
        HandleCombat();
        HandleInteraction(); 

        // Forces the StatManager to mirror the King's internal math
        if (StatManager.Instance != null)
        {
            StatManager.Instance.currentHealth = this.currentHealth;
            StatManager.Instance.currentStamina = this.currentStamina;
        }
    }

    private void HandleMovement()
    {
        if (InputManager.Instance == null) return;
        
        Vector2 moveInput = InputManager.Instance.MoveInput;
        Vector3 moveDirection = Vector3.zero;

        if (mainCameraTransform != null)
        {
            Vector3 cameraForward = mainCameraTransform.forward;
            Vector3 cameraRight = mainCameraTransform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
        }
        else
        {
            moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        }

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
        // --- NEW: Tick down the exhaustion timer first ---
        if (regenTimer > 0)
        {
            regenTimer -= Time.deltaTime;
            return; // Exit the method so no stamina is recovered this frame
        }

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
                regenTimer = staminaRegenDelay; // --- NEW: Reset the delay timer ---
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
                regenTimer = staminaRegenDelay; // --- NEW: Reset the delay timer ---
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
        
        if (characterController != null) characterController.enabled = false;

        StartCoroutine(RestartLevelRoutine());

        this.enabled = false;
    }

    private System.Collections.IEnumerator RestartLevelRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        if (currentStamina > maxStamina) currentStamina = maxStamina;
    }

    // --- ONLY THE THREE METHODS BELOW WERE CHANGED ---

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact(this);
                
                // Hide the UI banner once the item is picked up/used
                if (InteractionPromptUI.Instance != null)
                {
                    InteractionPromptUI.Instance.HidePrompt();
                }
                
                currentInteractable = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            
            // Send the interact text to your UI banner instead of the console
            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.ShowPrompt($"Press 'F' to: {currentInteractable.GetInteractText()}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            
            // Hide the UI banner when the King walks away from the item
            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.HidePrompt();
            }
        }
    }
}