using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitSpawnSystem : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public Grid grid;
    public TextMeshProUGUI selectedUnitText;
    public GameObject panel;

    private GameObject selectedUnit;
    private Tilemap[] tileMaps;

    // Tilemaps that not allow to spawn unit
    private List<string> notAllowTileMaps = new List<string> { "Water", "Object" };

    void Start()
    {
        // Set tilemaps from grid children to array
        SetTileMaps(grid);
    }

    void Update()
    {
        // Hold left and right mouse buttons click
        OnMouseClick();
    }

    // public method to select unit to spawn
    public void SetUnitToSpawn(GameObject unit)
    {
        selectedUnitText.gameObject.SetActive(true);
        selectedUnitText.text = unit.name;
        selectedUnit = unit;
        Debug.Log("Unit " + unit.name + " selected");
    }

    // method to handle mouse click
    void OnMouseClick()
    {
        if (Input.GetMouseButtonDown(0) && selectedUnit != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 panelPos = Camera.main.ScreenToWorldPoint(panel.transform.position);

            // Check if clicked on unit spawn panel
            if ((mousePos.x > panelPos.x && mousePos.x < panelPos.x + 6) && (mousePos.y > panelPos.y && mousePos.y < panelPos.y + 3))
            {
                Debug.Log("Clicked on unit spawn panel");
                return;
            }

            // Check if clicked on tilemap
            if (CheckPlaceToSpawn(mousePos))
            {
                SpawnUnit(mousePos, selectedUnit);
            }
        }

        if (Input.GetMouseButton(1))
        {
            selectedUnit = null;
            selectedUnitText.gameObject.SetActive(false);
        }
    }

    // Check if mouse position is valid to spawn unit
    bool CheckPlaceToSpawn(Vector2 mousePosition)
    {
        foreach (Tilemap tilemap in tileMaps)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);
            if (tilemap.HasTile(cellPosition))
            {
                if (notAllowTileMaps.Contains(tilemap.name))
                {
                    Debug.Log("Can't spawn unit on there " + tilemap.name);
                    return false;
                }
            }
        }
        return true;
    }

    // Set tilemaps from grid children to array
    void SetTileMaps(Grid gridObject)
    {
        int tilemapCount = gridObject.transform.childCount;
        tileMaps = new Tilemap[tilemapCount];
        for (int i = 0; i < tilemapCount; i++)
        {
            tileMaps[i] = gridObject.transform.GetChild(i).GetComponent<Tilemap>();
        }
    }

    // Spawn unit at position
    void SpawnUnit(Vector2 position, GameObject unitToSpawn)
    {
        Instantiate(unitToSpawn, position, Quaternion.identity);
        Debug.Log("Unit " + unitToSpawn.name + " spawned");
    }
}
