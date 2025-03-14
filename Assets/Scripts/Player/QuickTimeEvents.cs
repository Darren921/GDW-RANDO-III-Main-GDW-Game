using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class QuickTimeEvents : MonoBehaviour
{
    [SerializeField] private IceMelting iceMelting;
    [SerializeField] private Slider _slider;
    [SerializeField] internal InputActionReference QTEInteract;
    private PlayerHotbar _playerHotbar;
    private PlayerInteraction _playerInteraction;

    private float valueMin, valueMax;
    internal static bool QTEActive;
    internal bool QTESucess, interacted;
    [SerializeField] internal  float qteMin;
    [SerializeField] private float Range;
    private float PressedValue;
    private bool  increasing;
    internal State state;
    [SerializeField] private float _sliderSpeed;
    [SerializeField] private RectTransform  _targetImage;
    internal bool cooldown;

    internal enum State
    {
        NotStarted,
        InProgress,
        Ended,
        Used
    }

    private void Start()
    {
        _playerInteraction =FindFirstObjectByType<PlayerInteraction>();
        _playerHotbar = FindFirstObjectByType<PlayerHotbar>();
        increasing = true;
        state = State.NotStarted;
        _slider.gameObject.SetActive(false);
    }

    public void StartQTE()
    {
        if(cooldown)return;
        //Generate Target Area
        GenerateRange();
        //Activate slider 
        StartCoroutine(StartModValue());
        //Place Target on slider
        PlaceTargetArea();
    }

    private void Update()
    {
        // if state in progress increase and decrease based on slider values 
        if (state == State.InProgress)
        {
            if (increasing)
            {
                _slider.value += _sliderSpeed * Time.deltaTime;
                if (_slider.value >= _slider.maxValue) increasing = false;
            }
            else
            {
                _slider.value -= _sliderSpeed * Time.deltaTime;
                if (_slider.value <= _slider.minValue) increasing = true;
            }
        }
    }

    private IEnumerator StartModValue()
    {
        _slider.gameObject.SetActive(true);
        state = State.InProgress;
        yield return new WaitUntil(() => state == State.Ended);
        StopQTE();
    }

    private void PlaceTargetArea()
    {
        
        _targetImage.anchorMin = new Vector2( Mathf.InverseLerp(_slider.minValue, _slider.maxValue, valueMin) , 0.5f);
        _targetImage.anchorMax = new Vector2( Mathf.InverseLerp(_slider.minValue, _slider.maxValue, valueMax), 0.5f);
        
        _targetImage.offsetMin = Vector2.zero;
        _targetImage.offsetMax = Vector2.zero;
        var delta = _targetImage.sizeDelta;
        delta.y = 40;
        _targetImage.sizeDelta = delta;
    }

    private void GenerateRange()
    {
        //generate min value
        valueMin = UnityEngine.Random.Range(_slider.minValue, _slider.maxValue);

        //choose between value that is + or - valueMin 
        var Choice = UnityEngine.Random.Range(1, 2);
        switch (Choice)
        {
            case 1:
                valueMax = valueMin - Range;
                break;
            case 2:
                valueMax = valueMin + Range;
                break;
        }

        //swapping min and max values if min is greater than max
        if (valueMin > valueMax) (valueMin, valueMax) = (valueMax, valueMin);
        //Refreshing till values meet conditions 
        while (valueMin < _slider.minValue || valueMax > _slider.maxValue) GenerateRange();
    }

    public void InteractQTE()
    {
        interacted = true;
        if (state != State.InProgress || cooldown) return;
        PressedValue = _slider.value;
        Time.timeScale = 0;
        if (PressedValue >= valueMin && PressedValue <= valueMax)
          QTESucess = true;
        else 
            QTESucess = false;
        state = State.Ended;
    }

 

    public void StopQTE()
    {
        print(QTESucess);
        print(interacted);
        if (QTESucess && interacted)
        {
            print("triggered");
            cooldown = true;

        }
        else if(!QTESucess && interacted)
        {
            print("triggered");
            _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().ReduceCount(1f);
            cooldown = true;
        }

        if (_playerInteraction.holdDuration == 0)
        {
            if (QTESucess && cooldown)
            {
                _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().ReduceCount(0.5f);

            }
            else if(!QTESucess && cooldown)
            {
                _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().ReduceCount(1f);
            }
            cooldown = false;
        }
         interacted = false;
        _slider.value = 0;
        _slider.gameObject.SetActive(false);
        Time.timeScale = 1;
        state = State.NotStarted;
      
    }

    
}
