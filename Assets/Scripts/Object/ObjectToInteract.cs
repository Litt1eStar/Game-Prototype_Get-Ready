using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToInteract : MonoBehaviour
{
    [Header("Checker Setting")]
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform aboveObjectCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsObject;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float aboveCheckDistance;

    [Header("Rigidbody Setting")]
    [SerializeField] private float dragOnGround = 500f;
    [SerializeField] private float dragOnAir = 1f;

    [Header("Object Data")]
    [SerializeField] private ObjectSO objectData;

    private Rigidbody aboveObject;
    private FixedJoint joint;
    private Rigidbody rb;
    private Transform objectHolder;
    private bool isThrowing = false;
    private bool isGrounded = true;
    private bool isAboveObject = false;

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
        CheckAboveObject();
        SetRigidbodyInfo();

        if (objectData.Size != ObjectSize.SMALL)
            ApplyObjectToChildOfBasedObject();
        else if (objectData.Size == ObjectSize.SMALL && objectHolder != null)
            transform.localPosition = Vector3.zero;
    }

    private void ApplyObjectToChildOfBasedObject()
    {
        if (isAboveObject && aboveObject != null)
        {
            if (joint == null)
            {
                joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = aboveObject;
                Debug.Log("Connected 2 joint together");
            }

            if (isGrounded && joint != null)
            {
                joint.massScale = rb.mass;
            }
        }
        else
        {
            if (joint != null)
            {
                Debug.Log("Destroy Joint");
                Destroy(joint);
            }
        }
    }

    private void CheckAboveObject()
    {
        RaycastHit hit;
        isAboveObject = Physics.Raycast(aboveObjectCheck.position, Vector3.up, out hit, aboveCheckDistance, whatIsObject);
        if (isAboveObject && hit.collider.gameObject != null)
        {
            aboveObject = hit.collider.attachedRigidbody;
        }
        else
        {
            aboveObject = null;
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
            rb.isKinematic = false;
        }
        else
        {
            if (isThrowing)
            {
                EnablePhysicAction();
            }
            else if (!isThrowing && objectHolder != null)
            {
                DisablePhysicAction();
            }
            else
            {
                EnablePhysicAction();
            }
        }
    }

    public void EnablePhysicAction()
    {
        rb.drag = dragOnAir;
        rb.isKinematic = false;
    }

    public void DisablePhysicAction()
    {
        rb.drag = dragOnAir;
        rb.isKinematic = true;
    }

    public void SetObjectHolder(Transform objectHolder)
    {
        this.objectHolder = objectHolder;
        transform.parent = objectHolder;
    }

    public void StartThrowingObject()
    {
        isThrowing = true;
        EnablePhysicAction(); // Ensure physics is enabled when throwing
    }

    public void FinishThrowingObject()
    {
        isThrowing = false;
    }

    public void ClearObjectHolder()
    {
        objectHolder = null;
        transform.parent = null;
    }

    public bool ObjectIsOnGround() => isGrounded;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundChecker.position, groundChecker.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(aboveObjectCheck.position, aboveObjectCheck.position + new Vector3(0, aboveCheckDistance, 0));
    }
}
