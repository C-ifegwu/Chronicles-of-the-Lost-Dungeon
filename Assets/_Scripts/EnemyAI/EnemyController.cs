using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[System.Serializable]
public class MeleeAttackConfig
{
    public string triggerName;
    [Tooltip("How many seconds to wait before this specific attack deals damage.")]
    public float damageDelay = 0.5f;
}

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IDamageable
{
    public enum CombatStyle { Melee, Laser }

    [Header("Stats")]
    public int maxHealth = 50;
    public int attackDamage = 10;
    private int currentHealth;

    [Header("UI Bridge")]
    public BossEnemy bossUI;
    public FloatingHealthBar floatingUI;
    public BossDefeatNotifier bossNotifier; 

    [Header("Damage Popups")]
    public GameObject damagePopupPrefab;

    [Header("Targeting & Vision")]
    public float patrolRadius = 10f; 
    [HideInInspector] public Transform target; 
    public float detectionRadius = 15f;
    public float fieldOfViewAngle = 140f;
    private Transform playerTransform;
    
    [Header("Combat Settings")]
    public CombatStyle currentCombatStyle = CombatStyle.Melee;
    public float attackCooldown = 2.0f;
    
    [Header("Melee Setup (Ignore if Laser)")]
    public MeleeAttackConfig[] meleeAttacks; 

    [Header("Laser Setup (Ignore if Melee)")]
    public LineRenderer laserRenderer;
    public Transform laserOrigin;
    public float laserDuration = 0.2f;

    [Header("Hit & Death Settings")]
    public string hitTriggerName = ""; 
    public float stunDuration = 1.0f;
    public string deathTriggerName = "Die";
    
    [Header("Vanish Settings")]
    public GameObject deathVFX; 
    public float bodyVanishDelay = 4.0f; 

    // --- NEW: Enemy 3D Audio Setup ---
    [Header("Enemy Audio")]
    public AudioSource enemyAudioSource;
    public AudioClip attackRoarSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    
    private IEnemyState currentState;
    private bool isDead = false;
    private bool canAttack = true; 

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (laserRenderer != null) laserRenderer.enabled = false;

        PlayerController player = Object.FindAnyObjectByType<PlayerController>();
        if (player != null) playerTransform = player.transform;

        if (bossUI == null)
        {
            bossUI = Object.FindAnyObjectByType<BossEnemy>();
        }
        if (floatingUI == null)
        {
            floatingUI = GetComponentInChildren<FloatingHealthBar>() ?? Object.FindAnyObjectByType<FloatingHealthBar>();
        }
        if (bossNotifier == null)
        {
            bossNotifier = GetComponent<BossDefeatNotifier>() ?? Object.FindAnyObjectByType<BossDefeatNotifier>();
        }

        // --- NEW: Auto-configure 3D Audio Source ---
        if (enemyAudioSource == null) enemyAudioSource = GetComponent<AudioSource>();
        if (enemyAudioSource == null) enemyAudioSource = gameObject.AddComponent<AudioSource>();
        enemyAudioSource.playOnAwake = false;
        enemyAudioSource.spatialBlend = 1f; // Forces the sound into 3D space

        if (bossUI != null) bossUI.ActivateBossUI(maxHealth);
        if (floatingUI != null) floatingUI.UpdateHealth(maxHealth, maxHealth);

        ChangeState(new EnemyPatrolState());
    }

    private void Update()
    {
        if (isDead) return;

        if (target == null && playerTransform != null)
        {
            LookForKing();
        }

        if (currentState != null) currentState.UpdateState(this);

        if (animator != null && agent != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private void LookForKing()
    {
        float distanceToKing = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToKing <= detectionRadius)
        {
            Vector3 directionToKing = (playerTransform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToKing);

            if (angle <= fieldOfViewAngle / 2f)
            {
                target = playerTransform;
            }
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (isDead) return;

        if (currentState != null) currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void PerformMeleeAttack()
    {
        if (!canAttack) return;
        canAttack = false; 

        // --- NEW: Play Attack Roar ---
        if (enemyAudioSource != null && attackRoarSound != null)
        {
            enemyAudioSource.PlayOneShot(attackRoarSound);
        }

        float currentDamageDelay = 0.5f; 

        if (animator != null && meleeAttacks.Length > 0) 
        {
            int randomIndex = Random.Range(0, meleeAttacks.Length);
            MeleeAttackConfig chosenAttack = meleeAttacks[randomIndex];
            
            if (!string.IsNullOrEmpty(chosenAttack.triggerName))
            {
                animator.SetTrigger(chosenAttack.triggerName);
                currentDamageDelay = chosenAttack.damageDelay; 
            }
        }
        
        StartCoroutine(DamageDelayRoutine(currentDamageDelay));
        StartCoroutine(ResetAttackRoutine());
    }

    private IEnumerator DamageDelayRoutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (target == null || isDead) yield break;

        float distanceToKing = Vector3.Distance(transform.position, target.position);
        
        if (distanceToKing <= agent.stoppingDistance + 3.0f) 
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null) damageable.TakeDamage(attackDamage);
        }
    }

    private IEnumerator ResetAttackRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void FireLaser()
    {
        if (!canAttack) return;
        canAttack = false;

        // --- NEW: Play Attack Roar for Laser ---
        if (enemyAudioSource != null && attackRoarSound != null)
        {
            enemyAudioSource.PlayOneShot(attackRoarSound);
        }

        if (laserRenderer == null || laserOrigin == null || target == null) return;
        StartCoroutine(ShootLaserRoutine());
        StartCoroutine(ResetAttackRoutine());
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

        if (target == null && playerTransform != null)
        {
            target = playerTransform;
        }

        currentHealth -= damageAmount;
        
        if (bossUI != null) bossUI.UpdateHealthBar(currentHealth);
        if (floatingUI != null) floatingUI.UpdateHealth(currentHealth, maxHealth);

        if (damagePopupPrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 2f; 
            GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            
            DamagePopUp popupScript = popup.GetComponent<DamagePopUp>();
            if (popupScript != null)
            {
                popupScript.Setup(damageAmount);
            }
        }
        
        if (currentHealth <= 0) 
        {
            Die();
        }
        else
        {
            // --- NEW: Play Hurt Sound ---
            if (enemyAudioSource != null && hurtSound != null)
            {
                enemyAudioSource.PlayOneShot(hurtSound);
            }

            StopAllCoroutines();
            
            if (animator != null && !string.IsNullOrEmpty(hitTriggerName))
            {
                animator.SetTrigger(hitTriggerName);
            }
            
            StartCoroutine(StunRoutine());
        }
    }

    private IEnumerator StunRoutine()
    {
        if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
        canAttack = false;
        
        if (laserRenderer != null) laserRenderer.enabled = false;

        yield return new WaitForSeconds(stunDuration);

        if (isDead) yield break;

        if (agent != null && agent.isOnNavMesh) agent.isStopped = false;
        canAttack = true; 
    }

    public void Die()
    {
        // --- NEW: Play Death Sound ---
        if (enemyAudioSource != null && deathSound != null)
        {
            enemyAudioSource.PlayOneShot(deathSound);
        }

        // --- NEW: Notify Victory Manager that an enemy was slain! ---
        if (VictoryManager.Instance != null)
        {
            VictoryManager.Instance.AddEnemyKilled();
        }
        // ------------------------------------------------------------

        if (bossUI != null) 
        {
            bossUI.HideBossUI();
        }

        if (bossNotifier != null) 
        {
            bossNotifier.NotifyBossDefeated();
        }
        
        if (GetComponent<ItemDrop>() != null) GetComponent<ItemDrop>().DropItem();
        
        LootDrop loot = GetComponent<LootDrop>();
        if (loot != null)
        {
            loot.TryDropLoot();
        }
        
        isDead = true;
        StopAllCoroutines(); 

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (animator != null && !string.IsNullOrEmpty(deathTriggerName))
        {
            animator.SetTrigger(deathTriggerName);
        }
        
        Collider coll = GetComponent<Collider>();
        if (coll != null) coll.enabled = false;

        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position + Vector3.up, Quaternion.identity);
        }

        Destroy(gameObject, bodyVanishDelay); 
    }
    
    public void HearNoise(Transform noiseSource)
    {
        if (isDead) return;
        
        if (target == null)
        {
            target = noiseSource;
        }
    }

    public void ApplyKnockback(Vector3 attackerPosition, float force)
    {
        if (isDead || agent == null || !agent.isOnNavMesh) return;
        
        Vector3 pushDirection = (transform.position - attackerPosition).normalized;
        pushDirection.y = 0; 
        
        agent.Move(pushDirection * force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}