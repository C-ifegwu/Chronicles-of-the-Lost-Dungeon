using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IDamageable
{
    // This creates the dropdown menu in the Inspector!
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
    public string attackTriggerName = "RightAttack";

    [Header("Laser Setup (Ignore if Melee)")]
    public LineRenderer laserRenderer;
    public Transform laserOrigin;
    public float laserDuration = 0.2f;
    
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    
    private IEnemyState currentState;

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
        if (currentState != null) currentState.UpdateState(this);

        if (animator != null && agent != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null) currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    // --- MELEE LOGIC ---
    public void PerformMeleeAttack()
    {
        if (animator != null && !string.IsNullOrEmpty(attackTriggerName)) 
        {
            animator.SetTrigger(attackTriggerName);
        }
        
        // Apply damage instantly for now
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
        currentHealth -= damageAmount;
        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        Destroy(gameObject); 
    }
}