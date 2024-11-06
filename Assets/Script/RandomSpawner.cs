using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class TimedAreaSpawner : MonoBehaviour
{

    [HideInInspector]
    //dictionary of spawn areas with guid as key
    public Dictionary<string, SpawnArea> spawnAreas = new Dictionary<string, SpawnArea>();




    public RandomTimer trapSpawnTime;                              // Interval between spawns
    public RandomTimer blockSpawnTime;                              // Interval between spawns
    public RandomTimer weaponSpawnTime;

    GameManager gameManager;


    float trapTimer = 15;
    float blockTimer = 10;
    float weaponTimer = 5;
    
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        
            
            trapTimer -= Time.deltaTime;
            blockTimer -= Time.deltaTime;
            weaponTimer -= Time.deltaTime;
            
            if (trapTimer <= 0)
            {
                trapTimer = Random.Range(trapSpawnTime.minTime, trapSpawnTime.maxTime);
                SpawnArea selectedArea = spawnAreas.ElementAt(Random.Range(0, spawnAreas.Count)).Value;
                gameManager.SpawnTrap(getPosition(selectedArea));
            }
            if (blockTimer <= 0)
            {
                blockTimer = Random.Range(blockSpawnTime.minTime, blockSpawnTime.maxTime);
                SpawnArea selectedArea = spawnAreas.ElementAt(Random.Range(0, spawnAreas.Count)).Value;
                gameManager.SpawnBlock(getPosition(selectedArea));
            }
            if (weaponTimer <= 0)
            {
                weaponTimer = Random.Range(weaponSpawnTime.minTime, weaponSpawnTime.maxTime);
                SpawnArea selectedArea = spawnAreas.ElementAt(Random.Range(0, spawnAreas.Count)).Value;
                gameManager.SpawnWeapon(getPosition(selectedArea));
            }
        
            
            
            // Spawn an object in the selected area

            // Wait for the global interval before the next spawn
    }

    private Vector3 getPosition(SpawnArea spawnArea)
    {
        // Choose a random prefab from the array

        // Generate a random position within the spawn area's bounds
        return new Vector3(
            Random.Range(spawnArea.areaTransform.position.x - spawnArea.areaSize.x / 2, spawnArea.areaTransform.position.x + spawnArea.areaSize.x / 2),
            Random.Range(spawnArea.areaTransform.position.y - spawnArea.areaSize.y / 2, spawnArea.areaTransform.position.y + spawnArea.areaSize.y / 2),
            0f // Assuming a 2D game; set Z to 0
        );

    }
}

[System.Serializable]
public struct RandomTimer {
    public float minTime;
    public float maxTime;
}

[System.Serializable]
public struct SpawnArea
{
    public Transform areaTransform;         // Reference to the spawn area GameObject
    public Vector2 areaSize;                // Width and height of the spawn area
}