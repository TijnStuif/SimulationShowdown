using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class MovingTile
    {
        public GameObject tile;
        public int moveDirection;

        public MovingTile(GameObject tile, int value)
        {
            this.tile = tile;
            this.moveDirection = 1;
        }
}

public class MovingGround : MonoBehaviour
{

    private List<MovingTile> movingFloorTiles = new List<MovingTile>();
    private MovingTile movingFloor;
    private List<MovingTile> floorTilesList = new List<MovingTile>();
    private List<MovingTile> toRemove = new List<MovingTile>();
    private const float SPEED = 0.02f; 
    private const float UPPER_BOUNDARY = 2.45f;
    private const float LOWER_BOUNDARY = 0f;
    private const int MAX_AMOUNT_MOVING_FLOORS = 10;


    void Start()
    {

        GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("SimulationBorder");

        

        foreach(GameObject tiles in floorTiles)
        {
            floorTilesList.Add(new MovingTile(tiles, 1));
        }

        StartCoroutine(SelectingFloorTile());

    }

    private bool IsListCleared()
    {
        foreach (var tileToRemove in toRemove)
        {
            if (movingFloorTiles.Contains(tileToRemove))
                return false;
        }
        return true;
    }

    private IEnumerator SelectingFloorTile()
    {
        
        if(movingFloorTiles.Count <= MAX_AMOUNT_MOVING_FLOORS)
        {
            movingFloor = floorTilesList[Random.Range(0, floorTilesList.Count)];
            movingFloorTiles.Add(movingFloor);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(SelectingFloorTile());
    }

    private void FixedUpdate()
    {
        if(IsListCleared())
        {
            toRemove.Clear();
            for (int i = 0; i < movingFloorTiles.Count; i++)
            {
                var currentTile = movingFloorTiles[i];

                if(currentTile.tile.transform.position.y >= UPPER_BOUNDARY)
                {
                    movingFloorTiles[i].moveDirection = -1;
                } 

                if(currentTile.tile.transform.position.y <= LOWER_BOUNDARY)
                {
                    movingFloorTiles[i].moveDirection = 1;
                }
                
                if(currentTile.tile.transform.position.y < LOWER_BOUNDARY)
                {
                    toRemove.Add(currentTile);
                }
                
                currentTile.tile.transform.position += new Vector3(0, SPEED * movingFloorTiles[i].moveDirection, 0);
            }
        }
        

        foreach(var tileToRemove in toRemove)
        {
            movingFloorTiles.Remove(tileToRemove);
        }
    }
}
