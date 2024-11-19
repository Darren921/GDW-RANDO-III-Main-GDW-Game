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
        if (curMeltingTime >= meltingTime)
        {
            meltingProgress += Time.deltaTime;
        }
        if (isMelting)
        {
            if (curMeltingTime <= 5)
            {
                curMeltingTime += Time.deltaTime;
            }
        }
        else
        {
            curMeltingTime = 0;
        }

        CheckMeltingProgress();
    }

    private void CheckMeltingProgress()
    {
        if (meltingProgress >= 25)
        {
            checkPoint1 = true;
        }
        if (meltingProgress >= 50)
        {
            checkPoint2 = true;
        }

        if (meltingProgress >= 75)
        {
            checkPoint3 = true;
        }

        if (meltingProgress >= 100)
        {
            checkPoint4 = true;
        }
    }

    private IEnumerator CheckIsMelting()
    {
        yield return new WaitUntil(() =>isMelting == false );
        active = false;

    }
}
