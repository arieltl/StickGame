using System.Collections;
using System.Collections.Generic;
using RagdollCreatures;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    private List<Transform> spawnPositions;
    private GameObject[] players;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start() {
        FindSpawnPositions();
        PositionPlayers(); // Reposiciona jogadores ao iniciar
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        StartCoroutine(PositionPlayersAfterSceneLoad());
    }

    private void FindSpawnPositions() {
        var spawns = GameObject.FindGameObjectsWithTag("Respawn");
        spawnPositions = new List<Transform>();
        foreach (var spawn in spawns) {
            spawnPositions.Add(spawn.transform);
        }
    }

    private IEnumerator PositionPlayersAfterSceneLoad() {
        yield return null; // Espera um frame para garantir que tudo esteja carregado

        FindSpawnPositions(); // Recarrega pontos de spawn para a nova cena
        PositionPlayers();
    }

    private void PositionPlayers() {
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > spawnPositions.Count) {
            Debug.LogError("Número de jogadores excede os pontos de spawn disponíveis!");
            return;
        }

        for (int i = 0; i < players.Length; i++) {
            var player = players[i];
            var controller = player.GetComponent<RagdollCreatureController>();

            if (i < spawnPositions.Count) {
                player.transform.position = spawnPositions[i].position;
                player.transform.rotation = spawnPositions[i].rotation;
                controller.Respawn();
            } else {
                Debug.LogWarning($"Jogador {i} não foi posicionado: número insuficiente de pontos de spawn.");
            }
        }
    }
}
