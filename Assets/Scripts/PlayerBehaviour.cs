using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody player;

    [Header("Player Movement")]
    private Vector2 playerInput = Vector2.zero;
    private Vector3 movement;
    public float groundDrag;
    [SerializeField] private float speed;
    [SerializeField] private Transform playerRotator;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    bool isGrounded;
    public float playerHeight;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.6f, groundMask);
        if (isGrounded)
        {
            player.drag = groundDrag;
        }
        else
        {
            player.drag = 0;
        }
        movement = new Vector3(playerInput.x, 0, playerInput.y);
        player.AddForce(speed * Time.fixedDeltaTime * movement.normalized, ForceMode.Impulse); 
        SpeedControl();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInput = context.ReadValue<Vector2>();
            RotationBasedInputs();
        } else if (context.canceled)
        {
            playerInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
            player.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    private void SpeedControl()
    {
        Vector3 horizontalVelocity = new(player.velocity.x, 0, player.velocity.z);
        if (horizontalVelocity.magnitude > (speed / 3))
        {
            Vector3 cappedVelocity = horizontalVelocity.normalized * (speed / 3);
            player.velocity = new Vector3(cappedVelocity.x, player.velocity.y, cappedVelocity.z);
        }
    }

    public void RotationBasedInputs()
    {
        if (playerInput == Vector2.zero)
        {
            return;
        }
        Vector3 rotatedInput = Quaternion.Euler(0, playerRotator.eulerAngles.y, 0) * new Vector3(playerInput.x, 0, playerInput.y);
        playerInput = new Vector2(rotatedInput.x, rotatedInput.z);
    }
}
