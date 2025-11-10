using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// EndlessField creates an infinite terrain system by spawning and recycling terrain tiles
/// as the player moves. This is a common technique in open-world games.
///
/// KEY CONCEPTS DEMONSTRATED:
/// - Procedural generation
/// - Object pooling (reusing GameObjects instead of constantly creating/destroying)
/// - Distance calculations
/// - Dictionary data structures
/// - Performance optimization
///
/// EDUCATIONAL NOTE: In games like Zelda, Minecraft, or open-world RPGs, you can't
/// create the entire world at once - it would use too much memory and CPU. Instead,
/// we create only the terrain near the player and recycle it as they move.
/// </summary>
public class EndlessField : MonoBehaviour
{
    // ============================================================================
    // TERRAIN CONFIGURATION
    // ============================================================================
    [Header("Terrain Tiles")]
    [Tooltip("The prefab to use for each terrain tile. This can be a plane, custom mesh, or terrain.")]
    [SerializeField] private GameObject terrainTilePrefab;

    [Tooltip("Size of each terrain tile (in Unity units/meters)")]
    [SerializeField] private float tileSize = 20f;

    [Tooltip("How many tiles in each direction to keep loaded (larger = more visible terrain)")]
    [SerializeField] private int viewDistanceInTiles = 5;

    // ============================================================================
    // PLAYER REFERENCE
    // ============================================================================
    [Header("Player Settings")]
    [Tooltip("The player Transform to center the terrain around")]
    [SerializeField] private Transform player;

    // ============================================================================
    // VISUAL CUSTOMIZATION
    // ============================================================================
    [Header("Appearance")]
    [Tooltip("Material to apply to terrain tiles")]
    [SerializeField] private Material terrainMaterial;

    [Tooltip("Add random height variation to make terrain more interesting?")]
    [SerializeField] private bool useHeightVariation = false;

    [Tooltip("Maximum random height offset for terrain tiles")]
    [SerializeField] private float maxHeightVariation = 0.5f;

    // ============================================================================
    // OPTIMIZATION
    // ============================================================================
    [Header("Performance")]
    [Tooltip("How often to check if tiles need updating (in seconds). Lower = more responsive, higher = better performance")]
    [SerializeField] private float updateInterval = 0.5f;

    // ============================================================================
    // INTERNAL STATE
    // ============================================================================
    // Dictionary to track which tiles are currently spawned
    // EDUCATIONAL NOTE: Dictionary is like a phone book - you look up values by a key
    // Here, the key is the tile coordinates, and the value is the GameObject
    private Dictionary<Vector2Int, GameObject> activeTiles = new Dictionary<Vector2Int, GameObject>();

    // Keep track of the player's last tile position to detect when they move
    private Vector2Int currentPlayerTileCoord;

    // Timer for update interval
    private float updateTimer;

    // Pool of inactive tiles we can reuse (better than Destroy/Instantiate)
    private Queue<GameObject> tilePool = new Queue<GameObject>();

    // ============================================================================
    // UNITY LIFECYCLE METHODS
    // ============================================================================

