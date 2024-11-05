using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionDelay = 2f;  // Time before the bomb explodes after being triggered

    public GameObject explosionEffect; // Optional: Explosion particle effect prefab

    void Start()
    {
        // Start the explosion countdown
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(explosionDelay);
        
        // Trigger explosion effect if there is one
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Call the explosion logic
        Explode();

        // Destroy the bomb object after it explodes
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Find all colliders in the explosion radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in hitColliders)
        {
            // Check if the object has the tag "Destructible"
            if (collider.CompareTag("Destructible"))
            {
                Destroy(collider.gameObject); // Destroy the object
            }
        }

        Debug.Log("Bomb exploded, destroyed all destructible items in radius.");
    }

    // Optional: Visualize the explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
