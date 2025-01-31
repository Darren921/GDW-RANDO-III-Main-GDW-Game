using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObj : MonoBehaviour,GameManager.IInteractable 
{
   internal int tracker;
   private SpawnManager _spawnManager;
   private Player _player;
   private PlayerHotbar _playerHotbar;
   public ItemObj item;
   public EquipmentBase equipment;

   private void Start()
   {
       _player = FindFirstObjectByType<Player>();
       _spawnManager = FindFirstObjectByType<SpawnManager>();
       _playerHotbar = _player.GetComponent<PlayerHotbar>();
       isHeld = false;
   }


   public bool isHeld { get; set; }

   public void Interact()
   {

       
       print("Pick up Item");
       if (this != null)
       {
           
           if (GetComponent<GameManager.IInteractable>() == null) return;

           var item = GetComponent<GroundObj>();
           var equipment = item.equipment;
           if (item && !item.CompareTag("Batteries") && !item.CompareTag("Fuel") )
           {
               print(item.tag);
               Item _item = new Item(item.item);
               if (_playerHotbar.Hotbar.EmptySlotCount > 0)
               {
                   if (_playerHotbar.Hotbar.AddItem(_item, 1))
                   {
                       _player.GetComponent<PlayerInteraction>().InteractText.text = "";

                       Destroy(gameObject);
                       if (equipment != null)
                       {
                           _playerHotbar._equipmentBases.Add(equipment);
                       }
                   }
               }
           }
           else
           {
               if (item.CompareTag("Batteries") && _playerHotbar.batteryCount < item.item.data.Limit )
               {
                   if (!_spawnManager.trackedIndexs.Contains((tracker))) return;
                   if (_playerHotbar.batteryCount <= 0)
                   {
                       _playerHotbar._equipmentBases[1].CurrentUses =
                           _playerHotbar._equipmentBases[1].MaxUses;
                       _spawnManager.SpawnedList.Remove(tracker);
                       _spawnManager.trackedIndexs.Remove(tracker);
                       Destroy(gameObject,0.1f);
                       return;
                   }
                   _playerHotbar.batteryCount++;
                   _spawnManager.SpawnedList.Remove(tracker);
                   _spawnManager.trackedIndexs.Remove(tracker);
                   Destroy(gameObject,0.1f);
               }
               else if (item.CompareTag("Fuel") && _playerHotbar.FuelCount < item.item.data.Limit )
               {
                   if (!_spawnManager.trackedIndexs.Contains((tracker))) return;
                   _playerHotbar.FuelCount++;
                   _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses = _playerHotbar.FuelCount;

                   _spawnManager.SpawnedList.Remove(tracker);
                   _spawnManager.trackedIndexs.Remove(tracker);
                   Destroy(gameObject,0.1f);
               }
           }

       }
   }

   public void HeldInteract()
   {
       
   }
}

