using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public GameObject objectHolder;
    public LayerMask whatIsPickable;
    public float pickupRadius;

    private bool isHoldingObject = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isHoldingObject)
        {
            PickupObject();
        }
        else if(Input.GetKeyDown(KeyCode.E) && isHoldingObject)
        {
            DropObject();
        }
    }

    private void DropObject()
    {
        ObjectToPick objectInHand = objectHolder.GetComponentInChildren<ObjectToPick>();
        if (objectInHand != null) 
        {
            objectInHand.DropObject();
            isHoldingObject = false;
        }
    }
    private void PickupObject()
    {
        Collider[] pickableColliders = Physics.OverlapSphere(transform.position, pickupRadius, whatIsPickable);
        if(pickableColliders.Length > 0)
        {
            if(pickableColliders[0].gameObject.TryGetComponent<ObjectToPick>(out ObjectToPick itemToPickup))
            {
                itemToPickup.PickupObject(objectHolder);
                isHoldingObject = true;
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
