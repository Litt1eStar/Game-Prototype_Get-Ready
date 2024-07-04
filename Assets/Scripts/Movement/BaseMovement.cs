using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    protected Rigidbody rb;
    protected float xInput;
    protected float zInput;
    protected float currentMovementSpeed;

    protected Vector3 invalidDirection = new Vector3(-1, -1, -1);
    protected Vector3 forwardDirectionOnStartInteraction;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {

    }
}
