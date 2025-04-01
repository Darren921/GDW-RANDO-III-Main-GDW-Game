using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class IceMelting : MonoBehaviour,GameManager.IInteractable
{
    [SerializeField] private float opacityLoss;  
    internal bool isMelting;
   private float _curOpacity; 
   [SerializeField] internal int MeltingStage;
    private float melted;
    private QuickTimeEvents quickTimeEvent;
   private bool active , checkPoint1, checkPoint2, checkPoint3, checkPoint4, checkPoint5;
   private Renderer _renderer;
   [SerializeField]private BoxCollider exitCollider;
   internal PlayerHotbar _playerHotbar;
   internal PlayerInteraction _playerInteraction;

   [SerializeField] GameObject _DoorHitbox;
   internal bool AtMeltingPoint;
   private bool torchActive;
   
   [SerializeField] internal TextMeshProUGUI IcemeltingText;
   private QuickTimeEvents quickTimeEvents;

   void Start()
   {
       if (GameManager.loaded && PlayerPrefs.HasKey("MeltingStage"))
       {
           MeltingStage = PlayerPrefs.GetInt("MeltingStage");
       }
       quickTimeEvent = FindObjectOfType<QuickTimeEvents>();
       isHeld = true;
       QTEAble = true;
       exitCollider.enabled = false;
       //Time.timeScale = 10;
        _renderer = GetComponent<Renderer>();
        _playerHotbar = FindObjectOfType<PlayerHotbar>();
        _playerInteraction = _playerHotbar. GetComponent<PlayerInteraction>();

      //  meltingProgress = 400;
        _curOpacity = 1;
    }
 
    private void OnTriggerStay(Collider other)
    {
//        print(isMelting);
  //      print(other.gameObject.tag);
     if (other.CompareTag("Torch"))
    {
      AtMeltingPoint = true;
//      print(AtMeltingPoint);
//      print(other.tag);
    }
     torchActive = _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().torchActive;
//     print(torchActive);
        if  (torchActive && AtMeltingPoint && _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().equipped)
        {
           // print(other.gameObject.tag);
            isMelting = true;
            
        }
        else
        {
            isMelting = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isMelting = false;
        AtMeltingPoint = false;
        IcemeltingText.text = "";
        _playerInteraction.ResetPlayerInteraction();
    }

    void Update()
    {
//        print(_player._equipmentBases[_player.returnTorchLocation()].torchActive);
        if (isMelting && !active)
        {
            active = true;
            StartCoroutine(CheckIsMelting());
        }
    }
    

    private void CheckMeltingProgress()
    {
     
        if (MeltingStage <= melted)
        {
            checkPoint1 = true;
            print("Final checkpoint");
             _renderer.enabled = false;
            _DoorHitbox.SetActive(false);
            exitCollider.enabled = true;
            if (_DoorHitbox.gameObject.name == "Door1")
            {
                GameManager.TutorialActive = false;
                FrostSystem.FrostOnOff();
            }
        } 
        else if(MeltingStage >= melted && !GameManager.TutorialActive)
        { 
            PlayerPrefs.SetInt("MeltingStage", MeltingStage);
        }
     
    }

    private IEnumerator CheckIsMelting()
    {
        yield return new WaitUntil(() => isMelting == false || torchActive== false );
        active = false;
        quickTimeEvent.StopQTE();
    }

    public void lowerMeltingStage()
    {
        MeltingStage--;
    }

    public bool QTEAble { get; set; }
    public bool isHeld { get; set; }

    public void Interact()
    {
       
    }

    public void HeldInteract()
    {
       
        if (!(_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses > 0) ||
            !(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1f)) return;
        print("in 1 ");
        if (!isMelting || !_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].equipped) return;
        print("in 2 ");
        if (_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration >= 4.9)
        {
            print("Held Interact");
           
            if ( !quickTimeEvent.interacted)
            {
                print("stoping QTE Normal");
              _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().ReduceCount(1);
              lowerMeltingStage();
            }
            CheckMeltingProgress();
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration = 0; 
            quickTimeEvent.StopQTE();
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().ResetPlayerInteraction();
            IcemeltingText.text = "";

        }
        else if(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 4.9)
        {
            print("in oh no ");
            print(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration);
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().ResetPlayerInteraction();
            quickTimeEvent.StopQTE();
            quickTimeEvent.qteResult.text = "";
        }

    }
}
