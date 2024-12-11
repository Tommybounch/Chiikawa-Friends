using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile interactedTile;

    [SerializeField] private Tile plantedTile; // Stage 1
    [SerializeField] private Tile growingTile; // Stage 2
    [SerializeField] private Tile matureTile;  // Stage 3

    private Dictionary<Vector3Int, int> growthStages = new Dictionary<Vector3Int, int>();
    private Dictionary<Vector3Int, int> plantingDays = new Dictionary<Vector3Int, int>();

    [SerializeField] private DayNightScript dayNightScript; // Reference to your time system

    void Start()
    {
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            interactableMap.SetTile(position, hiddenInteractableTile);
        }
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);
        if (tile != null && tile.name == "Interactable")
        {
            return true;
        }
        return false;
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
    }

    public void SetPlanted(Vector3Int position)
    {
        interactableMap.SetTile(position, plantedTile);
        growthStages[position] = 1; // Stage 1
        plantingDays[position] = dayNightScript.days; // Record the day it was planted
        
    }

    public void UpdateGrowth()
    {
        int currentDay = dayNightScript.days;
        List<Vector3Int> updatedPositions = new List<Vector3Int>();

        foreach (var position in growthStages.Keys)
        {
            int stage = growthStages[position];
            int plantedDay = plantingDays[position];

            // Check if enough time has passed to grow to the next stage
            if (currentDay - plantedDay >= stage) 
            {
                if (stage == 1)
                {
                    interactableMap.SetTile(position, growingTile); // Update to stage 2
                    growthStages[position] = 2;
                }
                else if (stage == 2)
                {
                    interactableMap.SetTile(position, matureTile); // Update to stage 3 (final stage)
                    growthStages[position] = 3;
                }

                plantingDays[position] = currentDay; // Reset planting day for the next stage
                updatedPositions.Add(position); // Track updated tiles
            }
        }

        // Remove fully grown plants from tracking (if you want)
        foreach (var position in updatedPositions)
        {
            if (growthStages[position] == 3)
            {
                growthStages.Remove(position);
                plantingDays.Remove(position);
            }
        }
    }
}
