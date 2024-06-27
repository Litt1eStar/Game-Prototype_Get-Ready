using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToPick : MonoBehaviour
{
    public Transform groundChecker;
    public LayerMask whatIsGround;
    public float groundCheckDistance;
    public float dragOnGround = 500f;
    public float dragOnAir = 1f;

    private Rigidbody rb;
    private GameObject objectHolder;
    private bool isGrounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGround();
    }
    private void FixedUpdate()
    {
        SetRigidbodyInfo();
        if (objectHolder != null) 
        {
            rb.MovePosition(objectHolder.transform.position);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(groundChecker.position, Vector3.down, groundCheckDistance, whatIsGround);
    }
    private void SetRigidbodyInfo()
    {
        if (isGrounded)
        {
            rb.drag = dragOnGround;
        }
        else
        {
            rb.drag = dragOnAir;
        }
    }
    public void PickupObject(GameObject objectHolder)
    {
        this.objectHolder = objectHolder;
        transform.parent = objectHolder.transform;
        rb.useGravity = false;
    }

    public void DropObject()
    {
        objectHolder = null;
        transform.parent = null;
        rb.useGravity = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundChecker.position, groundChecker.position + new Vector3(0, -groundCheckDistance, 0));
    }
}
