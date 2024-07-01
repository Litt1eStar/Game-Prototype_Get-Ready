using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToPick : MonoBehaviour
{
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float dragOnGround = 500f;
    [SerializeField] private float dragOnAir = 1f;
    [SerializeField] private float forceMagnitude = 10f;

    [SerializeField] private ObjectSO objectData;

    private Rigidbody rb;
    private Transform objectHolder;
    private bool isGrounded = true;
    public ObjectSize ObjectSize => objectData.Size;
    public float ObjectWeight => objectData.Weight;
    public Rigidbody Rigidbody => rb;

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
            transform.position = objectHolder.position;
            //rb.MovePosition(objectHolder.transform.position);
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
        else if(!isGrounded) 
        {
            if (objectHolder == null)
            {
                rb.drag = dragOnAir;
                rb.isKinematic = false;
            }
            else
            {
                rb.drag = dragOnAir;
                rb.isKinematic = true;
            }
        }
    }

    public void SetObjectHolder(Transform objectHolder)
    {
        this.objectHolder = objectHolder;
        transform.parent = objectHolder;
    }

    public void ClearObjectHolder()
    {
        objectHolder = null;
        transform.parent = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundChecker.position, groundChecker.position + new Vector3(0, -groundCheckDistance, 0));
    }
}