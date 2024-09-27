using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody playerBody;
    
    private float movementX;
    private float movementY; 
    public float speed = 0;   
    public GameObject shadowGround;
    public GameObject shadowAttack;
    public GameObject FlyingCube;
    public GameObject player;
    private Vector3 shadowPositionAdjuster = new Vector3(-1f, -0.15f, -1f);
    private Vector3 shadowAttackPositionAdjuster = new Vector3(-1f, 3f, 0.85f);
    private Vector3 CubePositionAdjuster = new Vector3(-1f, 4f, 0.85f);
   

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

        // if(Input.GetKey(KeyCode.W))
        // {
        //     shadowPositionAdjuster = new Vector3(0f, -0.15f, -1f);
        // }
        // if(Input.GetKey(KeyCode.A))
        // {
        //     shadowPositionAdjuster = new Vector3(-1f, -0.15f, -2f);
        // }
        // if(Input.GetKey(KeyCode.S))
        // {
        //     shadowPositionAdjuster = new Vector3(-2f, -0.15f, -1f);
        // }
        // if(Input.GetKey(KeyCode.D))
        // {
        //     shadowPositionAdjuster = new Vector3(-1f, -0.15f, 0f);
        // }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        playerBody.AddForce(movement * speed);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            Instantiate(shadowGround, player.transform.position + player.transform.localScale + shadowPositionAdjuster, player.transform.rotation);
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(FlyingCube, player.transform.position + player.transform.localScale + CubePositionAdjuster, FlyingCube.transform.rotation);
            Instantiate(shadowAttack, player.transform.position + player.transform.localScale + shadowAttackPositionAdjuster, shadowAttack.transform.rotation);
        }
    }
}
