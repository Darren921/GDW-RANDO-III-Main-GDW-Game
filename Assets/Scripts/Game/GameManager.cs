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
    
    
    [Header("References")]
    [SerializeField] private Player _player;

    private void Start()
    {
        _curOpacity = -1;
     
        //use this to toggle off and on freezing in areas, maybe tut area?
         isFreezing = true;
        _frost = 0;
      
    }
    private void Update()
    {
       
         if(_player is null) return;
         switch (isFreezing)
         {
             case false:
                 return;
             case true when _player.returnTorchState():
             {
                 _frost -= Time.deltaTime;
                 _curOpacity -= 0.01f * Time.deltaTime;
                 frostTexture.SetFloat(Opacity,_curOpacity ); 
                 if (_frost <= 0)
                 {
                     _frost = 0;
                 }

                 break;
             }
             case true when !_player.returnTorchState():
             {
                 _frost += Time.deltaTime;
                 _curOpacity += 0.01f * Time.deltaTime;
                 frostTexture.SetFloat(Opacity,_curOpacity ); 
                 if (_frost >= maxFrost)
                 {
                     _frost = 0;
                     frostTexture.SetFloat(Opacity,0 ); 

                     StartCoroutine(_player.LookatDeath());
                 }

                 break;
             }
         }

        
    }
    void OnApplicationQuit()
    {
        if(frostTexture is null) return;
        frostTexture.SetFloat(Opacity,-1 ); 
        _frost = 0;
    }

   
}