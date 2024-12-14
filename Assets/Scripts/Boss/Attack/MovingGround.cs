using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MovingGround : MonoBehaviour
{

    private HashSet<GameObject> movingFloorTiles = new HashSet<GameObject>();
    private GameObject movingFloor;
    private List<GameObject> floorTiles = new List<GameObject>();
    private List<GameObject> UpTiles = new List<GameObject>();
    private List<GameObject> DownTiles = new List<GameObject>();
    [SerializeField] private Material indicatorMaterial;
    private int movementSwap = 0;
   


    void Start()
    {
        floorTiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("SimulationBorder"));

        StartCoroutine(SelectingFloorTile());

        movementSwap = 1;
    }

    private IEnumerator SelectingFloorTile()
    {
        Debug.Log("Coroutine starts");
        movingFloor = floorTiles[Random.Range(0, floorTiles.Count)];
        DownTiles.Add(movingFloor);
        floorTiles.Remove(movingFloor);

        

        yield return new WaitForSeconds(6f);

        StartCoroutine(SelectingFloorTile());
    }

    private void FixedUpdate()
    {
        if(movementSwap == 1)
        {
            StartCoroutine(MoveTileUp());
        }
        if(movementSwap == 2)
        {
            StartCoroutine(MoveTileDown());
        }
    }


    private IEnumerator MoveTileUp()
    {
        foreach (var movingFloor in DownTiles)
        {
            
            movingFloor.GetComponent<MeshRenderer>().material = indicatorMaterial;

            yield return new WaitForSeconds(1f);

            movingFloor.transform.position = Vector3.MoveTowards(movingFloor.transform.position, new Vector3(movingFloor.transform.position.x, 2.45f, movingFloor.transform.position.z), 1f * Time.deltaTime);
            

            yield return new WaitForSeconds(12f);

            UpTiles.Add(movingFloor);
            DownTiles.Remove(movingFloor);
            movementSwap = 2;
        }
    }

    private IEnumerator MoveTileDown()
    {
        foreach (var movingFloor in UpTiles)
        {
            movingFloor.transform.position = Vector3.MoveTowards(movingFloor.transform.position, new Vector3(movingFloor.transform.position.x, 0, movingFloor.transform.position.z), 1f * Time.deltaTime);
        
            yield return new WaitForSeconds(12f);

            DownTiles.Add(movingFloor);
            UpTiles.Remove(movingFloor);
            movementSwap = 1;
        }
    }

}
