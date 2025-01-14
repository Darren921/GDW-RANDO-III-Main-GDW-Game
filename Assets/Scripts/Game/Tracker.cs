using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
   internal int tracker;
   private SpawnManager _spawnManager;
   private void Start()
   {
      if (this != null)
      { 
         _spawnManager = FindObjectOfType<SpawnManager>();
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag("Player")) return;
      switch (tag)
      {
         case "Fuel":
            if (_spawnManager.trackedIndexs.Contains(tracker))
            {
               _spawnManager.SpawnedList.Remove(tracker);
               _spawnManager.trackedIndexs.Remove(tracker);
            }
            break;
            
         case "Batteries":
            if (_spawnManager.trackedIndexs.Contains(tracker))
            {
               _spawnManager.SpawnedList.Remove(tracker);
               _spawnManager.trackedIndexs.Remove(tracker);
            }
            break;
      }
        
      Destroy(gameObject,0.1f);
   }

   void Update()
   {
     
   }
}
