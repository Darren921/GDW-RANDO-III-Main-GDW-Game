using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IceMelting : MonoBehaviour
{
   private bool isMelting { get; set; }
   private float curMeltingTime;
   [SerializeField]private float meltingTime;
   private Player _player;
   private float meltingProgress;
   private bool active , checkPoint1, checkPoint2, checkPoint3, checkPoint4;
    void Start()
    {
        _player = FindObjectOfType<Player>();
        meltingProgress = 400;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _player.returnTorchState())
        {
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
    }

    void Update()
    {
        if (isMelting && !active)
        {
            active = true;
            StartCoroutine(CheckIsMelting());
        }

        if (isMelting)
        {
            meltingProgress -= Time.deltaTime;
        }
        CheckMeltingProgress();
    }

    private void CheckMeltingProgress()
    {
        if (meltingProgress <= 0)
        {
            checkPoint1 = true;
            print("Final checkpoint");
        }
        if (meltingProgress <= 100)
        {
            checkPoint2 = true;
            print("third last checkpoint");

        }

        if (meltingProgress <= 200)
        {
            checkPoint3 = true;
            print("second checkpoint");

        }

        if (meltingProgress <= 300)
        {
            checkPoint4 = true;
            print("first checkpoint");
        }
    }

    private IEnumerator CheckIsMelting()
    {
        print("melting active ");
        yield return new WaitUntil(() => isMelting == false );
        active = false;
    }
}
