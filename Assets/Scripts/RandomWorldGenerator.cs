using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomWorldGenerator : MonoBehaviour
{
    // Tilemaps for different layers
    public Tilemap groundTilemap;
    public Tilemap waterTilemap;
    public Tilemap objectTilemap;

    // Tiles for different terrain and objects
    public Tile groundTile;
    public RuleTile waterTile;
    public Tile treeTile;
    public Tile oreTile;

    // World generation parameters
    public int mapWidth = 100;
    public int mapHeight = 100;
    public int seed = 0; // Seed for random generation
    public float scale = 0.1f; // Scale for noise function

    // Chances for generating trees and ores
    public float treeChance = 0.3f;
    public float oreChance = 0.1f;

    // Noise offset for terrain generation
    private float offsetX;
    private float offsetY;

    void Start()
    {
        // Generate a random seed if not provided
        if (seed == 0)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        // Initialize random state with seed
        Random.InitState(seed); 

        // Generate random offsets for Perlin noise
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);

        GenerateWorld(); // Start world generation
    }

    void GenerateWorld()
    {
        GenerateTerrain(); // Generate terrain layout
        GenerateObjects(); // Populate world with objects
    }

    void GenerateTerrain()
    {
        // Loop through map dimensions
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                // Calculate Perlin noise value with offset
                float noise = Mathf.PerlinNoise((i + offsetX) * scale, (j + offsetY) * scale);
                ChooseTile(i, j, noise); // Choose and set tile based on noise
            }
        }
    }

    void GenerateObjects()
    {
        // Future implementation for object generation
    }

    void ChooseTile(int x, int y, float noise)
    {
        // Set basic ground tile
        SetTile(groundTilemap, x, y, groundTile);

        // Determine tile type based on noise
        if (noise < 0.3f)
        {
            // If noise is low, set water tile 2x2
            if (x % 2 == 0 && y % 2 == 0) 
            { 
                SetRuleTile(waterTilemap, x, y, waterTile);
                SetRuleTile(waterTilemap, x + 1, y, waterTile);
                SetRuleTile(waterTilemap, x + 1, y + 1, waterTile);
                SetRuleTile(waterTilemap, x, y + 1, waterTile);
            }
        }
        else if (noise > 0.4f) // offset from water
        {
            // Add randomness to object placement
            float objectChance = Random.Range(0f, 1f);

            if (noise < 0.7f && objectChance < treeChance)
            {
                // Place tree based on chance
                SetTile(objectTilemap, x, y, treeTile);
            }
            else if (noise < 1.0f && objectChance < oreChance)
            {
                // Place ore based on chance
                SetTile(objectTilemap, x, y, oreTile);
            }
        }
    }

    void SetTile(Tilemap tilemap, int x, int y, Tile tile)
    {
        // Helper method to set a tile on a tilemap
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    void SetRuleTile(Tilemap tilemap, int x, int y, RuleTile tile)
    {
        // Helper method to set a tile on a tilemap
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public void SetSeedAndRecreate(int newSeed)
    {
        // Public method to set a new seed and recreate the world
        seed = newSeed;
        Random.InitState(seed);
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
        GenerateWorld();
    }
}