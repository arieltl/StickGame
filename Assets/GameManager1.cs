using System.Collections;
using System.Collections.Generic;
using RagdollCreatures;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<PlaceableItem> placeableTraps = new List<PlaceableItem>();
    public List<PlaceableItem> placeableBlocks = new List<PlaceableItem>();
    public List<PlayerInfo> players = new List<PlayerInfo>();
    public GameObject trapIconPrefab;
    float trapTimer = 20f;
    public bool isGameRunning = false;

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
            SpawnTraps();
        }
    }

    private void SpawnTraps()
    {
        // Code to spawn traps (kept here for clarity)
        var trap = Instantiate(trapIconPrefab, new Vector3(3, -2, 0), Quaternion.identity);
        var renderer = trap.GetComponent<SpriteRenderer>();
        var trapI = Random.Range(0, placeableTraps.Count);

        if (renderer != null)
        {
            renderer.sprite = placeableTraps[trapI].icon;
        }

        var pickUp = trap.GetComponent<PlaceablePickUp>();
        pickUp.placeableItem = placeableTraps[trapI];

        var block = Instantiate(trapIconPrefab, new Vector3(5, 6, 0), Quaternion.identity);
        var blockRenderer = block.GetComponent<SpriteRenderer>();
        var blockI = Random.Range(0, placeableBlocks.Count);
        if (blockRenderer != null)
        {
            blockRenderer.sprite = placeableBlocks[blockI].icon;
        }

        var blockPickUp = block.GetComponent<PlaceablePickUp>();
        blockPickUp.placeableItem = placeableBlocks[blockI];
    }

    public void StartGame()
    {
        isGameRunning = true;
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerObjects.Length; i++)
        {
            var player = playerObjects[i].GetComponent<RagdollCreatureController>();
            player.playerId = i;
            this.players.Add(new PlayerInfo { health = 100, score = 0, controller = player });
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

    public void AddScore(int playerId, int points)
    {
        // Increase the score for the player with the given playerId
        players[playerId].score += points;

        // Print the new score to the debug log
        Debug.Log("Player " + playerId + " score: " + players[playerId].score);
    }

    IEnumerator RespawnPlayer(int playerId)
    {
        yield return new WaitForSeconds(3);
        var player = players[playerId];
        player.controller.Respawn();
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelAsync(levelName));
    }

    IEnumerator LoadLevelAsync(string levelName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        if (asyncLoad == null)
        {
            Debug.LogError("Failed to load level " + levelName);
            yield return null;
        }
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

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
