using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [Header("Combat Settings")]
    public float damage = 10;
    public float attackRange = 2;
    public float attackCooldownTime = 2f;
    private float attackCooldownTracker = 0;

    [Header("Game Object Assignment")]
    public LayerMask targetLayer;
    public GameObject closestCell;
    public GameObject closestEnemy;

    // Private Declarations
    private NavMeshAgent m_Agent;
    private Animator m_animator;
    private float radius = 1000f;
    private float closestDistance = 1000f;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_animator = transform.Find("AnimatedEnemy").GetComponent<Animator>();

        HandleTargeting();
    }

    void Update()
    {
        if (closestEnemy == null)
        {
            HandleTargeting();
            return;
        }

        HandleAttack();
    }

    void HandleAttack()
    {
        // Check if the attack timer is off cooldown and decrement if not
        if (attackCooldownTracker > 0)
        {
            attackCooldownTracker -= Time.deltaTime;
            return;
        }

        // Check if the enemy is in range to attack the cell
        if ((m_Agent.destination - transform.position).magnitude < attackRange)
        {
            // Stop the tracker
            m_Agent.isStopped = true;

            // Stop walk animation
            m_animator.SetBool("isWalking", false);

            // Put the enemy into attacking state
            m_animator.SetTrigger("isAttacking");

            // Reset attack cooldown
            attackCooldownTracker = attackCooldownTime;

            // Ensure closestEnemy is valid before accessing it
            if (closestEnemy != null)
            {
                var healthManager = closestEnemy.transform.Find("NewCellBase").GetComponent<HealthManager>();

                healthManager.TakeDamage(damage);

                if (healthManager.currentHealth <= 0)
                {
                    // Destroy the closest enemy
                    Debug.Log("Destroying the enemy");
                    Destroy(closestEnemy);

                    closestEnemy = null; // Mark the enemy as null
                }
            }
        }
    }

    void HandleTargeting()
    {
        // Reactivate the agent
        m_Agent.isStopped = false;
        m_animator.SetBool("isWalking", true);

        // Reset the closest distance range
        closestDistance = radius;

        // Find all the game objects in the target layer
        Collider[] targets = Physics.OverlapSphere(Vector3.zero, radius, targetLayer);

        closestEnemy = null; // Reset closestEnemy before finding the new one

        // Find the closest target in the target list
        foreach (Collider target in targets)
        {
            float distanceFromTarget = (target.transform.position - transform.position).magnitude;

            Debug.Log($"{gameObject.name} is {distanceFromTarget} units away from {target.name}");

            if (distanceFromTarget < closestDistance)
            {
                closestEnemy = target.gameObject;
                closestDistance = distanceFromTarget;
            }
        }

        if (closestEnemy != null)
        {
            Debug.Log($"{closestEnemy.name} is the closest target at a distance of {closestDistance}");
            m_Agent.destination = closestEnemy.transform.position;
        }
        else
        {
            Debug.Log("No targets found!");
        }
    }
}
