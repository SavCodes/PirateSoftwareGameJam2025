using UnityEngine;
using System.Collections.Generic;


public class SlotManager : MonoBehaviour
{
    // Load the different tier slot frames
    [Header("Slot tier prefabs")]
    public GameObject nullSlot;
    public GameObject singleSlot;
    public GameObject doubleSlot;
    public GameObject quadSlot;

    // Initialize slot renderer
    private Renderer rend;

    // Initialize game building manager
    private BuilderManager builderManager;

    // Initialize upgrade tower variable
    private GameObject upgradeTower;
    private GameObject upgradeSlot;
    private Vector3 upgradeTowerOffset;
    private Vector3 upgradeSlotOffset;

    // Initialize rotational offsets
    private Quaternion towerRotation;
    private Quaternion slotRotation;

    // Initialize original color variable
    private Color originalColor;

    // Initialize the four cell slots
    private GameObject slotOne;
    private GameObject slotTwo;
    private GameObject slotThree;
    private GameObject slotFour;

    // Initialize the slot list
    private GameObject[] slotList = new GameObject[4];

    Dictionary<string, List<GameObject>> towerOccupancy = new Dictionary<string, List<GameObject>>();


    void Start()
    {
      builderManager = GameObject.Find("ShopManager").transform.Find("Shop").GetComponent<BuilderManager>();
      rend = GetComponent<Renderer>();
      originalColor = rend.material.color;

      // Declare slot positions
      Vector3 slotOnePos = new Vector3 (1f, 1.25f, 1f);
      Vector3 slotTwoPos = new Vector3 (1f, 1.25f, -1f);
      Vector3 slotThreePos = new Vector3 (-1f, 1.25f, 1f);
      Vector3 slotFourPos = new Vector3 (-1f, 1.25f, -1f);

      // Create slot objects
      slotOne = (GameObject) Instantiate(nullSlot, transform.position + slotOnePos, Quaternion.Euler(0, 0, 0));
      slotTwo = (GameObject) Instantiate(nullSlot, transform.position + slotTwoPos, Quaternion.Euler(0, 0, 0));
      slotThree = (GameObject) Instantiate(nullSlot, transform.position + slotThreePos, Quaternion.Euler(0, 0, 0));
      slotFour = (GameObject) Instantiate(nullSlot, transform.position + slotFourPos, Quaternion.Euler(0, 0, 0));

      // Assign slots to parent object
      slotOne.transform.parent = transform;
      slotTwo.transform.parent = transform;
      slotThree.transform.parent = transform;
      slotFour.transform.parent = transform;

      // Assign slots to slot list
      slotList[0] = slotOne;
      slotList[1] = slotTwo;
      slotList[2] = slotThree;
      slotList[3] = slotFour;
    }

    public void UpdateSlots(GameObject slot)
    {
        // Get the Slot component from the provided GameObject
        Slot slotComponent = slot.GetComponent<Slot>();
        if (slotComponent == null )
        {
            Debug.LogWarning("Invalid slot!");
            return;
        }

        if (slotComponent.occupyingStructure == null)
        {
            Debug.LogWarning("Invalid occupying structure!");
            return;
        }

        string structureName = slotComponent.occupyingStructure.name;
        // Update the tower occupancy dictionary
        if (towerOccupancy.ContainsKey(structureName))
        {
            towerOccupancy[structureName].Add(slot);
        }
        else
        {
            towerOccupancy[structureName] = new List<GameObject> { slot };
        }

        // Check the updated occupancy states
        foreach (var pair in towerOccupancy)
        {
            Debug.Log($"Tower Name: {pair.Key}, Tower Count: {pair.Value.Count}");

            if (pair.Value.Count == 2)
            {
                HalfUpgradeTower(pair.Key, pair.Value);
                return;
            }
        }
    }

    // Helper method to highlight slots
    private void HighlightSlots(List<GameObject> slots, Color color)
    {
        foreach (GameObject slot in slots)
        {
            Renderer renderer = slot.GetComponent<Slot>().occupyingStructure.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }

    private void HalfUpgradeTower(string name, List<GameObject> slots)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            Destroy(slots[i]);
            Destroy(slot.occupyingStructure);
        }

        if (name == builderManager.towerOne.name + "(Clone)")
        {
            // New objects
            upgradeTower = builderManager.towerOneLevelTwo;
            upgradeSlot =  doubleSlot;

            // Positional Offsets
            upgradeTowerOffset = new Vector3 (0, 4, 0);
            upgradeSlotOffset = new Vector3 (0, 2, 0);

            // Rotational Offsets
            towerRotation = Quaternion.identity;
            slotRotation = Quaternion.Euler(270, 0, 0);
        }

        else if (name == builderManager.towerOneLevelTwo.name + "(Clone)")
        {
            // New objects
            upgradeTower = builderManager.towerOneLevelThree;
            upgradeSlot = quadSlot;

            // Positional Offsets
            upgradeTowerOffset = new Vector3 (0, 4, 0);
            upgradeSlotOffset = new Vector3 (0, 2, 0);

            // Rotational Offsets
            towerRotation = Quaternion.identity;
            slotRotation = Quaternion.Euler(270, 0, 0);

        }

        else if (name == builderManager.towerTwo.name + "(Clone)")
        {
            // New Objects
            upgradeTower = builderManager.towerTwoLevelTwo;
            upgradeSlot = nullSlot;

            // Offsets
            upgradeTowerOffset = new Vector3 (0, 1, 0);
            towerRotation = Quaternion.Euler(270, 180, 0);

        }

        else if (name == builderManager.towerTwoLevelTwo.name + "(Clone)")
        {
            upgradeTower = builderManager.towerTwoLevelThree;
            upgradeSlot = nullSlot;

            // Offsets
            upgradeTowerOffset = new Vector3 (0, 1, 0);
            towerRotation = Quaternion.Euler(270, 180, 0);
        }

        else if (name == builderManager.towerThree.name + "(Clone)")
        {

            upgradeTower = builderManager.towerThreeLevelTwo;
        }

        else if (name == builderManager.towerFour.name + "(Clone)")
        {
            upgradeTower = builderManager.towerFourLevelTwo;
        }

        else
        {
            Debug.Log($"Name = {name}");
            Debug.Log($"Builder Manager Reference = {builderManager.towerThree.name}");
        }

        // Create the replacement upgrade tower
        GameObject upgradeTowerGO = (GameObject) Instantiate(upgradeTower, transform.root.position + upgradeTowerOffset, towerRotation);
        upgradeTowerGO.transform.parent = transform;

        // Create the replacement upgrade slot
        GameObject upgradeSlotGO = (GameObject) Instantiate(upgradeSlot, transform.root.position + upgradeSlotOffset, slotRotation);
        upgradeSlotGO.transform.parent = transform;
        upgradeTowerGO.transform.parent = upgradeSlotGO.transform;
        upgradeSlotGO.GetComponent<Slot>().occupyingStructure = upgradeTowerGO;
        upgradeSlotGO.GetComponent<Slot>().isOccupied = true;

        // Clear the slots from the dictionary
        string structureName = slots[0].GetComponent<Slot>().occupyingStructure.name;
        towerOccupancy.Remove(structureName);

        UpdateSlots(upgradeSlotGO);
    }
}
