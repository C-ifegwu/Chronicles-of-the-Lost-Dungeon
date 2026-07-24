using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (InputManager.Instance == null || animator == null) return;

        // 1. Handle Movement Speed
        float currentSpeed = InputManager.Instance.MoveInput.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        // 2. Handle Blocking Stance
        animator.SetBool("IsBlocking", InputManager.Instance.IsBlocking);

        // 3. Handle Attack Triggers
        if (InputManager.Instance.MeleeTriggered)
        {
            animator.SetTrigger("MeleeAttack");
        }

        if (InputManager.Instance.SpecialTriggered)
        {
            animator.SetTrigger("SpecialAttack");
        }
    }
}