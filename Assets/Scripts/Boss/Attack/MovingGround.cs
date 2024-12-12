using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MovingGround : MonoBehaviour
{

    private HashSet<GameObject> movingFloorTiles = new HashSet<GameObject>();
    private GameObject movingFloor;
    private List<GameObject> floorTiles = new List<GameObject>();
    [SerializeField] private Material indicatorMaterial;
    private Vector3 moveDirectionFloor = new Vector3(0, 0.05f, 0);

    void Start()
    {
        floorTiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("SimulationBorder"));

        Debug.Log(floorTiles);
        StartCoroutine(SelectingFloorTile());
    }

    private IEnumerator SelectingFloorTile()
    {
        Debug.Log("Coroutine starts");
        movingFloor = floorTiles[Random.Range(0, floorTiles.Count)];
        movingFloorTiles.Add(movingFloor);

        movingFloor.GetComponent<MeshRenderer>().material = indicatorMaterial;

        yield return new WaitForSeconds(6f);

        StartCoroutine(SelectingFloorTile());
    }


    private void FixedUpdate()
    {
        foreach (var movingFloor in movingFloorTiles)
        {
            
            if(movingFloor.transform.position.y < 24.5)
            {
                movingFloor.transform.position += moveDirectionFloor;
            } 
            // else 
            // {
            //     movingWall.transform.position += moveToOriginalPosition;
            //     movingWall.GetComponent<MeshRenderer>().material = boxMaterial;
            //     movingWalls.Remove(movingWall);
            //     break;
            // }
        }
        
    }

}
