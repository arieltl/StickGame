using System.Collections;
using System.Collections.Generic;
using RagdollCreatures;
using UnityEngine;

public class Activatetrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // On collision with player, activate the trap
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var limb = collision.gameObject.GetComponent<RagdollLimb>();
        if (limb != null)    
        {
            // Activate the trap
            GetComponent<Animator>().SetBool("isActive", true);
        }
    }
}
