using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    public CameraController cameraController;
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

    // y 2, z -6
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
                        if (collider.gameObject.TryGetComponent<ObjectToPick>(out ObjectToPick objectToInteractWith))
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

    private void StartPickupObject(ObjectToPick objectToInteractWith)
    {
        if (currentObjectInHand == null)
        {
            currentObjectInHand = objectToInteractWith;
            playerMovement.StartPickupObject(objectToInteractWith);
            cameraController.StartPushPullObject();
            isInteractingWithObject = true;
        }
    }

    private void StartPushPullObject(ObjectToPick objectToInteractWith)
    {
        if (currentObjectInHand == null)
        {
            currentObjectInHand = objectToInteractWith;
            playerMovement.StartPushPullObject(objectToInteractWith);
            cameraController.StartPushPullObject();
            isInteractingWithObject = true;
        }
    }

    private void StopInteractWithObject()
    {
        if (currentObjectInHand != null)
        {
            currentObjectInHand = null;
            playerMovement.StopInteractWithObject();
            cameraController.StopPushPullObject();
            isInteractingWithObject = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}