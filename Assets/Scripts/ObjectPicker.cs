using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public GameObject objectHolder;
    public LayerMask whatIsPickable;
    public float pickupRadius;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        Collider[] pickableColliders = Physics.OverlapSphere(transform.position, pickupRadius, whatIsPickable);
        if(pickableColliders.Length > 0)
        {
            if(pickableColliders[0].gameObject.TryGetComponent<ObjectToPick>(out ObjectToPick itemToPickup))
            {
                itemToPickup.PickupObject(objectHolder);
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
