using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Virtual Camera List")]
    public CinemachineVirtualCamera virtualCameraWhileNotInteracting;
    [Header("Virtaul Camera Left/Right")]
    public CinemachineVirtualCamera virtualCameraWhileInteracting_leftOffset;
    public CinemachineVirtualCamera virtualCameraWhileInteracting_rightOffset;
    [Header("Virtual Camera Forward/Backward")]
    public CinemachineVirtualCamera virtualCameraWhileInteracting_forward;
    public CinemachineVirtualCamera virtualCameraWhileInteracting_backward;

    public Transform cameraTracker;

    private bool isInteractingWithObject;
    private PlayerMovement playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindFirstObjectByType<PlayerMovement>();
    }
    private void Update()
    {
        transform.position = playerTransform.transform.position;
        HandleCinemachineCamera();
    }

    private void HandleCinemachineCamera()
    {
        if (isInteractingWithObject)
        {
            if (playerTransform.IsStandStill)
            {
                virtualCameraWhileNotInteracting.gameObject.SetActive(true);
                virtualCameraWhileInteracting_leftOffset.gameObject.SetActive(false);
                virtualCameraWhileInteracting_rightOffset.gameObject.SetActive(false);
                virtualCameraWhileInteracting_forward.gameObject.SetActive(false);
                virtualCameraWhileInteracting_backward.gameObject.SetActive(false);
            }
            else if (playerTransform.IsMovingToleft)
            {
                virtualCameraWhileNotInteracting.gameObject.SetActive(false);
                virtualCameraWhileInteracting_leftOffset.gameObject.SetActive(true);
                virtualCameraWhileInteracting_rightOffset.gameObject.SetActive(false);
                virtualCameraWhileInteracting_forward.gameObject.SetActive(false);
                virtualCameraWhileInteracting_backward.gameObject.SetActive(false);
            }
            else if (playerTransform.IsMovingToRight)
            {
                virtualCameraWhileNotInteracting.gameObject.SetActive(false);
                virtualCameraWhileInteracting_leftOffset.gameObject.SetActive(false);
                virtualCameraWhileInteracting_rightOffset.gameObject.SetActive(true);
                virtualCameraWhileInteracting_forward.gameObject.SetActive(false);
                virtualCameraWhileInteracting_backward.gameObject.SetActive(false);
            }
            else if (playerTransform.IsMovingForward)
            {
                virtualCameraWhileNotInteracting.gameObject.SetActive(false);
                virtualCameraWhileInteracting_leftOffset.gameObject.SetActive(false);
                virtualCameraWhileInteracting_rightOffset.gameObject.SetActive(false);
                virtualCameraWhileInteracting_forward.gameObject.SetActive(true);
                virtualCameraWhileInteracting_backward.gameObject.SetActive(false);
            }
            else if (playerTransform.IsMovingBackward) 
            {
                virtualCameraWhileNotInteracting.gameObject.SetActive(false);
                virtualCameraWhileInteracting_leftOffset.gameObject.SetActive(false);
                virtualCameraWhileInteracting_rightOffset.gameObject.SetActive(false);
                virtualCameraWhileInteracting_forward.gameObject.SetActive(false);
                virtualCameraWhileInteracting_backward.gameObject.SetActive(true);
            }
        }
        else
        {
            virtualCameraWhileNotInteracting.gameObject.SetActive(true);
            virtualCameraWhileInteracting_leftOffset.gameObject.SetActive(false);
            virtualCameraWhileInteracting_rightOffset.gameObject.SetActive(false);
        }
    }

    public void StartPushPullObject()
    {
        isInteractingWithObject = true;
    }

    public void StopPushPullObject()
    {
        isInteractingWithObject = false;
    }
}
