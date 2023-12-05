using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSelectionSystem : MonoBehaviour
{
    public GameObject selectPrefab;

    private List<GameObject> selectedUnits = new List<GameObject>();
    private List<GameObject> selects = new List<GameObject>();

    private void Update()
    {
        SelectUnits();
    }

    void SelectUnits()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckIsUnit();
        }
    }

    // Add select sign above selected unit
    void AddSelectSign(GameObject selectedUnit)
    {
        GameObject selected = Instantiate(selectPrefab, selectedUnit.transform.position, Quaternion.identity);
        selected.transform.parent = selectedUnit.transform;

        selects.Add(selected);
    }

    // Check if unit is selected earlier
    void CheckIsSelected(GameObject selectedUnit)
    {
        if (selectedUnits.Contains(selectedUnit))
        {
            selectedUnits.Clear();
            foreach (GameObject select in selects)
            {
                Destroy(select);
            }
        }

        selectedUnits.Add(selectedUnit);
        AddSelectSign(selectedUnit);

        Debug.Log("Unit " + selectedUnit.name + " selected");
    }

    // Check if clicked on unit
    void CheckIsUnit()
    {
        GameObject selectedUnit;

        if (getTargetOnClick() != null)
        {
            selectedUnit = getTargetOnClick().gameObject;
        } else
        {
            selectedUnit = null;
        }

        if (selectedUnit != null)
        {
            if (selectedUnit.tag == "Unit")
            {
                CheckIsSelected(selectedUnit);
            }
        }
        else
        {
            selectedUnits.Clear();
            foreach (GameObject select in selects)
            {
                Destroy(select);
            }
        }
    }

    // Get target on mouse click
    Collider2D getTargetOnClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            return hit.collider;
        } else
        {
            return null;
        }
    }

    // Get selected units
    public List<GameObject> getSelectedUnits()
    {
        return selectedUnits;
    }
}
