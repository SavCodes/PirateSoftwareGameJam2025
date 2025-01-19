using UnityEngine;

public class TowerSelectionOne : MonoBehaviour
{
    public GameObject towerSelection;

    public void OnClick()
    {
       GameObject.Find("BuildManager").GetComponent<BuilderManager>().selectedTower = towerSelection;
       Debug.Log("A new tower has been selected");
    }
}
