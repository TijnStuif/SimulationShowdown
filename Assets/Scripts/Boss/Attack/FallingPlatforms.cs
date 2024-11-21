using System.Collections;
using System.Collections.Generic;
using Boss.Attack;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour, IAttack
{
    public Type Type => Type.Environment;
    private List<GameObject> tiles;
    private List<GameObject> tilesToRemove = new List<GameObject>();
    private GameObject targetedTile;
    public Material indicatorMaterial;

    void Start()
    {
        tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("SimulationBorder"));
        SelectTile();
    }
    public void Execute()
    {
        Debug.Log("tile attack");

        for(int i = 0; i < 3; i++)
        {
            AttackIndicator();
            Invoke(nameof(TileAttack), 3f);
        }
        

        
    }

    private void SelectTile()
    {    
        targetedTile = tiles[Random.Range(0, tiles.Count)];   
    }

    private void AttackIndicator()
    {
        targetedTile.GetComponent<MeshRenderer>().material = indicatorMaterial;
    }

    private void TileAttack()
    {
        tiles.Remove(targetedTile);
        tilesToRemove.Add(targetedTile);
        targetedTile.SetActive(false);
        SelectTile();
    }
}


