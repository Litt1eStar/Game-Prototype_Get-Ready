using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    [SerializeField] private KeyCode buttonToThrow;
    [SerializeField] private float throwForce;
    [SerializeField] private float upperThrowAngle = 2f;
    private PlayerMovement playerMovement;
    private ObjectToInteract objectOnHand;
    private Vector3 forwardDirection;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        forwardDirection = Vector3.zero;
    }

    private void Update()
    {
        CheckForwardDirection();
        if (Input.GetKeyDown(buttonToThrow) && objectOnHand != null && objectOnHand.ObjectSize == ObjectSize.SMALL)
        {
            ThrowObject();
        }
        CheckIfObjectFinishThrowing();
    }

    private void CheckIfObjectFinishThrowing()
    {
        if (objectOnHand == null) return;

        if (objectOnHand.ObjectIsOnGround())
        {
            objectOnHand.FinishThrowingObject();
            //ClearObjectOnHand(); // Clear reference after throwing is complete
        }
    }

    private void CheckForwardDirection()
    {
        forwardDirection = Vector3.Normalize(transform.forward);
    }

    private void ThrowObject()
    {
        objectOnHand.StartThrowingObject();
        objectOnHand.ClearObjectHolder();
        Vector3 force = new Vector3(forwardDirection.x, forwardDirection.y + upperThrowAngle, forwardDirection.z) * throwForce;
        objectOnHand.Rigidbody.AddForce(force);
    }

    public void SetObjcetOnHand(ObjectToInteract newObject)
    {
        objectOnHand = newObject;
    }

    public void ClearObjectOnHand()
    {
        objectOnHand = null;
    }
}
