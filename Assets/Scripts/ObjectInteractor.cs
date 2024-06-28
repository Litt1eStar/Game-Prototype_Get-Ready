using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    public GameObject objectHolder;
    public GameObject pushObjectHolder;
    public LayerMask whatIsPickable;
    public float pickupRadius;
    public float pushForceMagnitude;

    private PlayerMovement playerMovement;
    private ObjectToPick currentObjectInHand;
    private bool isInteractingWithObject = false;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isInteractingWithObject)
                PushObject();
            else
                ReleaseObject();
        }
    }

    private void PushObject()
    {
        Debug.Log("Push Object");
        if (currentObjectInHand == null)
        {
            Collider[] pickableColliders = Physics.OverlapSphere(transform.position, pickupRadius, whatIsPickable);
            if (pickableColliders.Length > 0)
            {
                if (pickableColliders[0].gameObject.TryGetComponent<ObjectToPick>(out ObjectToPick itemToPickup))
                {
                    itemToPickup.PickupObject(pushObjectHolder);
                    currentObjectInHand = itemToPickup;
                    playerMovement.StartObjectInteraction();
                    isInteractingWithObject = true;
                }
            }
        }
    }

    private void ReleaseObject()
    {
        if (currentObjectInHand != null)
        {
            currentObjectInHand.DropObject();
            playerMovement.StopObjectInteraction();
            currentObjectInHand = null;
            isInteractingWithObject = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}