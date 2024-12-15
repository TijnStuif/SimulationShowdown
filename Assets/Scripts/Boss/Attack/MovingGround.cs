using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class MovingGround : MonoBehaviour
{

    private List<GameObject> movingFloorTiles = new List<GameObject>();
    private GameObject movingFloor;
    private List<GameObject> floorTiles = new List<GameObject>();
    [SerializeField] private Material indicatorMaterial;
    private int moveDirection = 1;
   
   private enum State
   {
    Down,
    Middle,
    Up
   }


    void Start()
    {

        floorTiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("SimulationBorder"));
        // floorTiles = new List<(Transform, State)>();

        // var tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("SimulationBorder"));

        // foreach(var tile in tiles)
        // {
        //     var tuple = (tile.transform, State.Down);
        //     floorTiles.Add(tuple);
        // }

        StartCoroutine(SelectingFloorTile());

    }

    private IEnumerator SelectingFloorTile()
    {
        movingFloor = floorTiles[Random.Range(0, floorTiles.Count)];
        movingFloorTiles.Add(movingFloor);
        

        movingFloor.GetComponent<MeshRenderer>().material = indicatorMaterial;

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(SelectingFloorTile());
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < movingFloorTiles.Count; i++)
        {
            var currentTile = movingFloorTiles[i];
            if(currentTile.transform.position.y >= 2.45)
            {
                moveDirection = -1;
            } 
            if(currentTile.transform.position.y <= 0f)
            {
                moveDirection = 1;
            }
            
            currentTile.transform.position += new Vector3(0, 0.01f * moveDirection, 0);
        }


        //StartCoroutine(MoveTiles());
    }
    
    
    // private IEnumerator MoveTiles()
    // {
    //     float speed = 1f;

    //     for (int i = 0; i < movingFloorTiles.Count; i++)
    //     {
    //         var currentTile = movingFloorTiles[i];
            
    //         if(currentTile.Item2 == State.Down)
    //         {
    //            currentTile.Item1.transform.position = Vector3.MoveTowards(currentTile.Item1.position, new Vector3(currentTile.Item1.position.x, 2.45f, currentTile.Item1.position.z), speed * Time.deltaTime); 

    //            if(currentTile.Item1.position.y >= 2.45f)
    //            {
    //                 currentTile.Item2 = State.Middle;
    //                 currentTile.Item1.position = new Vector3(currentTile.Item1.position.x, 2.45f, currentTile.Item1.position.z);
                    
    //                 floorTiles.Add(currentTile);
    //                 movingFloorTiles.RemoveAt(i);
    //                 i--;

    //                 break;
    //            }
    //         } 

    //         yield return new WaitForEndOfFrame();

    //         if(currentTile.Item2 == State.Middle)
    //         {
    //             currentTile.Item1.transform.position = Vector3.MoveTowards(currentTile.Item1.position, new Vector3(currentTile.Item1.position.x, 4.9f, currentTile.Item1.position.z), speed * Time.deltaTime);

    //             if(Mathf.Abs(currentTile.Item1.position.y - 4.9f) < 0.01f)
    //             {    
    //                 currentTile.Item2 = State.Up;
    //                 currentTile.Item1.position = new Vector3(currentTile.Item1.position.x, 4.9f, currentTile.Item1.position.z);

    //                 floorTiles.Add(currentTile);
    //                 movingFloorTiles.RemoveAt(i);
    //                 i--;
    //                 break;
    //             }
    //         } 

    //         yield return new WaitForEndOfFrame();

    //         if(currentTile.Item2 == State.Up)
    //         {
    //            currentTile.Item1.transform.position = Vector3.MoveTowards(currentTile.Item1.position, new Vector3(currentTile.Item1.position.x, 2.45f, currentTile.Item1.position.z), speed * Time.deltaTime); 
    //         } 

    //         yield return new WaitForEndOfFrame();
    //     }
    // }


}
