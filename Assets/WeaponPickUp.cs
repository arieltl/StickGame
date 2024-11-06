using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RagdollCreatures;

public class WeaponPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

        var InteractableScript = controller.GetComponentInChildren<Interact>();
        if (InteractableScript == null) return;

        InteractableScript.InteractOnCollision(this.gameObject);
    }
}
