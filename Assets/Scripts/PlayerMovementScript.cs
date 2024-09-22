using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 700f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float rotateHorizontal = Input.GetAxis("Mouse X");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 rotation = new Vector3(0.0f, rotateHorizontal, 0.0f);

        MovePlayer(movement);
        RotatePlayer(rotation);
    }

    void MovePlayer(Vector3 movement)
    {
        Vector3 moveDirection = transform.forward * movement.z + transform.right * movement.x;
        rb.velocity = moveDirection * speed;
    }

    void RotatePlayer(Vector3 rotation)
    {
        Quaternion deltaRotation = Quaternion.Euler(rotation * rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}