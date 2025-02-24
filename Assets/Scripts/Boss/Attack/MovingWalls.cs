using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingWalls : MonoBehaviour
{
    private HashSet<GameObject> movingWalls = new HashSet<GameObject>();
    private GameObject movingWall;
    private Vector3 moveDirectionWall = new Vector3(0, 0, 0.1f);
    private Vector3 moveToOriginalPosition = new Vector3(0, 0, 30);
    private PhaseController phaseController;
    private List<GameObject> walls = new List<GameObject>();
    public float wallMovementSpeed = 0.01f;
    private Outline outline;

    
    void Start()
    {
        walls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wall"));

        StartCoroutine(SelectingWalls());
    }

    private IEnumerator SelectingWalls()
    {
        movingWall = walls[Random.Range(0, walls.Count)];
        movingWalls.Add(movingWall);

        yield return new WaitForSeconds(1f);

        StartCoroutine(SelectingWalls());
    }

    private void FixedUpdate()
    {
        foreach (var movingWall in movingWalls)
        {
            movingWall.GetComponent<Outline>().enabled = true;
            
            if(movingWall.transform.position.z >= -15)
            {
                movingWall.transform.position -= moveDirectionWall;
            } 
            else 
            {
                movingWall.transform.position += moveToOriginalPosition;
                movingWall.GetComponent<Outline>().enabled = false;
                movingWalls.Remove(movingWall);
                break;
            }
        }
        
    }
}
