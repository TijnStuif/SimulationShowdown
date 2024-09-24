using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody playerBody;
    
    private float movementX;
    private float movementY; 
    public float speed = 0;   
    public GameObject shadow;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;

        movementY = movementVector.y;

        if(Input.GetKey(KeyCode.W))
        {
            
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        playerBody.AddForce(movement * speed);
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Instantiate(shadow, player.transform.position + player.transform.localScale, player.transform.rotation);
        }
    }
}