    /// <summary>
    /// Initialization - find player and create initial terrain.
    /// </summary>
    private void Start()
    {
        // Try to find player if not assigned
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("EndlessField: No player assigned and no GameObject with 'Player' tag found!");
                enabled = false; // Disable this script
                return;
            }
        }

        // Create a default tile prefab if none provided
        if (terrainTilePrefab == null)
        {
            Debug.LogWarning("EndlessField: No terrain tile prefab assigned. Creating default plane.");
            terrainTilePrefab = CreateDefaultTilePrefab();
        }

        // Get the player's starting tile position
        currentPlayerTileCoord = GetTileCoordinateFromPosition(player.position);

        // Generate initial terrain around the player
        UpdateTerrain();
    }

    /// <summary>
    /// Called every frame - checks if we need to update terrain.
    /// </summary>
    private void Update()
    {
        if (player == null) return;

        // Update timer
        updateTimer += Time.deltaTime;

        // Only check for terrain updates at specified intervals (optimization)
        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;

            // Get player's current tile coordinate
            Vector2Int playerTileCoord = GetTileCoordinateFromPosition(player.position);

            // If player moved to a different tile, update the terrain
            if (playerTileCoord != currentPlayerTileCoord)
            {
                currentPlayerTileCoord = playerTileCoord;
                UpdateTerrain();
            }
        }
    }

    // ============================================================================
    // COORDINATE CONVERSION
    // ============================================================================

    /// <summary>
    /// Converts a world position to tile coordinates.
    /// EDUCATIONAL NOTE: We use this to figure out which "tile" a position is in.
    /// For example, if tileSize = 20, then position (25, 0, 10) is in tile (1, 0).
    /// </summary>
    private Vector2Int GetTileCoordinateFromPosition(Vector3 worldPosition)
    {
        // Divide world position by tile size and round down
        // EDUCATIONAL NOTE: Mathf.FloorToInt rounds down to nearest integer
        // This ensures negative coordinates work correctly
        int x = Mathf.FloorToInt(worldPosition.x / tileSize);
        int z = Mathf.FloorToInt(worldPosition.z / tileSize);

        return new Vector2Int(x, z);
    }

    /// <summary>
    /// Converts tile coordinates to world position (center of tile).
    /// </summary>
    private Vector3 GetWorldPositionFromTileCoord(Vector2Int tileCoord)
    {
        // Calculate center position of the tile
        float x = tileCoord.x * tileSize + (tileSize / 2f);
        float z = tileCoord.y * tileSize + (tileSize / 2f);

        // Add random height variation if enabled
        float y = 0f;
        if (useHeightVariation)
        {
            // Use consistent random based on tile coordinates
            // EDUCATIONAL NOTE: We use coordinates as a seed so each tile
            // always has the same height (consistent world generation)
            Random.InitState(tileCoord.x * 10000 + tileCoord.y);
            y = Random.Range(-maxHeightVariation, maxHeightVariation);
        }

        return new Vector3(x, y, z);
    }

    // ============================================================================
    // TERRAIN GENERATION & MANAGEMENT
    // ============================================================================

    /// <summary>
    /// Main method that updates which terrain tiles should be active.
    /// </summary>
    private void UpdateTerrain()
    {
        // Calculate which tiles should be visible based on player position
        HashSet<Vector2Int> tilesNeeded = new HashSet<Vector2Int>();

        // Loop through all tiles in view distance
        for (int x = -viewDistanceInTiles; x <= viewDistanceInTiles; x++)
        {
            for (int z = -viewDistanceInTiles; z <= viewDistanceInTiles; z++)
            {
                Vector2Int tileCoord = new Vector2Int(
                    currentPlayerTileCoord.x + x,
                    currentPlayerTileCoord.y + z
                );

                tilesNeeded.Add(tileCoord);
            }
        }

        // Remove tiles that are too far away
        List<Vector2Int> tilesToRemove = new List<Vector2Int>();
        foreach (var kvp in activeTiles)
        {
            if (!tilesNeeded.Contains(kvp.Key))
            {
                tilesToRemove.Add(kvp.Key);
            }
        }

        // Recycle removed tiles into the pool
        foreach (var tileCoord in tilesToRemove)
        {
            GameObject tile = activeTiles[tileCoord];
            activeTiles.Remove(tileCoord);
            RecycleTile(tile);
        }

        // Spawn new tiles that are needed but don't exist
        foreach (var tileCoord in tilesNeeded)
        {
            if (!activeTiles.ContainsKey(tileCoord))
            {
                GameObject newTile = GetTileFromPool();
                Vector3 position = GetWorldPositionFromTileCoord(tileCoord);
                newTile.transform.position = position;
                activeTiles[tileCoord] = newTile;
            }
        }
    }

    // ============================================================================
    // OBJECT POOLING
    // ============================================================================
    // EDUCATIONAL NOTE: Object Pooling is an optimization technique where we reuse
    // GameObjects instead of constantly creating and destroying them. This is much
    // faster and reduces garbage collection (which causes lag spikes).

    /// <summary>
    /// Gets a tile from the pool, or creates a new one if the pool is empty.
    /// </summary>
    private GameObject GetTileFromPool()
    {
        GameObject tile;

        if (tilePool.Count > 0)
        {
            // Reuse an existing tile from the pool
            tile = tilePool.Dequeue();
            tile.SetActive(true);
        }
        else
        {
            // No tiles in pool, create a new one
            tile = Instantiate(terrainTilePrefab, transform);
            tile.transform.localScale = Vector3.one * (tileSize / 10f); // Scale to match tile size

            // Apply material if provided
            if (terrainMaterial != null)
            {
                Renderer renderer = tile.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = terrainMaterial;
                }
            }
        }

        return tile;
    }

    /// <summary>
    /// Returns a tile to the pool for later reuse.
    /// </summary>
    private void RecycleTile(GameObject tile)
    {
        tile.SetActive(false);
        tilePool.Enqueue(tile);
    }

    // ============================================================================
    // DEFAULT TILE CREATION
    // ============================================================================

    /// <summary>
    /// Creates a simple plane prefab if no tile prefab was provided.
    /// </summary>
    private GameObject CreateDefaultTilePrefab()
    {
        // Create a plane GameObject
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.name = "DefaultTerrainTile";

        // Set a nice grass-green color
        Renderer renderer = plane.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(0.3f, 0.6f, 0.3f); // Green
        }

        // Disable the GameObject (we'll use it as a prefab)
        plane.SetActive(false);

        return plane;
    }

    // ============================================================================
    // PUBLIC INTERFACE
    // ============================================================================

    /// <summary>
    /// Gets the number of currently active terrain tiles.
    /// Useful for debugging and performance monitoring.
    /// </summary>
    public int GetActiveTerrainCount()
    {
        return activeTiles.Count;
    }

    /// <summary>
    /// Forces a complete terrain refresh.
    /// </summary>
    public void RefreshTerrain()
    {
        UpdateTerrain();
    }

    /// <summary>
    /// Clears all terrain and resets the system.
    /// </summary>
    public void ClearAllTerrain()
    {
        foreach (var tile in activeTiles.Values)
        {
            Destroy(tile);
        }
        activeTiles.Clear();

        while (tilePool.Count > 0)
        {
            Destroy(tilePool.Dequeue());
        }
    }

    // ============================================================================
    // DEBUG VISUALIZATION
    // ============================================================================

    /// <summary>
    /// Draws debug information in the Scene view.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (player == null) return;

        // Draw the player's current tile in yellow
        Vector2Int playerTile = GetTileCoordinateFromPosition(player.position);
        Vector3 tileCenter = GetWorldPositionFromTileCoord(playerTile);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(tileCenter, new Vector3(tileSize, 0.1f, tileSize));

        // Draw view distance boundary in cyan
        Gizmos.color = Color.cyan;
        float viewRadius = viewDistanceInTiles * tileSize;
        DrawGizmoCircle(tileCenter, viewRadius, 32);
    }

    /// <summary>
    /// Helper method to draw a circle in the Gizmos view.
    /// </summary>
    private void DrawGizmoCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );

            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}

// ================================================================================
// EXPERIMENTATION IDEAS FOR LEARNING
// ================================================================================
// Try modifying these values and see what happens:
//
// 1. TILE SIZE: Set tileSize to 50.0 - creates bigger terrain chunks
// 2. VIEW DISTANCE: Set viewDistanceInTiles to 10 - loads more terrain (but uses more memory)
// 3. HEIGHT VARIATION: Enable useHeightVariation - creates a bumpy landscape
// 4. UPDATE INTERVAL: Set updateInterval to 2.0 - tiles update less frequently (more obvious pop-in)
//
// ADVANCED EXPERIMENTS:
// - Add different biomes (grass, desert, snow) based on tile coordinates
// - Implement noise-based height generation for realistic hills and valleys
// - Add props/objects (trees, rocks) to each tile
// - Create a minimap that shows loaded tiles
// - Add tile transitions/blending for smooth visual connections
// - Implement chunk-based LOD (Level of Detail) for distant tiles
// ================================================================================