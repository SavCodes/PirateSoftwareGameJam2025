using UnityEngine;

public class Slot : MonoBehaviour
{
    [Header("Slot Settings")]
    // Offset and Rotation fine tuning
    public Vector3 towerOffset;
    public Quaternion towerRotation;
    public bool isOccupied = false;

    // Declare game object component references
    public Renderer rend;
    public GameObject occupyingStructure;
    public BuilderManager builderManager;

    // Private constructor declarations
    private Color originalColor;

    private SlotManager slotManager;

    void Start()
    {
        slotManager = transform.parent.GetComponent<SlotManager>();
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        builderManager = GameObject.Find("ShopManager").transform.Find("Shop").GetComponent<BuilderManager>();
    }


    void OnMouseEnter()
    {
        rend.material.color = isOccupied ? Color.red : Color.green;
    }

    void OnMouseExit()
    {
        rend.material.color = originalColor;
    }

    void OnMouseDown()
    {

        // Exit early if the cell slot is occupied
        if (isOccupied)
        {
            return;
        }

        Debug.Log(builderManager.selectedTower);
        Debug.Log(builderManager.towerOne);

        // Determine the offset to draw the tower on based off of its specific type
        if (builderManager.selectedTower == builderManager.towerOne)
        {
            // Declare tower offset vectors
            towerOffset = new Vector3 (0, 1, 0);
            towerRotation = Quaternion.identity;

            // Create the new slot
            GameObject newSlot = (GameObject) Instantiate(slotManager.singleSlot, transform.position + Vector3.up * 0.5f, Quaternion.Euler(270, 0, 0));
            newSlot.transform.parent = transform.parent;

            // Create the new tower to fill the slot
            occupyingStructure = (GameObject) Instantiate(builderManager.selectedTower, transform.position + towerOffset, towerRotation);
            occupyingStructure.transform.parent = newSlot.transform;

            // Update the occupation state of the new slot
            newSlot.GetComponent<Slot>().occupyingStructure = occupyingStructure;
            newSlot.GetComponent<Slot>().isOccupied = true;

            // Update the slot manager dictionary
            slotManager.UpdateSlots(newSlot);
            Destroy(gameObject);
            return;
        }


        else if (builderManager.selectedTower == builderManager.towerTwo)
        {
            towerOffset = new Vector3 (0, 0.5f, 0);
            towerRotation = Quaternion.Euler(270, 180, 0);
        }

        else if (builderManager.selectedTower == builderManager.towerThree)
        {
            towerOffset = new Vector3 (0, 0, 0);
            towerRotation = Quaternion.Euler(0, 0, 0);
        }

        else if (builderManager.selectedTower == builderManager.towerFour)
        {
            towerOffset = new Vector3 (0, 0, 0);
            towerRotation = Quaternion.Euler(0, 0, 0);
        }


        // Create the new structure in the cell slot
        occupyingStructure = (GameObject) Instantiate(builderManager.selectedTower, transform.position + towerOffset, towerRotation);
        occupyingStructure.transform.parent = transform;

        // Toggle the occupation state to true
        isOccupied = true;

        slotManager.UpdateSlots(gameObject);
        }


}
