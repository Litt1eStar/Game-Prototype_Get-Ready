using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float sprintSpeed;
    public float rotationSpeed = 90f;

    private Rigidbody rb;
    private float xInput;
    private float zInput;

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

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

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
            float targetAngle = Mathf.Atan2(xInput, zInput) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    private void ApplyMovement()
    {
        Vector3 movement = new Vector3(xInput, 0, zInput).normalized * currentMovementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }
}
