using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float sprintSpeed;
    public float movementSpeedOnCarryObject;
    public float rotationSpeed = 90f;

    private Rigidbody rb;
    private float xInput;
    private float zInput;

    public float XInput => xInput;

    private float currentMovementSpeed;

    private Vector3 invalidDirection = new Vector3(-1, -1, -1);
    private Vector3 forwardDirectionOnStartInteraction;
    private bool isCarryingObject;
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

    public void StartObjectInteraction()
    {
        forwardDirectionOnStartInteraction = transform.forward.normalized;
        isCarryingObject = true;
    }

    public void StopObjectInteraction()
    {
        forwardDirectionOnStartInteraction = new Vector3(-1, -1, -1);
        isCarryingObject = false;
    }
}