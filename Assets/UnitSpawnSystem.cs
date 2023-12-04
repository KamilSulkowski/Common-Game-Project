using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UnitSpawnSystem : MonoBehaviour
{

    public GameObject unitPrefab;
    public GameObject[] unitPrefabs;
    public Grid grid;

    private bool isSpawnMode = false;
    private Tilemap[] tileMaps;

    // Tilemaps that not allow to spawn unit
    private List<string> notAllowTileMaps = new List<string> { "Water", "Object" };

    void Start()
    {
        SetTileMaps(grid);
    }

    void Update()
    {
        // Spawn unit on left mouse click
        if (isSpawnMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (CheckPlaceToSpawn(mousePos))
                {
                    SpawnUnit(mousePos, unitPrefab);
                }
            }
        }

        // Press S to toggle spawn mode
        if (Input.GetKeyDown(KeyCode.S))
        {
            isSpawnMode = !isSpawnMode;
        }
    }

    // Draw info about spawn mode on left top corner
    void OnGUI()
    {
        if (isSpawnMode)
        {
            GUI.backgroundColor = Color.gray;
            GUI.Box(new Rect(10, 10, 150, 30), "Spawn unit mode");
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
    }
}
