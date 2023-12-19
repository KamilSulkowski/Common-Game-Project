using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitSpawnSystem : MonoBehaviour
{
    // Array of prefabs for different unit types.
    public GameObject[] unitPrefabs;

    // Reference to the grid in the game environment.
    public Grid grid;

    // UI text element to display the selected unit's name.
    public TextMeshProUGUI selectedUnitText;

    // Panel representing the unit spawn area.
    public GameObject panel;

    // Currently selected unit for spawning.
    private GameObject selectedUnit;

    // Array of tilemaps in the environment.
    private Tilemap[] tileMaps;

    // List of tilemap names where units cannot be spawned.
    private List<string> notAllowTileMaps = new List<string> { "Water", "Object" };

    void Start()
    {
        // Initialize tilemaps from the grid.
        SetTileMaps(grid);
    }

    void Update()
    {
        // Handle mouse click events.
        OnMouseClick();
    }

    // Public method to select a unit type for spawning.
    public void SetUnitToSpawn(GameObject unit)
    {
        selectedUnitText.gameObject.SetActive(true);
        selectedUnitText.text = unit.name;
        selectedUnit = unit;
        Debug.Log("Unit " + unit.name + " selected");
    }

    // Handles mouse click events for spawning units.
    void OnMouseClick()
    {
        if (Input.GetMouseButtonDown(0) && selectedUnit != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 panelPos = Camera.main.ScreenToWorldPoint(panel.transform.position);

            // Avoid spawning when clicking on the unit spawn panel.
            if (!IsClickOnPanel(mousePos, panelPos))
            {
                // Spawn unit if the location is valid.
                if (CheckPlaceToSpawn(mousePos))
                {
                    SpawnUnit(mousePos, selectedUnit);
                }
            }
        }

        // Deselect unit on right-click.
        if (Input.GetMouseButton(1))
        {
            DeselectUnit();
        }
    }

    // Checks whether the click was on the unit spawn panel.
    bool IsClickOnPanel(Vector2 mousePos, Vector2 panelPos)
    {
        bool isWithinPanelX = mousePos.x > panelPos.x && mousePos.x < panelPos.x + 6;
        bool isWithinPanelY = mousePos.y > panelPos.y && mousePos.y < panelPos.y + 3;

        if (isWithinPanelX && isWithinPanelY)
        {
            Debug.Log("Clicked on unit spawn panel");
            return true;
        }
        return false;
    }

    // Checks if the mouse position is valid for spawning a unit.
    bool CheckPlaceToSpawn(Vector2 mousePosition)
    {
        foreach (Tilemap tilemap in tileMaps)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);
            if (tilemap.HasTile(cellPosition) && notAllowTileMaps.Contains(tilemap.name))
            {
                Debug.Log("Can't spawn unit on " + tilemap.name);
                return false;
            }
        }
        return true;
    }

    // Initializes tilemaps from the provided grid object.
    void SetTileMaps(Grid gridObject)
    {
        int tilemapCount = gridObject.transform.childCount;
        tileMaps = new Tilemap[tilemapCount];
        for (int i = 0; i < tilemapCount; i++)
        {
            tileMaps[i] = gridObject.transform.GetChild(i).GetComponent<Tilemap>();
        }
    }

    // Spawns a unit at the specified position.
    void SpawnUnit(Vector2 position, GameObject unitToSpawn)
    {
        Instantiate(unitToSpawn, position, Quaternion.identity);
        Debug.Log("Unit " + unitToSpawn.name + " spawned");
    }

    // Deselects the currently selected unit.
    void DeselectUnit()
    {
        selectedUnit = null;
        selectedUnitText.gameObject.SetActive(false);
    }
}

