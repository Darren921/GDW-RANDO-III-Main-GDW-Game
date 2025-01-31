using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class IceMelting : MonoBehaviour,GameManager.IInteractable
{
    [SerializeField] private float opacityLoss;  
    internal bool isMelting;
   private float curMeltingTime;
   private float _curOpacity; 
   [SerializeField]private float meltingTime; 
   [SerializeField] private float MeltingStage;
    private float melted;

   private bool active , checkPoint1, checkPoint2, checkPoint3, checkPoint4, checkPoint5;
   private Renderer _renderer;
   private BoxCollider _boxCollider;
   [SerializeField]private BoxCollider exitCollider;
   private PlayerHotbar _playerHotbar;
   private Material iceMat;
   [SerializeField] GameObject _DoorHitbox;
   internal bool AtMeltingPoint;
   private bool torchActive;
   

   void Start()
   {
       isHeld = true;
       exitCollider.enabled = false;
       //Time.timeScale = 10;
        _boxCollider = GetComponent<BoxCollider>();
        _renderer = GetComponent<Renderer>();
        _playerHotbar = FindObjectOfType<PlayerHotbar>();
      //  meltingProgress = 400;
        _curOpacity = 1;
        iceMat = GetComponent<Renderer>().material;
    }
 
    private void OnTriggerStay(Collider other)
    {
//        print(isMelting);
  //      print(other.gameObject.tag);
     if (other.CompareTag("Torch"))
    {
      AtMeltingPoint = true;

     }
  torchActive = _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().torchActive;
     print(torchActive);
        if  (torchActive && AtMeltingPoint && _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().equipped)
        {
           // print(other.gameObject.tag);
            isMelting = true;
        }
        else
        {
            isMelting = false;
            AtMeltingPoint = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isMelting = false;
    }

    void Update()
    {
//        print(_player._equipmentBases[_player.returnTorchLocation()].torchActive);
        if (isMelting && !active)
        {
            active = true;
            StartCoroutine(CheckIsMelting());
        }

        if (isMelting)
        {
            
            // timeToMelt -= Time.deltaTime;
            // //base = 0.0025 mod(action block) = 0.0050
            // _curOpacity -=  opacityLoss * Time.deltaTime;
            // iceMat.SetFloat("_Opacity", _curOpacity);
             CheckMeltingProgress();

        } 
        
    }
    

    private void CheckMeltingProgress()
    {
        if (MeltingStage <= melted)
        {
            checkPoint1 = true;
            print("Final checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 0);
             _renderer.enabled = false;
             _boxCollider.enabled = false;
            _DoorHitbox.SetActive(false);
            exitCollider.enabled = true;

        } 
        else if (MeltingStage <= 2)
        {
            checkPoint2 = true;
          //  print("third last checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 25);
        }

        else if (MeltingStage <= 4)
        {
            checkPoint3 = true;
         //   print("second checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 50);
        }

        else if (MeltingStage <= 6)
        {
            checkPoint4 = true;
           // print("first checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 75);
        }
        else if (MeltingStage <= 8)
        {
            checkPoint5 = true;
            // print("first checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 75);
        }
     
    }

    private IEnumerator CheckIsMelting()
    {
        print("melting active ");
        yield return new WaitUntil(() => isMelting == false || torchActive== false );
        active = false;
    }

    public bool isHeld { get; set; }

    public void Interact()
    {
       
    }

    public void HeldInteract()
    {
        if (_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses > 0)
        {
            print("in 1 ");
            if (isMelting && _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].equipped  )
            {
                print("in 2 ");
                if (_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration >= 9.9)
                {
                    print("Held Interact");
                    _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().reduceCount();
                    _playerHotbar.gameObject.GetComponent<PlayerInteraction>().GetComponent<PlayerInteraction>().Reset();
                }
                else
                {
                    print("in oh no ");
                    print(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration);
                    _playerHotbar.gameObject.GetComponent<PlayerInteraction>().GetComponent<PlayerInteraction>().Reset();

                }
            }
        }
     
    }
}
