using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] LayerMask interactableMask;
    [SerializeField] float interactionRange;
    private Interactable currentInteractable;

    private void Update()
    {
        CheckForInteractableInDirection(transform.forward);
        
        if(Input.GetMouseButtonDown(0))
        {
            if (currentInteractable != null) currentInteractable.Interact();
        }
    }

    private void CheckForInteractableInDirection(Vector3 direction)
    {
        Debug.DrawRay(transform.position, direction.normalized * interactionRange);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, interactionRange, interactableMask))
        {
            Interactable i = hit.collider.GetComponent<Interactable>();
            if (i != null)
            {
                currentInteractable = i;
                UIController.Instance.InfoMessage(currentInteractable.GetTooltip(),0);
            }
        }
        else
        {
            if (currentInteractable != null) currentInteractable = null;
        }
    }

}
