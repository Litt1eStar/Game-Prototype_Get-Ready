using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float sprintSpeed;
    public float movementSpeedOnCarryObject;
    public float rotationSpeed = 90f;
    public float forceMagnitude;

    private Rigidbody rb;
    private float xInput;
    private float zInput;

    public float XInput => xInput;

    private float currentMovementSpeed;

    private Vector3 invalidDirection = new Vector3(-1, -1, -1);
    private Vector3 forwardDirectionOnStartInteraction;
    private bool isCarryingObject;
    private FixedJoint fixedJoint;
    private GameObject carriedObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentMovementSpeed = walkSpeed;
    }

    private void Update()
    {
        CheckInput();
        CharacterRotation();

        if (isCarryingObject && Input.GetKeyDown(KeyCode.E))
        {
            StopObjectInteraction();
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckInput()
    {
        if (isCarryingObject)
        {
            currentMovementSpeed = movementSpeedOnCarryObject;
            if (forwardDirectionOnStartInteraction != invalidDirection)
            {
                int xDirection = Mathf.Abs((int)forwardDirectionOnStartInteraction.x);
                int zDirection = Mathf.Abs((int)forwardDirectionOnStartInteraction.z);

                if (xDirection == 1 && zDirection == 0)
                {
                    xInput = Input.GetAxisRaw("Horizontal");
                    zInput = 0;
                }
                else if (xDirection == 0 && zDirection == 1)
                {
                    xInput = 0;
                    zInput = Input.GetAxisRaw("Vertical");
                }
            }
        }
        else
        {
            currentMovementSpeed = walkSpeed;
            xInput = Input.GetAxisRaw("Horizontal");
            zInput = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentMovementSpeed = sprintSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentMovementSpeed = walkSpeed;
        }
    }

    private void CharacterRotation()
    {
        if (xInput != 0 || zInput != 0)
        {
            if (isCarryingObject) return;
            float targetAngle = Mathf.Atan2(xInput, zInput) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    private void ApplyMovement()
    {
        Vector3 movement = new Vector3(xInput, 0, zInput).normalized * currentMovementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }

    public void StartObjectInteraction(GameObject obj)
    {
        forwardDirectionOnStartInteraction = transform.forward.normalized;
        isCarryingObject = true;
        carriedObject = obj;

        Rigidbody carriedRb = carriedObject.GetComponent<Rigidbody>();
        carriedRb.velocity = Vector3.zero;
        carriedRb.angularVelocity = Vector3.zero;

        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = carriedRb;
        fixedJoint.breakForce = Mathf.Infinity;
        fixedJoint.breakTorque = Mathf.Infinity;

        carriedRb.isKinematic = true;
    }

    public void StopObjectInteraction()
    {
        forwardDirectionOnStartInteraction = invalidDirection;
        isCarryingObject = false;

        if (fixedJoint != null)
        {
            Destroy(fixedJoint);
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigidbody = collision.collider.attachedRigidbody;

        if (rigidbody != null && rigidbody != rb && collision.collider.gameObject.GetComponent<IPickable>() != null)
        {
            Vector3 forceDirection = collision.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }
    }
}
