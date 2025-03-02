using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundObj : MonoBehaviour,GameManager.IInteractable 
{
   [SerializeField]internal int tracker;
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
           if (item && !item.CompareTag("Batteries") && !item.CompareTag("Fuel") && item.item.data.Id == 3 && item.item.data.Id == 4   )
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
               if (item.item.data.Id == _playerHotbar.Hotbar.database.ItemObjects[3].data.Id && _playerHotbar.batteryCount < item.item.data.Limit)
               {
                  // if (!_spawnManager.trackedIndexs.Contains((tracker)) || !_spawnManager.trackedIndexs.Contains(tracker) && item.name == "battery") return;
                   if (_playerHotbar.batteryCount <= 0 &&  _playerHotbar._equipmentBases[1].CurrentUses <= 0)
                   {
                       print("in here battery");
                       _playerHotbar._equipmentBases[1].CurrentUses = _playerHotbar._equipmentBases[1].MaxUses;
                       Destroy(gameObject,0.1f);
                       _player.GetComponent<PlayerInteraction>().Reset();
                       _player.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();  
                       _playerHotbar.GetComponent<PlayerInteraction>().InteractText.text = "";
                       if (CompareTag("Batteries"))
                       {
                           _spawnManager.AvaiableSpawns.Add(tracker);
                           _spawnManager.TrackedIndex.Remove(tracker);
                       }
                       else
                       {
                           print("Spec object");
                       }
                       return;
                   }
             
                   _playerHotbar.batteryCount++;
                   if (CompareTag("Batteries"))
                   {
                       _spawnManager.AvaiableSpawns.Add(tracker);
                       _spawnManager.TrackedIndex.Remove(tracker);
                   }
                   else
                   {
                       print("Spec object");
                   }
                   Destroy(gameObject,0.1f);
                   _player.GetComponent<PlayerInteraction>().Reset();
                   _player.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();  
                   _playerHotbar.GetComponent<PlayerInteraction>().InteractText.text = "";

               
               }
            
               
               if (item.item.data.Id == _playerHotbar.Hotbar.database.ItemObjects[4].data.Id  && _playerHotbar.FuelCount < item.item.data.Limit )
               {
                   print("in here fuel");
                 // if (!_spawnManager.trackedIndexs.Contains((tracker)) || (!_spawnManager.trackedIndexs.Contains(tracker))&& item.name == "Gas canister obj") return;
                   _playerHotbar.FuelCount++;
                   _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses = _playerHotbar.FuelCount;
                   print( _playerHotbar.FuelCount);
                   print(  _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses );
                   if (CompareTag("Fuel"))
                   {
                       _spawnManager.AvaiableSpawns.Add(tracker);
                       _spawnManager.TrackedIndex.Remove(tracker);
                   }
                   else
                   {
                       print("Spec object");
                   }
                   
                   Destroy(gameObject,0.1f);
                   _player.GetComponent<PlayerInteraction>().Reset();
                   _player.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();
                   _playerHotbar.GetComponent<PlayerInteraction>().InteractText.text = "";
                  
                 
               }
             
           }

       }
   }

   private void OnDestroy()
   {
       if(_playerHotbar == null) return;
       if(_playerHotbar.GetComponent<PlayerInteraction>().InteractText != null) _playerHotbar.GetComponent<PlayerInteraction>().InteractText.text = "";
       _player.GetComponent<PlayerInteraction>().Reset();
       _player.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();
       _player.GetComponentInChildren<FrostSystem>().DeFrost.gameObject.SetActive(true);
   }
   

   public void HeldInteract()
   {
       
   }
}

