using UnityEngine;
using RagdollCreatures;

public class FallTrigger : MonoBehaviour
{
    private GameManager gameManager;
    void OnTriggerEnter2D(Collider2D other)
    {   
        Debug.Log("Triggered");

        var limb = other.GetComponent<RagdollLimb>();
        if (limb != null)
        {
            Debug.Log("Creature fell off the map and was destroyed.");

            var creature = limb.GetRootParent();
            if (creature != null)
            {
                var controller = creature.GetComponent<RagdollCreatureController>();
                controller.DelayRespawn();
            }
        }
    }
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    
}
