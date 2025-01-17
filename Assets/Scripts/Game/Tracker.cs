using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
   internal int tracker;
   private SpawnManager _spawnManager;
   private Player _player;
   private void Start()
   {
      if (this != null)
      { 
         _spawnManager = FindObjectOfType<SpawnManager>();
         _player = FindObjectOfType<Player>();
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
        
   }

   void Update()
   {
     
   }
}
