using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IDamageable
{
    public enum CombatStyle { Melee, Laser }

    [Header("Stats")]
    public int maxHealth = 50;
    public int attackDamage = 10;
    private int currentHealth;

    [Header("Targeting")]
    public Transform target; 
    
    [Header("Combat Settings")]
    public CombatStyle currentCombatStyle = CombatStyle.Melee;
    
    [Header("Melee Setup (Ignore if Laser)")]
    [Tooltip("Add all your attack trigger names here. The AI will pick one randomly.")]
    public string[] attackTriggerNames = { "OrcAttack" };

    [Header("Laser Setup (Ignore if Melee)")]
    public LineRenderer laserRenderer;
    public Transform laserOrigin;
    public float laserDuration = 0.2f;

    [Header("Death Settings")]
    public string deathTriggerName = "Die";
    
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    
    private IEnemyState currentState;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (laserRenderer != null) laserRenderer.enabled = false;

        if (target == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null) target = player.transform;
        }

        ChangeState(new EnemyIdleState());
    }

    private void Update()
    {
        if (isDead) return; // Stop all logic if dead

        if (currentState != null) currentState.UpdateState(this);

        if (animator != null && agent != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (isDead) return;

        if (currentState != null) currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    // --- MELEE LOGIC ---
    public void PerformMeleeAttack()
    {
        if (animator != null && attackTriggerNames.Length > 0) 
        {
            // Pick a random number between 0 and the total number of attacks in the list
            int randomIndex = Random.Range(0, attackTriggerNames.Length);
            string chosenAttack = attackTriggerNames[randomIndex];
            
            if (!string.IsNullOrEmpty(chosenAttack))
            {
                animator.SetTrigger(chosenAttack);
            }
        }
        
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(attackDamage);
    }

    // --- LASER LOGIC ---
    public void FireLaser()
    {
        if (laserRenderer == null || laserOrigin == null || target == null) return;
        StartCoroutine(ShootLaserRoutine());
    }

    private IEnumerator ShootLaserRoutine()
    {
        laserRenderer.enabled = true;
        laserRenderer.SetPosition(0, laserOrigin.position);
        
        Vector3 targetCenter = target.position + Vector3.up * 1.0f;
        laserRenderer.SetPosition(1, targetCenter);

        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(attackDamage);

        yield return new WaitForSeconds(laserDuration);
        laserRenderer.enabled = false;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        isDead = true;

        // Stop the NavMeshAgent so the corpse doesn't keep sliding toward the King
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // Trigger the death animation
        if (animator != null && !string.IsNullOrEmpty(deathTriggerName))
        {
            animator.SetTrigger(deathTriggerName);
        }
        
        // Remove the collider so the King can walk over the body
        Collider coll = GetComponent<Collider>();
        if (coll != null) coll.enabled = false;

        // Wait 4 seconds for the animation to finish, then destroy the body
        Destroy(gameObject, 4f); 
    }
}