using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Setting")]
    public float walkSpeed;
    public float sprintSpeed;
    public float movementSpeedOnCarryObject;
    public float rotationSpeed = 90f;

    private Rigidbody rb;
    private float xInput;
    private float zInput;

    private ObjectToPick currentObject;
    private Vector3 invalidDirection = new Vector3(-1, -1, -1);
    private Vector3 forwardDirectionOnStartInteraction;
    private bool isInteractionObject = false;
    private float currentMovementSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentMovementSpeed = walkSpeed;
    }

    private void Update()
    {
        CheckInput();
        CharacterRotation();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isInteractionObject && currentObject != null) 
        {
            Vector3 movement = new Vector3(xInput, 0, zInput).normalized * currentMovementSpeed * Time.fixedDeltaTime;
            currentObject.Rigidbody.MovePosition(currentObject.transform.position + movement);
        }
    }

    private void CheckInput()
    {
        if (isInteractionObject)
        {
            currentMovementSpeed = movementSpeedOnCarryObject;
            //Clamp Input value based on forward direction of player when start interaction
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
            if (isInteractionObject) return;
            float targetAngle = Mathf.Atan2(xInput, zInput) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    private void ApplyMovement()
    {
        Vector3 movement = new Vector3(xInput, 0, zInput).normalized * currentMovementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }
    public void StartPushPullObject(ObjectToPick itemToPickup)
    {
        forwardDirectionOnStartInteraction = transform.forward.normalized;
        isInteractionObject = true;
        currentObject = itemToPickup;
    }

    public void StopPushPullObject()
    {
        forwardDirectionOnStartInteraction = new Vector3(-1, -1, -1);
        isInteractionObject = false;
        currentObject = null;
    }

    public bool IsMovingToleft => xInput < 0;
    public bool IsMovingToRight => xInput > 0;
    public bool IsMovingForward => zInput > 0;
    public bool IsMovingBackward => zInput < 0;
    public bool IsStandStill => xInput == 0 && zInput == 0;
}