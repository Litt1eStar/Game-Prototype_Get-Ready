using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    [Header("Player Movement Setting")]
    public float walkSpeed;
    public float sprintSpeed;
    public float movementSpeedOnCarryObject;
    public float rotationSpeed = 90f;

    [Header("Player Interactor")]
    [SerializeField] private Transform objectHolder;


    private ObjectToInteract currentObject;
    private bool isPushPull = false;
    private bool isPickup = false;
    public float CurrentMovementSpeed => currentMovementSpeed;
    public float HorizontalInput => xInput;
    public float VerticalInput => zInput;

    public override void Start()
    {
        base.Start();
        currentMovementSpeed = walkSpeed;
    }

    public override void Update()
    {
        base.Update();
        GetMovementInput();
        CharacterRotation();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsPushPullObject) 
        {            
            Vector3 movement = new Vector3(xInput, 0, zInput).normalized * currentMovementSpeed * Time.fixedDeltaTime;
            currentObject.Rigidbody.MovePosition(currentObject.transform.position + movement);
        }
    }

    private void GetMovementInput()
    {
        AdjustMovementSpeedBasedOnObjectWeight();
        
        if (IsPickupObject)
        {
            xInput = Input.GetAxisRaw("Horizontal");
            zInput = Input.GetAxisRaw("Vertical");
            return;
        }
        else if (IsPushPullObject)
        {
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

    private void AdjustMovementSpeedBasedOnObjectWeight()
    {
        if (currentObject == null)
        {
            currentMovementSpeed = walkSpeed;
            return;
        }

        float objectWeight = currentObject.ObjectWeight;

        if(objectWeight > 0f && objectWeight <= 10f)
            currentMovementSpeed = walkSpeed * 0.8f;
        else if(objectWeight > 10f && objectWeight <= 30)
            currentMovementSpeed = walkSpeed * 0.6f;
        else if(objectWeight > 30)
            currentMovementSpeed = walkSpeed * 0.25f;
    }

    private void CharacterRotation()
    {
        if (xInput != 0 || zInput != 0)
        {
            if (IsPushPullObject) return;
            float targetAngle = Mathf.Atan2(xInput, zInput) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    private void ApplyMovement()
    {
        Vector3 movement = new Vector3(xInput, 0, zInput).normalized * currentMovementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }
    public void StartPickupObject(ObjectToInteract itemToPickup)
    {
        isPickup = true;
        forwardDirectionOnStartInteraction = transform.forward.normalized;
        currentObject = itemToPickup;
        currentObject.SetObjectHolder(objectHolder);
    }

    public void StartPushPullObject(ObjectToInteract itemToPickup)
    {
        forwardDirectionOnStartInteraction = transform.forward.normalized;
        isPushPull = true;
        currentObject = itemToPickup;
    }

    public void StopInteractWithObject()
    {
        forwardDirectionOnStartInteraction = new Vector3(-1, -1, -1);
        isPushPull = false;
        isPickup = false;
        currentObject.ClearObjectHolder();
        currentObject = null;
    }

    public bool IsMovingToleft => xInput < 0;
    public bool IsMovingToRight => xInput > 0;
    public bool IsMovingForward => zInput > 0;
    public bool IsMovingBackward => zInput < 0;
    public bool IsStandStill => xInput == 0 && zInput == 0;
    public bool IsPickupObject => currentObject != null && isPickup && !isPushPull;
    public bool IsPushPullObject => currentObject != null && isPushPull && !isPickup;
}