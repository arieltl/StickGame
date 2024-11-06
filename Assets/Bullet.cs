using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RagdollCreatures;

public class Bullet : MonoBehaviour
{
    public GameManager gameManager;
    private int bulletDamage = 0;
    private bool damageApplied = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (damageApplied) return;

        var limb = collision.gameObject.GetComponent<RagdollLimb>();
        if (limb == null) {
            Destroy(gameObject);
            return;
        }

        var creature = limb.GetRootParent();
        if (creature == null) return;

        var controller = creature.GetComponent<RagdollCreatureController>();
        if (controller == null) return;

        var player = gameManager.players[controller.playerId];
        if (player == null) return;
        
        gameManager.ApplyDamage(controller.playerId, bulletDamage);
        damageApplied = true;

        Destroy(gameObject);
    }

    public void SetDamage(int damage)
    {
        this.bulletDamage = damage;
    }
}
