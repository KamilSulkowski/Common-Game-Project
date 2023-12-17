using UnityEngine;
using UnityEngine.UI;

public class BuildingPanelController : MonoBehaviour
{
    public GameObject buildingPanel;

    private void Start()
    {

        buildingPanel.SetActive(false);
    }

    public void ToggleBuildingPanel()
    {

        buildingPanel.SetActive(!buildingPanel.activeSelf);
    }
}
