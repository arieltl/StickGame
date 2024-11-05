using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 areaSize;
    TimedAreaSpawner timedAreaSpawner;
    string areaId;
    void Start()
    {
        areaId = System.Guid.NewGuid().ToString();
        timedAreaSpawner = FindObjectOfType<TimedAreaSpawner>();
        timedAreaSpawner.spawnAreas.Add(areaId,new SpawnArea()
        {
            areaTransform = transform,
            areaSize = areaSize
        });
    }

    void OnDestroy() {
        timedAreaSpawner.spawnAreas.Remove(areaId);
    }

}

