using System.Collections;
using System.Collections.Generic;
using RagdollCreatures;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    //List of spawn positions
    Transform[] spawnPositions;

    // Start is called before the first frame update
    GameObject[] players;

    void Start() {
        //finda all spawn positions by tag
        var spawns = GameObject.FindGameObjectsWithTag("Respawn");
        spawnPositions = new Transform[spawns.Length];
        for (int i = 0; i < spawns.Length; i++) {
            spawnPositions[i] = spawns[i].transform;
        }
        PositionPlayers();
    }

    // Update is called once per frame
    void Update() { }


    void PositionPlayers() {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int index = 0; index < players.Length; index++) {
            //Debug.Log("position: " + spawnPositions[index].position);
            var controller = players[index].GetComponent<RagdollCreatureController>();
            players[index].transform.position = spawnPositions[index].position;
            controller.Respawn();
        }
    }
    
    
}