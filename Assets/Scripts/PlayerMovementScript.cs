using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sneakSpeed = 2f;
    public float turnSpeed = 10f;
    public Transform cameraTransform;  

    public Vector2 moveInput;
    private Vector2 lookInput;

    public bool isSneaking;
    public float currentSpeed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        
        HandleLook();
    }

    private void FixedUpdate()
    {
        currentSpeed = isSneaking ? sneakSpeed : walkSpeed;
        HandleMovement();
    }

    public void OnMove(InputValue context)
    {
        moveInput = context.Get<Vector2>();
    }

    public void OnLook(InputValue context)
    {
        lookInput = context.Get<Vector2>();
    }

    public void OnSneak(InputValue context)
    {
        isSneaking = context.isPressed;
    }

    private void HandleMovement()
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 direction = (forward * moveInput.y + right * moveInput.x).normalized;
            rb.velocity = direction * currentSpeed + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void HandleLook()
    {
        if (lookInput.sqrMagnitude >= 0.01f)
        {
            float targetAngle = Mathf.Atan2(lookInput.x, lookInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}