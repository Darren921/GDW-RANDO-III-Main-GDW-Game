using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
 
    private static readonly int Opacity = Shader.PropertyToID("_Opacity");
    
    [Header("Frost")]
    bool isFreezing;
    [SerializeField]private float _frost;
    [SerializeField] private float maxFrost;
    [SerializeField] private Material frostTexture;
    private float _curOpacity;
    private bool TorchCheck;
    public static bool firstLoad;

    
    [Header("References")]
    [SerializeField] private Player _player;
    PlayerHotbar _playerHotbar;

    private void Start()
    {
        _playerHotbar = _player.GetComponent<PlayerHotbar>();
        _curOpacity = -0.7f;
     
        //use this to toggle off and on freezing in areas, maybe tut area?
         isFreezing = true;
        _frost = 0;

    }
    private void Update()
    {
        TorchCheck = _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().torchActive;
         if(_player is null) return;
         
         switch (isFreezing)
         {
             case false:
                 print("not freezing");
                 return;
             case true when TorchCheck && !_player.dead:
             {
                 _frost -= Time.deltaTime;
                 _curOpacity -= 0.015f * Time.deltaTime;
                 frostTexture.SetFloat(Opacity,_curOpacity ); 
                 if (_frost <= 0)
                 {
                     _frost = 0;
                 }

                 break;
             }
             case true when ! TorchCheck  && (!_player.dead):
             {
                 {
//                     print(_player._equipmentBases[_player.returnTorchLocation()].torchActive);
                     _frost += Time.deltaTime;
                     _curOpacity += 0.01f * Time.deltaTime;

                     frostTexture.SetFloat(Opacity, _curOpacity);
                     if (_frost >= maxFrost)
                     {
                         frostTexture.SetFloat(Opacity, -0.7f);
                         _player.dead = true;
                         
                         if (_player.dead)
                         {
                             StartCoroutine(_player.LookatDeath());

                         }
                     }
                     break;
                 }
             }
         }
        
    }
    void OnApplicationQuit()
    {
        if (frostTexture is not null)
        {
            frostTexture.SetFloat(Opacity,-1 ); 
            _frost = 0;
        }
     
    }
    public interface IInteractable
    {
        public void Interact();
        public void HeldInteract();
    }
   
}