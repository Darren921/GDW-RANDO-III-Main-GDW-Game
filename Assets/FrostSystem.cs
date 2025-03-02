using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FrostSystem : MonoBehaviour
{
    [Header("Frost")]
    bool isFreezing;
    [SerializeField]internal float _frost;
    internal bool DeFrosting;
    [SerializeField] internal GameObject DeFrost;
    [SerializeField] private float maxFrost;
    [SerializeField] private Material _frostTexture;
    private IceMelting iceMelting;
    private float _curOpacity;

    [Header("References")]
    [SerializeField] internal Player _player;

    protected PlayerHotbar _playerHotbar;
    private GameManager.IInteractable currentInteractable;


    // Start is called before the first frame update
    protected void Start()
    {
        iceMelting = FindObjectOfType<IceMelting>();
        _playerHotbar = FindFirstObjectByType<PlayerHotbar>();
        isFreezing = true;
        
        _frost = 0;
        _frostTexture.SetFloat("_Opacity", -1);
        _curOpacity = -1;

    }

    protected void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<GameManager.IInteractable>();
        if (interactable == null) return;
        currentInteractable = interactable; 
        print(currentInteractable);
        if (currentInteractable != null )
        {
            DeFrost.SetActive(false); 
        }
        else switch (iceMelting.AtMeltingPoint)
        {
            case true:
                DeFrost.SetActive(false); 
                break;
            case false:
                DeFrost.SetActive(true); 
              //  print("Tell me WHY");
                break;
        }
        
       
        
    }

    protected void OnTriggerStay(Collider other)
    {
        var interactable = other.GetComponent<GameManager.IInteractable>();
        if (interactable == null) return;
        currentInteractable = interactable; 
        print(currentInteractable);
        if (currentInteractable != null )
        {
            DeFrost.SetActive(false); 
        }
        else switch (iceMelting.AtMeltingPoint)
        {
            case true:
                DeFrost.SetActive(false); 
                break;
            case false:
                DeFrost.SetActive(true); 
              //  print("Tell me WHY");
                break;
        }
        
       
        
    }

    protected void OnTriggerExit(Collider other)
    {
         DeFrost.gameObject.SetActive(true); 
    }
    

    // Update is called once per frame
    void Update()
    {
        if (_player is null) return;
        if (!isFreezing) return;
        if (!_player.dead)
        {
            _frost += Time.deltaTime;
            _curOpacity += 0.019f * Time.deltaTime;
            _frostTexture.SetFloat("_Opacity" , _curOpacity);     

        }
        if (_frost >= maxFrost)
        {
            _player.dead = true;
            _curOpacity = -1;
            if (_player.dead) StartCoroutine(Player.LookAtDeath("frost"));
        }
    }

    public void reduceFrost()
    {
        _frost -= _frost;
        _curOpacity = -1;
    }
    
    
}
