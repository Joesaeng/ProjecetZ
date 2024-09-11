using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JPSTileSetter : MonoBehaviour
{
    private static JPSTileSetter _instance;
    public static JPSTileSetter Instance => _instance;

    public Tilemap groundTilemap;
    public Tilemap wallTilemap;

    public Vector2Int gridSize;

    private HashSet<Vector2Int> wallTilesPos = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> groundTilesPos = new HashSet<Vector2Int>();

    public JPSNode[,] grid;

    public void SetCurrentTileSet(Tilemap ground, Tilemap wall)
    {
        groundTilemap = ground;
        wallTilemap = wall;

        BoundsInt bounds = groundTilemap.cellBounds;

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        Vector3Int tileOffset = Vector3Int.up;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (groundTilemap.GetTile(pos) != null)
            {
                minX = Mathf.Min(minX, pos.x);
                minY = Mathf.Min(minY, pos.y);
                maxX = Mathf.Max(maxX, pos.x);
                maxY = Mathf.Max(maxY, pos.y);
            }
            if (wallTilemap.GetTile(pos) != null)
            {
                wallTilesPos.Add(new Vector2Int(pos.x,pos.y));
            }
        }

        gridSize = new Vector2Int(maxX + minX + 1, -(minY - maxY) + 1);
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = GetComponent<JPSTileSetter>();
        }
    }

    private void Start()
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        Vector3Int tileOffset = Vector3Int.up;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (groundTilemap.GetTile(pos) != null)
            {
                minX = Mathf.Min(minX, pos.x);
                minY = Mathf.Min(minY, pos.y);
                maxX = Mathf.Max(maxX, pos.x);
                maxY = Mathf.Max(maxY, pos.y);

                if (wallTilemap.GetTile(pos) != null)
                {
                    wallTilesPos.Add(new Vector2Int(pos.x, pos.y));
                }
                else
                {
                    groundTilesPos.Add(new Vector2Int(pos.x, pos.y));
                }

            }
            
        }

        gridSize = new Vector2Int(maxX + minX + 1, -(minY - maxY) + 1);
    }

    private void CreateGrid()
    {
        grid = new JPSNode[gridSize.x,gridSize.y];

        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                //Vector2 worldPos = 
            }
        }    
    }

    public bool IsWallTile(Vector2Int pos)
    {
        return wallTilesPos.Contains(pos);
    }
}
