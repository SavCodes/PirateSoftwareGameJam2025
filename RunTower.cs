using UnityEngine;

public class RunTower : MonoBehaviour
{

    [Header("Combat Settings")]
    public float range = 100f;
    public float damage = 10f;
    public float currentCooldown = 0f;
    public float maxCooldown = 3f;
    public bool isEnabled = true;
    public bool hitBoxEnabled = false;
    public LayerMask enemyLayer;
    public GameObject bulletPrefab;
    public Transform shootPosition;


    [Header("Bobbing and Rotation Settings")]
    public float bobAmplitude = 0.5f;
    public float rotationSpeed = 2f;
    public float slowFactor = 1f;
    public GameObject target;
    private Vector3 originalPos;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public float minPitch = 0.90f;
    public float maxPitch = 1.10f;

    [Header("Shop Settings")]
    private BuilderManager builderManager;
    public bool upgradeUIEnabled = false;
    public float aminoAcidCost = 100f;
    public float atpCost = 0f;
    public float dnaCost = 0f;
    public float atpUpkeep = 0.1f;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {


        originalPos = transform.position;
        builderManager = GameObject.Find("ShopManager").transform.Find("Shop").GetComponent<BuilderManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isEnabled)
        {
            CheckEnemies();
            UpdateCooldowns();
            HandleBob();
            HandleAtpUpkeep();
        }
    }

    void HandleAtpUpkeep()
    {
        builderManager.UpdateCurrency(0, -atpUpkeep * Time.deltaTime, 0);

        if (builderManager.atp <= 0)
        {
            isEnabled = false;
        }
    }

    void CheckEnemies()
    {
        float closestEnemyDistance = range + 1;
        Collider closestEnemyCollider = null;

        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, range, enemyLayer);

        foreach (Collider enemyCollider in enemyColliders)
        {
           float distance = (enemyCollider.GetComponent<Transform>().position - transform.position).magnitude;

           if (distance < closestEnemyDistance)
           {
                closestEnemyDistance = distance;
                closestEnemyCollider = enemyCollider;
           }

        }

        if (closestEnemyCollider != null)
        {
            AttackEnemy(closestEnemyCollider);
        }

    }

    void AttackEnemy(Collider enemyCollider)
    {
        if (enemyCollider == null)
        {
            return;
        }

        // Rotate the tower towards the enemy
        RotateTowardsTarget(enemyCollider);

        // Reference Enemy script which contains health information
        Enemy enemy = enemyCollider.GetComponent<Enemy>();

        if (currentCooldown <= 0)
        {
            // Play attack sound
            if(audioSource != null)
            {
                PitchAndPlaySound();
            }

            // Create the bullet prefab
            CreateBullet(enemyCollider.GetComponent<Transform>());

            // Apply damage to the enemy
            enemy.TakeDamage(damage);

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

    void HandleBob()
    {
        Vector3 _pos = transform.position;
        _pos.y = originalPos.y + bobAmplitude * Mathf.Sin(Time.time / slowFactor);
        transform.position = _pos;
    }

    void RotateTowardsTarget(Collider enemyCollider)
    {

        // Get the direction to the target
        Vector3 direction = enemyCollider.GetComponent<Transform>().position - transform.position;

        // If the direction is very small, don't update rotation
        if (direction.sqrMagnitude < 0.001f)
            return;

        // Calculate the target rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Use Quaternion.Euler to add the offset to the rotation
        Quaternion correctedRotation = lookRotation * Quaternion.Euler(0, 90, 180);

        // Smoothly interpolate the rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, correctedRotation, Time.deltaTime * rotationSpeed);
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
        bulletScript.damage = damage;

        bulletScript.SetTarget(target);

    }

    void PitchAndPlaySound()
    {
       audioSource.pitch = Random.Range(minPitch, maxPitch);
       audioSource.Play();
    }

    void OnMouseDown()
    {
        hitBoxEnabled = !hitBoxEnabled;
        upgradeUIEnabled = !upgradeUIEnabled;
        transform.Find("rangeSphere").gameObject.SetActive(hitBoxEnabled);
        transform.Find("UpgradeUI").gameObject.SetActive(upgradeUIEnabled);
    }

    public void SellTower()
    {
        Destroy(gameObject);
        builderManager.UpdateCurrency(aminoAcidCost * 0.5f, atpCost, dnaCost);
    }
}
