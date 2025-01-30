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
       _playerHotbar = _player.GetComponent<PlayerHotbar>();
   }





   public void Interact()
   {

       print("Pick up Item");
       if (this != null)
       {
           if (GetComponent<GameManager.IInteractable>() == null) return;

           var item = GetComponent<GroundObj>();
           var equipment = item.equipment;
           if (item)
           {
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
               /*else
               {
                   if (_player.Inventory.EmptySlotCount > 0)
                   {
                       if (_player.Inventory.AddItem(_item, 1))
                       {
                           _player.GetComponent<PlayerInteraction>().InteractText.text = "";
                           Destroy(gameObject);
                           if (equipment != null)
                           {
                               _player._equipmentBases.Add(equipment);
                           }
                       }
                   }
               }*/

           }

       }
   }

   public void HeldInteract()
   {
       
   }
}

