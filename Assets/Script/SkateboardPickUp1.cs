using UnityEngine;
using UnityEngine.SceneManagement;
using RagdollCreatures;

public class SkateBoardPickup : MonoBehaviour
{
    public string newSceneName; // The name of the new scene to load
    private GameManager gameManager;

    void Start()
    {
        // Find the GameManager instance for score tracking and scene management
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Pickup Triggered");

        // Check if the colliding object has a RagdollLimb component (for player detection)
        var limb = other.GetComponent<RagdollLimb>();
        if (limb != null)
        {
            // Get the RagdollCreatureController from the player
            var playerController = other.GetComponentInParent<RagdollCreatureController>();
            if (playerController != null)
            {
                int playerId = playerController.playerId;  // Assume playerId is set on each player

                Debug.Log("Player picked up the object. Changing scene to: " + newSceneName);

                // Add a point to the player's score in GameManager
                gameManager.AddScore(playerId, 1);

                // Load the specified scene
                SceneManager.LoadScene(newSceneName);

                // Destroy this object after pickup to prevent multiple triggers
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("No RagdollCreatureController found on player.");
            }
        }
        else
        {
            Debug.Log("Non-player object entered the pickup trigger: " + other.name);
        }
    }
}
