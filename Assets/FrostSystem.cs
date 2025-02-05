using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostSystem : MonoBehaviour,GameManager.IInteractable
{
    [Header("Frost")]
    bool isFreezing;
    [SerializeField]internal float _frost;
    [SerializeField] private float maxFrost;
    private float _curOpacity;

    
    [Header("References")]
    [SerializeField] private Player _player;
    PlayerHotbar _playerHotbar;

    // Start is called before the first frame update
    void Start()
    {
        _playerHotbar = FindFirstObjectByType<PlayerHotbar>();
        isHeld = true;
        isFreezing = true;
        _frost = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player is null) return;
        if (!isFreezing) return;
        _frost += Time.deltaTime;
        if (_frost >= maxFrost)
        {
            _player.dead = true;

            if (_player.dead) StartCoroutine(Player.LookAtDeath());
        }
    }

    public void reduceFrost()
    {
        _frost -= _frost;
    }
    
    public bool isHeld { get; set; }
    public void Interact()
    {
        
    }

    public void HeldInteract()
    {
        print("here");
        InputManager.ChangeBinding(true);
        
        if (!(_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses > 0) ||
            !(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1f)) return;
        
        print("in 1 ");
        
        if (!_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].equipped) return;
        print("in 2 ");
        
        if (_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration >= 4.9f)
        {
            print("Held Interact Self");
             reduceFrost();
            _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().reduceCount();
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().Reset();
            _playerHotbar.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();
        }
        
        else if(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1)
        {
            print("in oh no ");
            print(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration);
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().Reset();
            _playerHotbar.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();

        }
    }
}
