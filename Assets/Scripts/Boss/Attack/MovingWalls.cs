using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingWalls : MonoBehaviour
{

    private List<GameObject> firstRowWalls;
    private List<GameObject> secondRowWalls;
    private List<GameObject> movingWalls = new List<GameObject>();
    private GameObject movingWall;
    private Vector3 moveDirectionWall = new Vector3(0, 0, 0.001f);

    
    void Start()
    {
        firstRowWalls = new List<GameObject>(GameObject.FindGameObjectsWithTag("FirstRowWall"));
        secondRowWalls = new List<GameObject>(GameObject.FindGameObjectsWithTag("SecondRowWall"));

        SelectWalls();
        Debug.Log(firstRowWalls);
    }

    
    private void SelectWalls()
    {
        for (int i = 0; i < 4; i++)
        {
            //Selects random tiles and adds them to a list
            movingWall = firstRowWalls[Random.Range(0, firstRowWalls.Count)]; 
            movingWalls.Add(movingWall);  

            movingWall = secondRowWalls[Random.Range(0, secondRowWalls.Count)]; 
            movingWalls.Add(movingWall);  

            Debug.Log(movingWalls);
        }
    }

    private void Update()
    {
        foreach (var movingWall in movingWalls)
        {
            movingWall.transform.position -= moveDirectionWall;
            Debug.Log(movingWall.transform.position);
        }
    }
}
