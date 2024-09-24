using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
   public GameObject player;
   public GameObject gameOverText;
   public MeshRenderer meshRenderer;
   public PlayerInput playerInput;


   void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.CompareTag("LightSpot"))
      {
         //player.gameObject.SetActive(false);
         playerInput.enabled = false;
         meshRenderer.enabled = false;
         gameOverText.gameObject.SetActive(true);
         
      }
   }

   // Update is called once per frame
   void Update()
   {

   }
}
