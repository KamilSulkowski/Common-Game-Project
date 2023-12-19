using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSelectionSystem : MonoBehaviour
{
    // Singleton instance for global access.
    public static UnitSelectionSystem Instance { get; private set; }

    // Prefab for the selection indicator.
    public GameObject selectPrefab;

    // List of currently selected units.
    private List<GameObject> selectedUnits = new List<GameObject>();

    // List of selection indicators.
    private List<GameObject> selects = new List<GameObject>();

    private void Awake()
    {
        // Ensure a single instance of the selection system.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Handle unit selection each frame.
        SelectUnits();
    }

    // Main method to handle unit selection.
    void SelectUnits()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if a unit is clicked.
            CheckIsUnit();
        }
    }

    // Adds a visual indicator above a selected unit.
    void AddSelectSign(GameObject selectedUnit)
    {
        GameObject selected = Instantiate(selectPrefab, selectedUnit.transform.position, Quaternion.identity);
        selected.transform.parent = selectedUnit.transform;
        selects.Add(selected);
    }

    // Checks if a unit has already been selected.
    void CheckIsSelected(GameObject selectedUnit)
    {
        if (selectedUnits.Contains(selectedUnit))
        {
            // Clear previous selections if the unit is already selected.
            ClearSelections();
        }

        // Add the new selection.
        selectedUnits.Add(selectedUnit);
        AddSelectSign(selectedUnit);
        Debug.Log("Unit " + selectedUnit.name + " selected");
    }

    // Clears current selections.
    void ClearSelections()
    {
        selectedUnits.Clear();
        foreach (GameObject select in selects)
        {
            Destroy(select);
        }
        selects.Clear();
    }

    // Checks if the clicked object is a unit.
    void CheckIsUnit()
    {
        GameObject selectedUnit = getTargetOnClick()?.gameObject;

        if (selectedUnit != null && selectedUnit.tag == "Unit")
        {
            CheckIsSelected(selectedUnit);
        }
        else
        {
            ClearSelections();
        }
    }

    // Returns the target GameObject clicked on.
    Collider2D getTargetOnClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        return hit.collider;
    }

    // Provides access to the currently selected units.
    public List<GameObject> getSelectedUnits()
    {
        return selectedUnits;
    }
}

