using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
   private int BossHp; 
   public GameObject Shadow;
   public GameObject FlyingCube;
   private bool Hit = false;

    void Start()
    {
        BossHp = 100;
    }
    
   void OnCollisionEnter(Collision collision)
   {
    if (collision.gameObject.CompareTag("Ground"))
    {
        Destroy(gameObject);
    }
    if (collision.gameObject.CompareTag("Boss"))
    {
        Destroy(gameObject);
        Hit = true;
    }
   }
   void Update()
    {
        if(Hit == true)
        {
            BossHp -= 10;
            Debug.Log("Current BossHP = " + BossHp);
            Debug.Log(Hit);
            //Hit = false;
        }
    }
}
