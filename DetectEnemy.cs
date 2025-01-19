using UnityEngine;

public class DetectEnemy : MonoBehaviour
{

    [Header("Tower Settings")]
    public float range = 100f;
    public float damage = 10f;
    public float currentCooldown = 0f;
    public float maxCooldown = 3f;
    public LayerMask enemyLayer;
    public GameObject bulletPrefab;
    public Transform shootPosition;

    public bool isEnabled = true;


    void Update()
    {
        if (isEnabled)
        {
            CheckEnemies();
            UpdateCooldowns();
        }
    }

    void CheckEnemies()
    {
        float closestRange = range + 1;

        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, range, enemyLayer);

        foreach (Collider collider in enemyColliders)
        {
            AttackEnemy(collider);
        }

    }

    void AttackEnemy(Collider enemyCollider)
    {
        // Reference Enemy script which contains health information
        Enemy enemy = enemyCollider.GetComponent<Enemy>();

        // Reference the enemy's transform
        Transform enemyTransform = enemyCollider.GetComponent<Transform>();

        // Get the distance from the enemy
        float distance = (enemyTransform.position - transform.position).magnitude;

        // Get the direction to the enemy
        Vector3 direction = enemyTransform.position - transform.position;

        // Calculate the direction to look
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 rotation = lookRotation.eulerAngles;

        // Apply the rotation
        transform.Find("Gun").rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (currentCooldown <= 0)
        {
            // Create the bullet prefab
            CreateBullet(enemyTransform);

            // Apply damage to the enemy
            enemy.currentHealth -= damage;

            // Reset the attack cooldown
            currentCooldown = maxCooldown;
        }
    }

    void UpdateCooldowns()
    {
        if (currentCooldown > 0 )
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void CreateBullet(Transform target)
    {
        // Create a bullet object
        if ( bulletPrefab == null )
        {
            Debug.Log("No bullet prefab provided");
            return;
        }

        else if ( shootPosition == null)
        {
            Debug.Log("No shoot position provided");
            return;
        }

        GameObject bulletPrefabGO = (GameObject)Instantiate(bulletPrefab, shootPosition.position, Quaternion.identity);

        BulletSeek bulletScript = bulletPrefabGO.GetComponent<BulletSeek>();

        bulletScript.SetTarget(target);

    }

    void OnMouseEnter()
    {
        transform.Find("rangeSphere").gameObject.SetActive(true);
    }


    void OnMouseExit()
    {
        transform.Find("rangeSphere").gameObject.SetActive(false);
    }

}
