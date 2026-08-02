using UnityEngine;
using System.Collections;

public class RangedAbility : MonoBehaviour, IAbility
{
    [Header("Ranged Stats")]
    [SerializeField] private int damageAmount = 15;
    [SerializeField] private float attackRange = 12f;

    [Tooltip("How many seconds to wait before the shot deals damage.")]
    [SerializeField] private float damageDelay = 0.3f;

    [Header("Auto-Aim / Target Lock")]
    [SerializeField] private float autoAimRadius = 12f;

    [Header("Visuals")]
    [Tooltip("Optional beam that visualizes the shot (same idea as the enemy's laser). Leave empty to skip the effect.")]
    [SerializeField] private LineRenderer shotRenderer;
    [SerializeField] private float shotVisibleDuration = 0.15f;

    private PlayerAnimator playerAnimator;
    private bool isAttacking = false;
    private Transform lockedTarget;

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        if (shotRenderer != null) shotRenderer.enabled = false;
    }

    public void Execute()
    {
        if (isAttacking) return;

        AutoAimAtNearestEnemy();

        if (playerAnimator != null)
        {
            playerAnimator.TriggerRanged();
        }

        StartCoroutine(DamageDelayRoutine());
    }

    private IEnumerator DamageDelayRoutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(damageDelay);

        FireAtTarget();

        yield return new WaitForSeconds(0.2f);
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
                    nearestEnemy = col.transform.root;
                }
            }
        }

        lockedTarget = nearestEnemy;

        if (lockedTarget != null)
        {
            Vector3 directionToEnemy = (lockedTarget.position - transform.position).normalized;
            directionToEnemy.y = 0;

            if (directionToEnemy != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToEnemy);
            }
        }
    }

    private void FireAtTarget()
    {
        Vector3 originPoint = transform.position + Vector3.up * 1.4f;
        Vector3 endPoint = originPoint + transform.forward * attackRange;

        if (lockedTarget != null && Vector3.Distance(transform.position, lockedTarget.position) <= attackRange)
        {
            endPoint = lockedTarget.position + Vector3.up * 1.0f;

            IDamageable damageable = lockedTarget.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
            }

            EnemyController enemy = lockedTarget.GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                enemy.ApplyKnockback(transform.position, 1.0f);
            }
        }

        if (shotRenderer != null)
        {
            StartCoroutine(ShowShotRoutine(originPoint, endPoint));
        }
    }

    private IEnumerator ShowShotRoutine(Vector3 start, Vector3 end)
    {
        shotRenderer.enabled = true;
        shotRenderer.SetPosition(0, start);
        shotRenderer.SetPosition(1, end);

        yield return new WaitForSeconds(shotVisibleDuration);

        shotRenderer.enabled = false;
    }
}
