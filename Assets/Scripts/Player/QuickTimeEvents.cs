using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuickTimeEvents : MonoBehaviour
{
    [SerializeField] private IceMelting iceMelting;
    [SerializeField] private Slider _slider; 
    [SerializeField]  internal InputActionReference QTEInteract;
    

    private float valueMin, valueMax;
    [SerializeField] private float Range;
    private float PressedValue;
    private bool interacted;
    private State state;
    private bool increasing;
    [SerializeField] private float _sliderSpeed;
    [SerializeField] private Image _targetImage;
    enum State
    {
        NotStarted,
        InProgress,
        Ended,
      
    }
    private void Start()
    {
        increasing = true;
        state = State.NotStarted;
        _slider.gameObject.SetActive(false);
    }

    public void StartQTE()
    {
        GenerateRange();
        StartCoroutine(StartModValue());
        PlaceHighlightedArea();
    }

    private void Update()
    {
        if (state == State.InProgress)
        {
            if (increasing)
            {
                _slider.value += _sliderSpeed * Time.deltaTime;
                if (_slider.value >= _slider.maxValue)
                {
                    increasing = false;
                }
            }
            else
            {
                _slider.value -= _sliderSpeed * Time.deltaTime;
                if (_slider.value <= _slider.minValue)
                {
                    increasing = true;
                }
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

    private void PlaceHighlightedArea()
    {
      
    }

    private void GenerateRange()
    {
         valueMin = UnityEngine.Random.Range(_slider.minValue, _slider.maxValue);
        var Choice = UnityEngine.Random.Range(1, 2);
        switch (Choice)
        {
            case 1:
               valueMax =  valueMin - Range;
                break;
            case 2:
                valueMax =  valueMin + Range;
                break;
            
        }
        if (valueMin > valueMax)
        {
            var temp = valueMin;
             
            valueMin = valueMax;
             
            valueMax = temp;
        }
         while (valueMin  < _slider.minValue || valueMax > _slider.maxValue)
         {
             GenerateRange();
         }

       
         
    }

    public void InteractQTE()
    {
        if(state != State.InProgress) return;
        PressedValue = _slider.value;
        Time.timeScale = 0;
        if (PressedValue >= valueMin && PressedValue <= valueMax)
        {
            print("Successfully Performed");
        }
        else
        {
            print("Failed to Perform");
        }
        state = State.Ended;
    }

    private void StopQTE()
    {
       
        _slider.value = 0;
        _slider.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    
    
}
