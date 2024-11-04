using UnityEngine;
using UnityEngine.SceneManagement;
using RagdollCreatures;

public class SkateBoardPickup : MonoBehaviour
{
    public string newSceneName; // The name of the new scene to load
    private GameManager gameManager;

    void Start()
    {
        // Find the GameManager instance if needed for any future functionality
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Pickup Triggered");

        // Check if the colliding object has a RagdollLimb component (for player detection)
        var limb = other.GetComponent<RagdollLimb>();
        if (limb != null)
        {
            Debug.Log("Player picked up the object. Changing scene to: " + newSceneName);

            // Load the specified scene
            SceneManager.LoadScene(newSceneName);

            // Destroy this object after pickup to prevent multiple triggers
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Non-player object entered the pickup trigger: " + other.name);
        }
    }
}
