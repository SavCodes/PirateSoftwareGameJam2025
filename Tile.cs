using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isOccupied = false;
    private Renderer rend;
    private Color originalColor;
    private GameObject transparentTower;
    public BuilderManager builderManager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        builderManager = GameObject.Find("BuildManager").GetComponent<BuilderManager>();
    }


    void OnMouseDown()
    {
        if (!isOccupied)
        {
            PlaceTower();
        }
    }

    void PlaceTower()
    {
        // Replace this with your tower prefab
        Instantiate(builderManager.selectedTower, transform.position + Vector3.up * 2f, Quaternion.identity);
        Destroy(transparentTower);
        isOccupied = true;
    }

    void OnMouseEnter()
    {
        if (!isOccupied && builderManager.selectedTower != null)
        {
            transparentTower = (GameObject) Instantiate(builderManager.selectedTower, transform.position + Vector3.up * 2f, Quaternion.identity);
            transparentTower.GetComponent<DetectEnemy>().isEnabled = false;
        }

        rend.material.color = isOccupied ? Color.red : Color.green;
    }

    void OnMouseExit()
    {
        if (!isOccupied)
        {
            Destroy(transparentTower);
        }

        rend.material.color = originalColor;
    }
}
