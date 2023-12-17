using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovementSystem : MonoBehaviour
{
    public float movementSpeed = 2f;

    private List<GameObject> units;
    private NavMeshAgent navMeshAgent;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        units = UnitSelectionSystem.Instance.getSelectedUnits();
        if (Input.GetMouseButtonDown(1))
        {
            if (units != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                foreach (GameObject unit in units)
                {
                    Vector2 target = CalculateOffsetDestination(mousePosition, units.IndexOf(unit), units.Count);
                    MoveTowards(unit, target);
                }
            }
        }
    }

    private void MoveTowards(GameObject unit, Vector2 targetPosition)
    {
        if (targetPosition == null)
        {
            Debug.LogError("MoveTowards called with a null targetPosition.");
            return;
        }

        if (unit == null)
        {
            Debug.LogError("MoveTowards called with a null unit GameObject.");
            return;
        }

        Debug.Log(unit.name + " is moving towards " + targetPosition);

        UnitMovement unitMovement = unit.GetComponent<UnitMovement>();
        if (unitMovement != null)
        {
            Debug.Log("Found UnitMovement component on " + unit.name);
            unitMovement.Move(targetPosition);
        }
        else
        {
            Debug.LogError("UnitMovement component not found on " + unit.name);
        }
    }

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
