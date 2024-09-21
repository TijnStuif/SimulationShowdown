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
    private Vector2 playerInput;
    private Vector3 movement;
    public float groundDrag;
    [SerializeField] private float speed;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    bool isGrounded;
    public float playerHeight;

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
        } else if (context.canceled)
        {
            playerInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
            player.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    private void SpeedControl()
    {
        Vector3 horizontalVelocity = new(player.velocity.x, 0, player.velocity.z);
        if (horizontalVelocity.magnitude > speed)
        {
            Vector3 cappedVelocity = horizontalVelocity.normalized * speed;
            player.velocity = new Vector3(cappedVelocity.x, player.velocity.y, cappedVelocity.z);
        }
    }
}
