using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class IceMelting : MonoBehaviour,GameManager.IInteractable
{
    [SerializeField] private float opacityLoss;  
    internal bool isMelting;
   private float _curOpacity; 
   [SerializeField] internal int MeltingStage;
    private int melted;
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

    [SerializeField] Animator TutorialDoorAnimator;
    [SerializeField] NavMeshAgent monster;
    [SerializeField] Transform DoorPos;
    [SerializeField] Animator IceBlock;
    [SerializeField] Animator ElevatorDoor;
    [SerializeField] GameObject iceCollider;
    [SerializeField] GameObject Monster;
   void Start()
   {
       
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
        else
        {
            CheckMeltingProgress();
        }
    }
    

    private void CheckMeltingProgress()
    {
     
        if (MeltingStage == melted)
        {
            checkPoint1 = true;
            print("Final checkpoint");
             _renderer.enabled = false;
             StartCoroutine(Delay());
            print(_DoorHitbox.name);
            StartCoroutine(CutSceneDelay());
            
            if (_DoorHitbox.gameObject.name == "CubeTut")
            {
                StartCoroutine(TutDoorOpen());
                Monster.SetActive(true);
            }
            else
            {
                ElevatorDoor.SetBool("OpenDoor",true);
                iceCollider.SetActive(false);
            }
            
        } 
        else if(MeltingStage >= melted && !GameManager.TutorialActive)
        { 
            PlayerPrefs.SetInt("MeltingStage", MeltingStage);
        }
     
    }

    private IEnumerator CutSceneDelay()
    {
        
        exitCollider.enabled = true;
        yield return null;  
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        _DoorHitbox.SetActive(false);
    }

    private IEnumerator CheckIsMelting()
    {
        yield return new WaitUntil(() => isMelting == false || torchActive== false );
        active = false;
        quickTimeEvent.StopQTE();
    }

    public void lowerMeltingStage()
    {
        if (_DoorHitbox.gameObject.name == "CubeTut")
        {
            MeltingStage--;
        }
        else
        {
            MeltingStage--;
            monster.SetDestination(DoorPos.position);
        }
            

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
    IEnumerator TutDoorOpen()
    {
        GameManager.TutStop();
        _playerInteraction.iceMelting = GameObject.Find("MainIce").GetComponent<IceMelting>();
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Renderer>().enabled = false;
        TutorialDoorAnimator.Play("Tutorial Door First open");
        iceCollider.SetActive(false);
        yield return new WaitForSeconds(4);
        FrostSystem.isFreezing = true;
        yield return null;

    }
}
