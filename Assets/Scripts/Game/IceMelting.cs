using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class IceMelting : MonoBehaviour,GameManager.IInteractable
{
    [SerializeField] private float opacityLoss;  
    internal bool isMelting;
   private float _curOpacity; 
   [SerializeField] internal int MeltingStage;
    private float melted;

   private bool active , checkPoint1, checkPoint2, checkPoint3, checkPoint4, checkPoint5;
   private Renderer _renderer;
   private BoxCollider _boxCollider;
   [SerializeField]private BoxCollider exitCollider;
   internal PlayerHotbar _playerHotbar;
   [SerializeField] GameObject _DoorHitbox;
   internal bool AtMeltingPoint;
   private bool torchActive;
   
   [SerializeField] internal TextMeshProUGUI IcemeltingText;

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
             CheckMeltingProgress();
        } 
        
    }
    

    private void CheckMeltingProgress()
    {
        if (MeltingStage <= melted)
        {
            checkPoint1 = true;
            print("Final checkpoint");
             _renderer.enabled = false;
             _boxCollider.enabled = false;
            _DoorHitbox.SetActive(false);
            exitCollider.enabled = true;

        } 
        else switch (MeltingStage)
        {
            case 1:
                checkPoint2 = true;
                //  print("third last checkpoint");
                PlayerPrefs.SetFloat("MeltingStage", 1);
                break;
            case 2:
                checkPoint3 = true;
                //   print("second checkpoint");
                PlayerPrefs.SetFloat("MeltingStage", 2);
                break;
            case 3:
                checkPoint4 = true;
                // print("first checkpoint");
                PlayerPrefs.SetFloat("MeltingStage", 3);
                break;
            case 4:
                checkPoint5 = true;
                // print("first checkpoint");
                PlayerPrefs.SetFloat("MeltingStage", 4);
                break;
        }
     
    }

    private IEnumerator CheckIsMelting()
    {
        yield return new WaitUntil(() => isMelting == false || torchActive== false );
        active = false;
    }

    public bool isHeld { get; set; }

    public void Interact()
    {
       
    }

    public void HeldInteract()
    {
       
        if (!(_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses > 0) ||
            !(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 10.1f)) return;
        print("in 1 ");
        if (!isMelting || !_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].equipped) return;
        print("in 2 ");
        if (_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration >= 9.9)
        {
            print("Held Interact");
            _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().ReduceCount();
            MeltingStage--;
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().Reset();
            _playerHotbar.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();
            IcemeltingText.text = "";

        }
        else if(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 9.9)
        {
            print("in oh no ");
            print(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration);
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().Reset();
            _playerHotbar.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();

        }

    }
}
