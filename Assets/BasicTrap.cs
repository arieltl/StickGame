using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RagdollCreatures;

public class BasicTrap : MonoBehaviour
{

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        KillPlayer(collision.gameObject);
    }

    private void KillPlayer(GameObject gameObject)
    {
        var limb = gameObject.GetComponent<RagdollLimb>();
        if (limb == null) return;

        var creature = limb.GetRootParent();
        if (creature == null) return;

        var controller = creature.GetComponent<RagdollCreatureController>();
        if (controller == null) return;

        gameManager.ApplyDamage(controller.playerId, 100);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        KillPlayer(collider.gameObject);
    }
}
