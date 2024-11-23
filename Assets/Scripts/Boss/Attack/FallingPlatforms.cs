using System.Collections;
using System.Collections.Generic;
using Boss.Attack;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour, IAttack
{
    public Type Type => Type.Environment;
    private List<GameObject> tiles;
    private List<GameObject> tilesToRemove = new List<GameObject>();
    private List<GameObject> targetedTiles = new List<GameObject>();
    
    private GameObject targetedTile;
    public Material indicatorMaterial;
    [SerializeField] private int numberOfTiles = 3;
    private bool attackCompleted = true;

    void Start()
    {
        //Puts and holds all the tiles in a list
        tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("SimulationBorder"));
    }

    public void Execute()
    {
        //Check if the previous attack is completed
        //This avoids an enumeration exception en possible bugs
        if (attackCompleted)
        {
            attackCompleted = false;

            //Clear the list of targeted tiles to make sure the same tiles are not selected twice
            targetedTiles.Clear();

            //Selects random tiles to be removed
            SelectTiles(numberOfTiles);
            
            //Start the attack sequence
            StartCoroutine(Attack());

        }
        
    }

    private void SelectTiles(int numberOfTiles)
    {    
        //Selects an amount of tiles
        for (int i = 0; i < numberOfTiles; i++)
        {
            //Makes sure there are still some tiles left
            if (tiles.Count <= 10) break;

            //Selects random tiles and adds them to a list
            targetedTile = tiles[Random.Range(0, tiles.Count)]; 
            targetedTiles.Add(targetedTile);  

            //Removes the targetedtiles from the tiles list to make sure the same tiles arent selected twice
            tiles.Remove(targetedTile);
        }
    }

    private IEnumerator Attack()
    {
        //Loops through each selected tile in the list
        foreach (var targetedTile in targetedTiles)
        {
            //changes the color to red to indicate the tile is going to be removed
            targetedTile.GetComponent<MeshRenderer>().material = indicatorMaterial;

            //Lets the routine pause for a second to create some sort of animation of different tiles being selected
            yield return new WaitForSeconds(1f);
        }

        //Makes sure the player has a few seconds to get of the selected tiles
        yield return new WaitForSeconds(2f);

        //Loops through the selected tiles and removes them
        foreach (var targetedTile in targetedTiles)
        {
            targetedTile.SetActive(false);
            tilesToRemove.Add(targetedTile);
        }

        //Attack is completed
        attackCompleted = true;
    }
}


