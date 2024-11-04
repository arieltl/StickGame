using System.Collections;
using System.Collections.Generic;
using RagdollCreatures;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<PlaceableItem> placeableTraps = new List<PlaceableItem>();
    public List<PlaceableItem> placeableBlocks = new List<PlaceableItem>();
    public List<PlayerInfo> players = new List<PlayerInfo>();
    public GameObject trapIconPrefab;
    float trapTimer = 20f;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        trapTimer += Time.deltaTime;
        if (trapTimer >= 20)
        {
            trapTimer = 0;

            var trap = Instantiate(trapIconPrefab, new Vector3(3, -2, 0), Quaternion.identity);

            var renderer = trap.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = placeableTraps[0].icon;
            }

            var pickUp = trap.GetComponent<PlaceablePickUp>();
            pickUp.placeableItem = placeableTraps[0];
        }
    }
    
    public void StartGame()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i].GetComponent<RagdollCreatureController>();
            player.playerId = i;
            this.players.Add(new PlayerInfo { health = 100, score = 0 ,controller = player});
        }
        LoadLevel("ExclusionZoneTest 1");
    }
    
    public void ApplyDamage(int playerId, int damage)
    {
       var player = players[playerId];
       if (player.controller.creature.isDead) return;
       player.health -= damage;
        if (player.health <= 0)
        {
            player.controller.creature.DeactivateAllMuscles();
            player.controller.creature.isDead = true;
            StartCoroutine(RespawnPlayer(playerId));
        } 
    }
    
    IEnumerator RespawnPlayer(int playerId)
    {
        yield return new WaitForSeconds(3);
        var player = players[playerId];
        player.controller.Respawn();
    }
    // Update is called once per frame
    public void LoadLevel(string levelName) {
        StartCoroutine(LoadLevelAsync(levelName));
    }
    
    IEnumerator LoadLevelAsync(string levelName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        if (asyncLoad == null) {
            Debug.LogError("Failed to load level " + levelName);
            yield return null;
        }
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
//struct player info with health and score
public struct PlayerInfo
{
    public int health;
    public int score;
    public RagdollCreatureController controller;
    public PlaceableItem? collectedItem;
}
[System.Serializable]
public struct PlaceableItem
{
    public GameObject prefab;
   
    public Sprite icon;
}