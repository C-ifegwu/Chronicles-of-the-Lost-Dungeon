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
        // Safety check to prevent silent errors
        if (InputManager.Instance == null || animator == null) return;

        // .magnitude converts the Vector2 (X, Y) into a single positive number
        // It will be 0 when standing still, and greater than 0 when moving
        float currentSpeed = InputManager.Instance.MoveInput.magnitude;
        
        // Feed that number directly into the Animator parameter we created
        animator.SetFloat("Speed", currentSpeed);
    }
}