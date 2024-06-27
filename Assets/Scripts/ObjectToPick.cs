using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToPick : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject objectHolder;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (objectHolder != null) 
        {
            rb.MovePosition(objectHolder.transform.position);
        }
    }
    public void PickupObject(GameObject objectHolder)
    {
        this.objectHolder = objectHolder;
        rb.useGravity = false;
    }
}
