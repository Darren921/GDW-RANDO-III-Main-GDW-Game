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
         switch (tag)
         {
            case "Fuel":
               if (gm.trackedIndexs.Contains(tracker))
               {
                  gm.SpawnedList.Remove(tracker);
                  gm.trackedIndexs.Remove(tracker);
                  gm.fuelInScene--;
               }
               break;
            
            case "Batteries":
               if (gm.trackedIndexs.Contains(tracker))
               {
                  gm.SpawnedList.Remove(tracker);
                  gm.trackedIndexs.Remove(tracker);
                  gm.batteriesInScene--;
               }
               break;
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
