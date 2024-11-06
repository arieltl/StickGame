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
    public List<GameObject> weapons = new List<GameObject>();
    
    public List<PlayerInfo> players = new List<PlayerInfo>();
    public GameObject trapIconPrefab;
    float trapTimer = 20f;
    public bool isGameRunning = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }



    public void SpawnTrap(Vector3 position) {
        var trap = Instantiate(trapIconPrefab, position, Quaternion.identity);

        var renderer = trap.GetComponent<SpriteRenderer>();
        var trapI = Random.Range(0, placeableTraps.Count);

        if (renderer != null)
        {
            renderer.sprite = placeableTraps[trapI].icon;
        }

        var pickUp = trap.GetComponent<PlaceablePickUp>();
        pickUp.placeableItem = placeableTraps[trapI];
            
    }

    public void SpawnBlock(Vector3 position) {
        var block  = Instantiate(trapIconPrefab, position, Quaternion.identity);
        var blockRenderer = block.GetComponent<SpriteRenderer>();
        var blockI = Random.Range(0, placeableBlocks.Count);
        if (blockRenderer != null)
        {
            blockRenderer.sprite = placeableBlocks[blockI].icon;
        }
            
        var blockPickUp = block.GetComponent<PlaceablePickUp>();
        var blockItem = placeableBlocks[blockI];
        blockPickUp.placeableItem = blockItem;
    }
    
    public void SpawnWeapon(Vector3 position) {
        var weapon = Instantiate(weapons[Random.Range(0, weapons.Count)], position, Quaternion.identity);
    }
    public bool AddScore(int playerId, int points)
    {
        // Increase the score for the player with the given playerId
        players[playerId].score += points;

        if (players[playerId].score >= 3)
        {
            LoadLevel("EndGame");
            return true;

        }
        return false;
    }
    public void StartGame()
    {
        isGameRunning = true;
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
public class PlayerInfo
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