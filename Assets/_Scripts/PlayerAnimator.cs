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

        if (InputManager.Instance.MeleeTriggered) animator.SetTrigger("MeleeAttack");
        if (InputManager.Instance.SpecialTriggered) animator.SetTrigger("SpecialAttack");
        
        if (InputManager.Instance.JumpTriggered) animator.SetTrigger("Jump");
        if (InputManager.Instance.DodgeTriggered) animator.SetTrigger("Dodge");
    }

    public void SetAnimationSpeed(float newSpeedMultiplier)
    {
        globalAnimationSpeed = newSpeedMultiplier;
        if (animator != null) animator.speed = globalAnimationSpeed;
    }

    public void TriggerHit()
    {
        if (animator != null) animator.SetTrigger("GetHit");
    }

    public void TriggerDeath()
    {
        if (animator != null) animator.SetTrigger("Death");
    }
}