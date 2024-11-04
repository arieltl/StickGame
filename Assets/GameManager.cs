using System.Collections;
using System.Collections.Generic;
using RagdollCreatures;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<PlayerInfo> players = new List<PlayerInfo>();
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
    
    public void  ApplyDamage(int playerId, int damage)
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
}