using UnityEngine;
using RagdollCreatures;

public class FallTrigger : MonoBehaviour
{
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {   
        //Get Tag of the object that entered the trigger
        string tag = other.gameObject.tag;
        
        if (tag == "PlayerBody")
        {
            RespawnPlayer(other);
        } else if (tag == "Equipable" || tag == "Bullet")
        {
            Destroy(other.gameObject); //Destroy the object that entered the trigger
        }
    }

    private void RespawnPlayer(Collider2D other)
    {
        var limb = other.GetComponent<RagdollLimb>();
        if (limb != null)
        {
            //Debug.Log("Creature fell off the map and was destroyed.");

            var creature = limb.GetRootParent();
            if (creature != null)
            {
                var controller = creature.GetComponent<RagdollCreatureController>();
                controller.DelayRespawn();
            }
        }
    }   
}
