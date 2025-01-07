using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IceMelting : MonoBehaviour
{
    [SerializeField] private float opacityLoss;
   private bool isMelting { get; set; }
   private float curMeltingTime;
   private float _curOpacity; 
   [SerializeField]private float meltingTime;
   [SerializeField] private float meltingProgress;
      private float melted;

   private bool active , checkPoint1, checkPoint2, checkPoint3, checkPoint4;
   private Renderer _renderer;
   private BoxCollider _boxCollider;
   private Player _player;
   private Material iceMat;


 
  
   [SerializeField] GameObject _DoorHitbox;
   private bool AtMeltingPoint;

   void Start()
   {
       //Time.timeScale = 10;
        _boxCollider = GetComponent<BoxCollider>();
        _renderer = GetComponent<Renderer>();
        _player = FindObjectOfType<Player>();
      //  meltingProgress = 400;
        _curOpacity = 1;
        iceMat = GetComponent<Renderer>().material;
    }
 
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
            AtMeltingPoint = true;
        }
        if  (_player.returnTorchState()&& AtMeltingPoint)
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
        print(_player.returnTorchState());
        if (isMelting && !active)
        {
            active = true;
            StartCoroutine(CheckIsMelting());
        }

        if (isMelting)
        {
            meltingProgress -= Time.deltaTime;
            //base = 0.0025 mod(action block) = 0.0050
            _curOpacity -=  opacityLoss * Time.deltaTime;
            iceMat.SetFloat("_Opacity", _curOpacity);
        }
        CheckMeltingProgress();
    }

    private void CheckMeltingProgress()
    {
        if (meltingProgress <= melted)
        {
            checkPoint1 = true;
            print("Final checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 0);
             _renderer.enabled = false;
             _boxCollider.enabled = false;
            _DoorHitbox.SetActive(false);

        } 
        else if (meltingProgress <= 100)
        {
            checkPoint2 = true;
          //  print("third last checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 25);
        }

        else if (meltingProgress <= 200)
        {
            checkPoint3 = true;
         //   print("second checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 50);
        }

        else if (meltingProgress <= 300)
        {
            checkPoint4 = true;
           // print("first checkpoint");
            PlayerPrefs.SetFloat("CheckpointOpacity", 75);
        }
    }

    private IEnumerator CheckIsMelting()
    {
        print("melting active ");
        yield return new WaitUntil(() => isMelting == false );
        active = false;
    }
}
