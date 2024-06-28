using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToPick : MonoBehaviour, IPickable
{
    public Transform groundChecker;
    public LayerMask whatIsGround;
    public float groundCheckDistance;
    public float dragOnGround = 500f;
    public float dragOnAir = 1f;
    public float forceMagnitude = 10f;

    public Rigidbody Rigidbody => rb;

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
        if (Input.GetKeyDown(KeyCode.V))
        {

            rb.AddForce(Vector3.forward * forceMagnitude, ForceMode.Impulse);
        }
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
            if (objectHolder == null)
            {
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
            }
        }
        else
        {
            rb.drag = dragOnAir;
            rb.isKinematic = false;
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