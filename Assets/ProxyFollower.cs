using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyFollower : MonoBehaviour
{
    public string targetTag;
    private Transform target;

    void Start()
    {
        FindTarget();    
    }

    void Update()
    {
        // Check if the target is null (destroyed) and try to find a new one
        if (target == null)
        {
            FindTarget();
        }

        // If the target is not null, follow it
        if (target != null)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }

    void FindTarget()
    {
        // Ensure the parent exists
        Transform parent = transform.parent;
        if (parent == null) return;
        
        // Find the sibling dynamically (exclude this object)
        Transform sibling = null;
        foreach (Transform child in parent)
        {
            if (child != transform) // Exclude this object
            {
                sibling = child;
                break;
            }
        }

        // If no sibling is found, exit
        if (sibling == null) return;

        // Find the "Root" child of the sibling
        Transform root = sibling.Find("Root");
        if (root == null) return;

        // Search for a child of "Root" with the target tag
        foreach (Transform child in root)
        {
            if (child.CompareTag(targetTag))
            {
                target = child;
                return;
            }
        }
    }
}
