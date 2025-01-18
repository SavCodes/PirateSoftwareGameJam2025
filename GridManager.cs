using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Grid Setting Controller
    [Header("Grid Settings")]
    public float width = 10f;
    public float height = 10f;
    public float tileSize = 1f;
    public GameObject tilePrefab;


    void Start()
    {
        // Create the game grid when the scene is loaded
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Define the tile position
                Vector3 position = new Vector3 (x * tileSize, 0, z * tileSize);

                // Load in the tile prefab
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);

                // Set the tile parent transform
                tile.transform.parent = this.transform;

                // Set the tile name
                tile.name = $"Tile ({x}, {z})";
            }
        }

    }
}
