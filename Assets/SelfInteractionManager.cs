using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelfInteractionManager : MonoBehaviour, GameManager.IInteractable
{
    protected FrostSystem frostSystem;
    protected PlayerHotbar _playerHotbar;
    protected PlayerInteraction _playerInteraction;
    // Start is called before the first frame update
    protected void Start()
    {
        _playerHotbar = FindObjectOfType<PlayerHotbar>();
        frostSystem = FindObjectOfType<FrostSystem>();
        _playerInteraction = FindObjectOfType<PlayerInteraction>();
        isHeld = true;
        QTEAble = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (frostSystem.DeFrosting == true)
        {
            if (frostSystem._curOpacity < -0.98)
            {
                frostSystem.DeFrosting = false;
            }
        }
    }

    public bool QTEAble { get; set; }
    public bool isHeld { get; set; }
    public void Interact()
    {
        
    }

    public virtual void HeldInteract()
    {
        _playerHotbar.curEquipmentBase?.HeldInteract();
    }
}
