using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    public LayerMask whatIsPickable;
    public float pickupRadius;
    public float pushForceMagnitude;

    private PlayerMovement playerMovement;
    private ObjectToInteract currentObjectInHand;
    private bool isInteractingWithObject = false;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        HandleUserInput();
    }

    private void HandleUserInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isInteractingWithObject)
            {
                Collider[] pickableColliders = Physics.OverlapSphere(transform.position, pickupRadius, whatIsPickable);
                if (pickableColliders.Length > 0)
                {
                    foreach (Collider collider in pickableColliders)
                    {
                        if (collider.gameObject.TryGetComponent<ObjectToInteract>(out ObjectToInteract objectToInteractWith))
                        {
                            if(objectToInteractWith.ObjectSize == ObjectSize.SMALL)
                            {
                                StartPickupObject(objectToInteractWith);
                            }
                            else if(objectToInteractWith.ObjectSize == ObjectSize.MEDIUM || objectToInteractWith.ObjectSize == ObjectSize.LARGE)
                            {
                                StartPushPullObject(objectToInteractWith);
                            }
                        }
                    }
                }
            }
            else
            {
                StopInteractWithObject();
            }
        }
    }
    private void StartPickupObject(ObjectToInteract objectToInteractWith)
    {
        if (currentObjectInHand == null)
        {
            currentObjectInHand = objectToInteractWith;
            playerMovement.StartPickupObject(objectToInteractWith);
            isInteractingWithObject = true;
        }
    }

    private void StartPushPullObject(ObjectToInteract objectToInteractWith)
    {
        if (currentObjectInHand == null)
        {
            currentObjectInHand = objectToInteractWith;
            playerMovement.StartPushPullObject(objectToInteractWith);
            isInteractingWithObject = true;
        }
    }

    private void StopInteractWithObject()
    {
        if (currentObjectInHand != null)
        {
            currentObjectInHand = null;
            playerMovement.StopInteractWithObject();
            isInteractingWithObject = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}