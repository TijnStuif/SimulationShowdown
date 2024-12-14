using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MovingGround : MonoBehaviour
{

    private List<(Transform, State)> movingFloorTiles = new List<(Transform, State)>();
    private (Transform, State) movingFloor;
    private List<(Transform, State)> floorTiles = new List<(Transform, State)>();
    private List<GameObject> UpTiles = new List<GameObject>();
    private List<GameObject> DownTiles = new List<GameObject>();
    private List<(Transform, State)> floorTilesToRemove = new List<(Transform, State)>(); 
    [SerializeField] private Material indicatorMaterial;
   
   private enum State
   {
    Down,
    Middle,
    Up
   }


    void Start()
    {
        floorTiles = new List<(Transform, State)>();

        var tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("SimulationBorder"));

        foreach(var tile in tiles)
        {
            var tuple = (tile.transform, State.Down);
            floorTiles.Add(tuple);
        }

        StartCoroutine(SelectingFloorTile());

    }

    private IEnumerator SelectingFloorTile()
    {
        Debug.Log("Coroutine starts");
        movingFloor = floorTiles[Random.Range(0, floorTiles.Count)];
        movingFloorTiles.Add(movingFloor);

        movingFloorTiles[i].Item1.gameObject.GetComponent<MeshRenderer>().material = indicatorMaterial;

        yield return new WaitForSeconds(6f);

        StartCoroutine(SelectingFloorTile());
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < movingFloorTiles.Count; i++)
        {
            floorTilesToRemove.Add(movingFloorTiles[i]);

            if(movingFloorTiles[i].Item2 == State.Down)
            {
               movingFloorTiles[i].Item1.transform.position = Vector3.MoveTowards(movingFloorTiles[i].Item1.position, new Vector3(movingFloorTiles[i].Item1.position.x, 2.45f, movingFloorTiles[i].Item1.position.z), 1f * Time.deltaTime); 

               if(movingFloorTiles[i].Item1.position.y == 2.45f)
               {
                    
                    movingFloorTiles[i].Item2 = State.Middle;
               }
            } 

            if(movingFloorTiles[i].Item2 == State.Middle)
            {
               movingFloorTiles[i].Item1.transform.position = Vector3.MoveTowards(movingFloorTiles[i].Item1.position, new Vector3(movingFloorTiles[i].Item1.position.x, 2.45f, movingFloorTiles[i].Item1.position.z), 1f * Time.deltaTime); 
            } 

            if(movingFloorTiles[i].Item2 == State.Up)
            {
               movingFloorTiles[i].Item1.transform.position = Vector3.MoveTowards(movingFloorTiles[i].Item1.position, new Vector3(movingFloorTiles[i].Item1.position.x, 2.45f, movingFloorTiles[i].Item1.position.z), 1f * Time.deltaTime); 
            } 
        }
        
        foreach (var tileToRemove in floorTilesToRemove)
        {
            movingFloorTiles.Remove(tileToRemove);
        }
        floorTilesToRemove.Clear();
    }


}
