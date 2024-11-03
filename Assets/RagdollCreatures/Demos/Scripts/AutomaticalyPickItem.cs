using RagdollCreatures;
using Unity.VisualScripting;
using UnityEngine;

public class AutomaticalyPickItem : MonoBehaviour
{

    public void OnPickableItemEnter(RagdollLimb limb, Collider2D col)
    {
        if (col.gameObject.CompareTag("PickableItem"))
        {
            col.gameObject.GetComponent<IInteractable>().interact(gameObject);
        }
    }
  
}