using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
   internal int tracker;
   GameManager gm;
   private void Start()
   {
      if (this != null)
      {
         gm = FindObjectOfType<GameManager>();
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Player")
      {
         if (gm.trackedIndexs.Contains(tracker))
         {
            gm.SpawnedList.Remove(tracker);
            gm.trackedIndexs.Remove(tracker);
            gm.fuelInScene--;
         }
         Destroy(gameObject,0.1f);
      }
   }

   void Update()
   {
      if (Input.GetKeyDown(KeyCode.Space))
      {
       
      }
   }
}
