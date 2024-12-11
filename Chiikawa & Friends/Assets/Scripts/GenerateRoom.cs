using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    public GameObject blackSpacePrefab;
    public GameObject floorPrefab;
    public GameObject topWallPrefab;
    public GameObject bottomWallPrefab;
    public GameObject leftWallPrefab;
    public GameObject rightWallPrefab;
    public GameObject topLeftCornerPrefab;
    public GameObject topRightCornerPrefab;
    public GameObject bottomLeftCornerPrefab;
    public GameObject bottomRightCornerPrefab;
    public int roomWidth = 20;
    public int roomHeight = 20;
    public float noiseScale = 10f;
    public float threshold = 0.5f;
    private Vector2 noiseOffset;
    private bool[,] roomShape;

    private void Start()
    {
        noiseOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
        Vector2Int startPosition = new Vector2Int(0, 0);
        GenerateRoom(startPosition);
    }

    public void GenerateRoom(Vector2Int position)
    {
        roomShape = new bool[roomWidth, roomHeight];
        // Generate room shape using Perlin noise with the random offset
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                // Check if the current tile is not on the edge of the room
                if (x > 0 && x < roomWidth - 1 && y > 0 && y < roomHeight - 1)
                {
                    float noiseValue = Mathf.PerlinNoise(
                        (x + position.x) / noiseScale + noiseOffset.x,
                        (y + position.y) / noiseScale + noiseOffset.y
                    );

                    // Set roomShape to true only for non-edge tiles
                    roomShape[x, y] = noiseValue > threshold;
                }
                else
                {
                    roomShape[x, y] = false;
                }
            }
        }

        //Ensure all floor tiles are connected
        EnsureFloorConnectivity(ref roomShape);

        //Place tiles based on room shape
        for (int x = -1; x <= roomWidth; x++)
        {
            for (int y = -1; y <= roomHeight; y++)
            {
                Vector3 tilePosition = new Vector3(position.x + x, position.y + y, 0);

                if (x >= 0 && x < roomWidth && y >= 0 && y < roomHeight)
                {
                    // Floor tiles
                    if (roomShape[x, y])
                    {
                        Instantiate(floorPrefab, tilePosition, Quaternion.identity);
                    }
                }

                // Wall tiles
                if ((x > -1 && x < roomWidth) && (y > -1 && y < roomHeight) && 
                    (!roomShape[x, y] && IsFloorTileAdjacent(x, y)))
                {
                    if (y + 1 < roomHeight && roomShape[x, y + 1]) // Floor tile is below, so this is a top wall
                    {
                        Instantiate(topWallPrefab, tilePosition, Quaternion.identity);
                    }
                    /*
                    else if (x > 0 && y < roomHeight - 1 && roomShape[x - 1, y] && roomShape[x, y + 1]) // Top-right corner
                    {
                        Instantiate(topRightCornerPrefab, tilePosition, Quaternion.identity);
                    }
                    else if (x < roomWidth - 1 && y < roomHeight - 1 && roomShape[x + 1, y] && roomShape[x, y + 1]) // Top-left corner
                    {
                        Instantiate(topLeftCornerPrefab, tilePosition, Quaternion.identity);
                    }
                    else if (x < roomWidth - 1 && y > 0 && roomShape[x + 1, y] && roomShape[x, y - 1]) // Bottom-left corner
                    {
                        Instantiate(topRightCornerPrefab, tilePosition, Quaternion.identity);
                    }
                    else if (x > 0 && y > 0 && roomShape[x - 1, y] && roomShape[x, y - 1]) // Bottom-right corner
                    {
                        Instantiate(topLeftCornerPrefab, tilePosition, Quaternion.identity);
                    }
                    */
                    else if (y - 1 >= 0 && roomShape[x, y - 1]) // Floor tile is above, so this is a bottom wall
                    {
                        Instantiate(bottomWallPrefab, tilePosition, Quaternion.identity);
                    }
                    else if (x + 1 < roomWidth && roomShape[x + 1, y]) // Floor tile is to the left, so this is a right wall
                    {
                        Instantiate(rightWallPrefab, tilePosition, Quaternion.identity);
                    }
                    else if (x - 1 >= 0 && roomShape[x - 1, y]) // Floor tile is to the right, so this is a left wall
                    {
                        Instantiate(leftWallPrefab, tilePosition, Quaternion.identity);
                    }
                }
                if ((x > -1 && x < roomWidth) && (y > -1 && y < roomHeight) && 
                    (!roomShape[x, y] && !(IsFloorTileAdjacent(x, y))))
                {
                    // Inside wall tiles for non-instantiated positions
                    Instantiate(blackSpacePrefab, tilePosition, Quaternion.identity);
                }
            }
        }
    }

    private bool IsFloorTileAdjacent(int x, int y)
    {
        return (x > 0 && roomShape[x - 1, y]) || 
            (x < roomWidth - 1 && roomShape[x + 1, y]) || 
            (y > 0 && roomShape[x, y - 1]) || 
            (y < roomHeight - 1 && roomShape[x, y + 1]);
    }

    private void EnsureFloorConnectivity(ref bool[,] roomShape)
    {
        // Find a starting point (any floor tile)
        Queue<Vector2Int> toVisit = new Queue<Vector2Int>();
        bool[,] visited = new bool[roomWidth, roomHeight];
        bool foundStart = false;
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (roomShape[x, y])
                {
                    toVisit.Enqueue(new Vector2Int(x, y));
                    visited[x, y] = true;
                    foundStart = true;
                    break;
                }
            }
            if (foundStart) break;
        }

        // Perform BFS to visit all connected floor tiles
        List<Vector2Int> connectedFloorTiles = new List<Vector2Int>();
        while (toVisit.Count > 0)
        {
            Vector2Int current = toVisit.Dequeue();
            connectedFloorTiles.Add(current);
            Vector2Int[] neighbors = new Vector2Int[] {
                new Vector2Int(current.x - 1, current.y), // Left
                new Vector2Int(current.x + 1, current.y), // Right
                new Vector2Int(current.x, current.y - 1), // Down
                new Vector2Int(current.x, current.y + 1)  // Up
            };

            foreach (var neighbor in neighbors)
            {
                if (IsValidPosition(neighbor.x, neighbor.y) && roomShape[neighbor.x, neighbor.y] && !visited[neighbor.x, neighbor.y])
                {
                    visited[neighbor.x, neighbor.y] = true;
                    toVisit.Enqueue(neighbor);
                }
            }
        }

        // If any floor tiles are disconnected, reconnect them
        if (connectedFloorTiles.Count < CountFloorTiles(roomShape))
        {
            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (roomShape[x, y] && !visited[x, y])
                    {
                        roomShape[x, y] = true;
                    }
                }
            }
        }
    }

    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < roomWidth && y >= 0 && y < roomHeight;
    }

    private int CountFloorTiles(bool[,] roomShape)
    {
        int count = 0;
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                if (roomShape[x, y]) count++;
            }
        }
        return count;
    }
}
