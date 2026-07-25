using UnityEngine;

public class AttackStateBehaviour : StateMachineBehaviour
{
    // This runs the exact frame the attack animation starts
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsAttacking", true);
    }

    // This runs the exact frame the attack animation finishes
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsAttacking", false);
    }
}