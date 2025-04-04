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
    public static bool isFreezing;
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
    private PlayerInteraction _playerInteraction;
    private GameManager.IInteractable currentInteractable;
    [SerializeField] private float _frostRate;
    [SerializeField] private float opacityRate;

    // Start is called before the first frame update
    protected void Start()
    {
        iceMelting = FindObjectOfType<IceMelting>();
        _playerHotbar = FindFirstObjectByType<PlayerHotbar>();
        _playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        isFreezing = false;
        
        _frost = 0;
        _frostTexture.SetFloat("_Opacity", -1);
        _curOpacity = -1;

    }

    protected void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<GameManager.IInteractable>();
        if (interactable == null) return;
        currentInteractable = interactable; 
//        print(currentInteractable);
        if (currentInteractable != null)
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
//        print(currentInteractable);
        if (currentInteractable != null )
        {
            DeFrost.SetActive(false);
//            print(currentInteractable);
        }
        else switch (iceMelting.AtMeltingPoint)
        {
            case true:
                DeFrost.SetActive(false); 
                
                print("Frost");
                break;
            case false:
                DeFrost.SetActive(true); 
               print("Tell me WHY");
                break;
        }
        
       
        
    }

    protected void OnTriggerExit(Collider other)
    {
       StartCoroutine(DelayRelease()) ;
    }

    private IEnumerator DelayRelease()
    {
        yield return null;
        DeFrost.gameObject.SetActive(true); 

    }


    void FixedUpdate()
    {
        if (!isFreezing && !GameManager.TutorialActive)
        {
            Mathf.Lerp(_frost,0, 5 * Time.fixedDeltaTime);
            _curOpacity = Mathf.Lerp(_curOpacity, -1, 5 * Time.fixedDeltaTime);
        }
        _frost -= _frost;
        _curOpacity = -1;
    }
    // Update is called once per frame
    void Update()
    {
        if (_player is null) return;
        if (!isFreezing) return;
        if (!_player.dead)
        {
            _frost += _frostRate * Time.deltaTime;
            _curOpacity += opacityRate * Time.deltaTime;
            _frostTexture.SetFloat("_Opacity" , _curOpacity);     

        }
        if (_frost >= maxFrost)
        {
            _player.dead = true;
            _curOpacity = -1;
            if (_player.dead) StartCoroutine(Player.LookAtDeath("frost"));
        }
    }

    private void OnDisable()
    {
        _frost = 0;
        _curOpacity = -1;

    }

    public IEnumerator reduceFrost()
    {
        isFreezing = false;
        
        yield return new WaitUntil (() => _frost == 0);
        isFreezing = true;
        
    }
    
    
}
