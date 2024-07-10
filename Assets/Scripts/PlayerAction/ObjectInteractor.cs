using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    public LayerMask whatIsPickable;
    public float pickupRadius;
    public float pushForceMagnitude;

    private PlayerMovement playerMovement;
    private ObjectThrower thrower;
    private ObjectToInteract currentObjectInHand;
    private bool isInteractingWithObject = false;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        thrower = GetComponent<ObjectThrower>();
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
                            if (objectToInteractWith.ObjectSize == ObjectSize.SMALL)
                            {
                                StartPickupObject(objectToInteractWith);
                                break; // Stop searching after finding the first object
                            }
                            else if (objectToInteractWith.ObjectSize == ObjectSize.MEDIUM || objectToInteractWith.ObjectSize == ObjectSize.LARGE)
                            {
                                StartPushPullObject(objectToInteractWith);
                                break; // Stop searching after finding the first object
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
            SetObjectOnHand(objectToInteractWith);
            objectToInteractWith.SetObjectHolder(transform); // Attach to interactor
            playerMovement.StartPickupObject(objectToInteractWith);
            isInteractingWithObject = true;
        }
    }

    private void StartPushPullObject(ObjectToInteract objectToInteractWith)
    {
        if (currentObjectInHand == null)
        {
            SetObjectOnHand(objectToInteractWith);
            objectToInteractWith.SetObjectHolder(transform); // Attach to interactor
            playerMovement.StartPushPullObject(objectToInteractWith);
            isInteractingWithObject = true;
        }
    }

    private void StopInteractWithObject()
    {
        if (currentObjectInHand != null)
        {
            currentObjectInHand.ClearObjectHolder();
            ClearObjectOnHand();
            playerMovement.StopInteractWithObject();
            isInteractingWithObject = false;
        }
    }

    private void SetObjectOnHand(ObjectToInteract objectToInteractWith)
    {
        currentObjectInHand = objectToInteractWith;
        thrower.SetObjcetOnHand(currentObjectInHand);
    }

    private void ClearObjectOnHand()
    {
        currentObjectInHand = null;
        thrower.ClearObjectOnHand();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
