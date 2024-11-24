using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RagdollCreatures;

public class PlaceablePickUp : MonoBehaviour
{
    public PlaceableItem placeableItem;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    { 
        var limb = collision.gameObject.GetComponent<RagdollLimb>();
        if (limb == null) return;

        var creature = limb.GetRootParent();
        if (creature == null) return;

        var controller = creature.GetComponent<RagdollCreatureController>();
        if (controller == null) return;

        var player = gameManager.players[controller.playerId];
        //Debug.Log("Player " + controller.playerId + " picked up " );
        player.collectedItem = placeableItem;
        
        
        Destroy(gameObject);
    }
}
