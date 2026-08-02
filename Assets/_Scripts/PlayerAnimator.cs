using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    [Header("Speed Settings")]
    public float globalAnimationSpeed = 1.2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = globalAnimationSpeed;
        }
    }

    private void Update()
    {
        if (InputManager.Instance == null || animator == null) return;

        animator.speed = globalAnimationSpeed;

        float animationSpeedState = 0f;
        if (InputManager.Instance.MoveInput.magnitude > 0.1f)
        {
            animationSpeedState = InputManager.Instance.IsSprinting ? 2f : 1f;
        }
        animator.SetFloat("Speed", animationSpeedState);

        animator.SetBool("IsBlocking", InputManager.Instance.IsBlocking);

        // Movement triggers
        if (InputManager.Instance.JumpTriggered) animator.SetTrigger("Jump");
        if (InputManager.Instance.DodgeTriggered) animator.SetTrigger("Dodge");
    }

    public void SetAnimationSpeed(float newSpeedMultiplier)
    {
        globalAnimationSpeed = newSpeedMultiplier;
        if (animator != null) animator.speed = globalAnimationSpeed;
    }

    // --- NEW COMBAT COMMANDS ---
    public void TriggerMelee()
    {
        if (animator != null) animator.SetTrigger("MeleeAttack");
    }
    
    public void TriggerSpecial()
    {
        if (animator != null) animator.SetTrigger("SpecialAttack");
    }

    // --- NEW: Ranged attack command (safe no-op if the Animator Controller doesn't
    // have a "RangedAttack" trigger parameter yet). ---
    public void TriggerRanged()
    {
        if (animator != null) animator.SetTrigger("RangedAttack");
    }

    // --- REACTION COMMANDS ---
    public void TriggerHit()
    {
        if (animator != null) animator.SetTrigger("GetHit");
    }

    public void TriggerDeath()
    {
        if (animator != null) animator.SetTrigger("Death");
    }
}