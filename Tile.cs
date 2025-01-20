using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isOccupied = false;
    private Renderer rend;
    private Color originalColor;
    private GameObject transparentTower;
    public BuilderManager builderManager;
    public CurrencyController currencyController;

    void Start()
    {
        // Declare the tile's renderer
        rend = GetComponent<Renderer>();

        // Save the tile's original color to revert hover highlighting
        originalColor = rend.material.color;

        // Declare the Building Manager
        builderManager = GameObject.Find("BuildManager").GetComponent<BuilderManager>();

        // Declare the Currency Controller
        currencyController = GameObject.Find("ShopManager").transform.Find("Shop").GetComponent<CurrencyController>();
    }


    void OnMouseDown()
    {
        // Get the cost of the tower selected to build
        float cost  = builderManager.selectedTower.GetComponent<DetectEnemy>().cost;

        // Get the play's current currency
        float currentCurrency = currencyController.currency;

        // Exit early if the player cannot afford the tower
        if (cost > currentCurrency)
        {
            Debug.Log("Not enough currency to purchase the tower");
            return;
        }


        if (!isOccupied)
        {
            // Place the tower
            PlaceTower();

            // Subtract the tower's cost from the player's currency
            currencyController.UpdateCurrency(-cost);

        }
    }

    void PlaceTower()
    {
        // Create and the bought tower
        Instantiate(builderManager.selectedTower, transform.position + Vector3.up * 2f, Quaternion.identity);

        // Destroy the tower created from hovering
        Destroy(transparentTower);

        // Update the occupation state of the tile
        isOccupied = true;
    }

    void OnMouseEnter()
    {
        if (!isOccupied && builderManager.selectedTower != null)
        {
            // Create a tower when hovering over an empty tile
            transparentTower = (GameObject) Instantiate(builderManager.selectedTower, transform.position + Vector3.up * 2f, Quaternion.identity);

            // Disable the tower so it does not attack enemies before it is bought
            transparentTower.GetComponent<DetectEnemy>().isEnabled = false;
        }

        // Turn tile green if it can be built on, otherwise turn it red
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
