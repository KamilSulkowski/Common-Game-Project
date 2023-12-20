using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovementSystem : MonoBehaviour
{
    // Movement speed of the units.
    public float movementSpeed = 2f;

    // List of currently controlled units.
    private List<GameObject> units;

    void Update()
    {
        // Retrieve selected units.
        units = UnitSelectionSystem.Instance.getSelectedUnits();
        if (Input.GetMouseButtonDown(1) && units != null)
        {
            // Move units to the clicked position.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (GameObject unit in units)
            {
                Vector2 target = CalculateOffsetDestination(mousePosition, units.IndexOf(unit), units.Count);
                MoveTowards(unit, target);
            }
        }
    }

    // Moves a unit towards a target position.
    private void MoveTowards(GameObject unit, Vector2 targetPosition)
    {
        if (unit == null)
        {
            Debug.LogError("MoveTowards called with a null unit GameObject.");
            return;
        }

        UnitMovement unitMovement = unit.GetComponent<UnitMovement>();
        if (unitMovement != null)
        {
            unitMovement.Move(targetPosition);
        }
        else
        {
            Debug.LogError("UnitMovement component not found on " + unit.name);
        }
    }

    // Calculates a position offset to avoid units overlapping.
    Vector2 CalculateOffsetDestination(Vector3 destination, int index, int totalUnits)
    {
        int unitsPerRow = Mathf.CeilToInt(Mathf.Sqrt(totalUnits));
        float spacing = 1f;

        int row = index / unitsPerRow;
        int column = index % unitsPerRow;

        float offsetX = (column - unitsPerRow / 2) * spacing;
        float offsetY = (row - unitsPerRow / 2) * spacing;

        return new Vector2(destination.x + offsetX, destination.y + offsetY);
    }
}

