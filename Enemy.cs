using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float healthBarSize = 30f;
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public float shield = 0f;
    public RectTransform healthBar; // Direct reference to the health bar RectTransform

    [Header("Pathing Settings")]
    public int waypointIndex = 1;          // Current waypoint index
    public float moveSpeed = 1f;           // Speed of movement (units per second)
    public GameObject waypointList;        // Parent object containing waypoints
    public Vector3 pathOffset = new Vector3 (0, 2, 0);


    private Transform[] waypointTransforms; // Array of waypoints

    void Start()
    {
        // Get all waypoint transforms and initialize starting position
        waypointList = GameObject.Find("Waypoints");
        waypointTransforms = waypointList.GetComponent<Transform>().GetComponentsInChildren<Transform>();
        transform.position = waypointTransforms[1].position; // Start at the first waypoint
        gameObject.layer = 3;
    }

    void Update()
    {
        UpdateHealthBar();

        // Move the enemy if it still has HP
        if ( currentHealth > 0 )
            HandleTraveling();

        // Handle death action if there is no more HP
        else
            HandleDeath();

    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Calculate the normalized health value (0 to 1)
            float healthNormalized = Mathf.Clamp01(currentHealth / maxHealth);

            // Update the width of the health bar using its sizeDelta property
            healthBar.sizeDelta = new Vector2(healthNormalized * healthBarSize, healthBar.sizeDelta.y);

            // Debug.Log("Health Bar Width Updated: " + healthBar.sizeDelta.x);
        }
        else
        {
            Debug.LogWarning("Health bar is not assigned!");
        }
    }

    void HandleDeath()
    {
        Destroy(gameObject);
    }

    void HandleTraveling()
    {
        // Check if there are more waypoints to move to
        if (waypointIndex < waypointTransforms.Length - 1)
        {
            // Move toward the next waypoint at a constant speed
            transform.position = Vector3.MoveTowards(
                transform.position,
                waypointTransforms[waypointIndex + 1].position + pathOffset,
                moveSpeed * Time.deltaTime
            );

            // Check if the object has reached the next waypoint
            if (Vector3.Distance(transform.position, waypointTransforms[waypointIndex + 1].position + pathOffset) < 0.1f)
            {
                waypointIndex++; // Move to the next waypoint
            }
        }

        if (waypointIndex == waypointTransforms.Length - 1)
        {
            // Find the EndNode in the Game Hierarchy
            GameObject EndNode = GameObject.Find("EndNode");

            // Destroy the Base for now => Add damage mechanic later
            Destroy(EndNode);

            // Self destruct when reaching the end
            Destroy(gameObject);
        }
    }

}
