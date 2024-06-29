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
        HandleUserInput();
    }

    private void HandleUserInput()
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
        if (currentObjectInHand == null)
        {
            Collider[] pickableColliders = Physics.OverlapSphere(transform.position, pickupRadius, whatIsPickable);
            if (pickableColliders.Length > 0)
            {
                if (pickableColliders[0].gameObject.TryGetComponent<ObjectToPick>(out ObjectToPick itemToPickup))
                {
                    currentObjectInHand = itemToPickup;
                    playerMovement.StartPushPullObject(itemToPickup);
                    isInteractingWithObject = true;
                }
            }
        }
    }

    private void ReleaseObject()
    {
        if (currentObjectInHand != null)
        {
            currentObjectInHand = null;
            playerMovement.StopPushPullObject();
            isInteractingWithObject = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}