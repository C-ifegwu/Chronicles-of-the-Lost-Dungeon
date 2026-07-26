using UnityEngine;
using System.Collections;

public class SpecialAbility : MonoBehaviour, IAbility
{
    [Header("Special Stats (Shield Attack)")]
    [SerializeField] private int damageAmount = 45;
    [SerializeField] private float attackRange = 3.0f;
    [SerializeField] private float attackAngle = 180f; // Wider hit area
    
    [Tooltip("Time in seconds before the shield bash hits.")]
    [SerializeField] private float damageDelay = 0.5f;

    [Header("Auto-Aim / Target Lock")]
    [SerializeField] private float autoAimRadius = 6.0f;

    private PlayerAnimator playerAnimator;
    private bool isAttacking = false;

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    public void Execute()
    {
        if (isAttacking) return;

        AutoAimAtNearestEnemy();

        if (playerAnimator != null)
        {
            // Triggers the "SpecialAttack" parameter you have in your screenshot
            playerAnimator.TriggerSpecial(); 
        }

        StartCoroutine(DamageDelayRoutine());
    }

    private IEnumerator DamageDelayRoutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(damageDelay);
        DetectAndDamageEnemies();
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }
    
    private void AutoAimAtNearestEnemy()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, autoAimRadius);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider col in nearbyColliders)
        {
            if (col.CompareTag("Enemy") || col.transform.root.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = col.transform;
                }
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 directionToEnemy = (nearestEnemy.position - transform.position).normalized;
            directionToEnemy.y = 0; 
            if (directionToEnemy != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToEnemy);
            }
        }
    }

    private void DetectAndDamageEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Enemy") || hit.transform.root.CompareTag("Enemy"))
            {
                Vector3 directionToHit = (hit.transform.position - transform.position).normalized;
                directionToHit.y = 0;
                float angle = Vector3.Angle(transform.forward, directionToHit);

                if (angle <= attackAngle / 2f)
                {
                    IDamageable damageable = hit.GetComponentInParent<IDamageable>();
                    if (damageable != null) damageable.TakeDamage(damageAmount);
                }
            }
        }
    }
}