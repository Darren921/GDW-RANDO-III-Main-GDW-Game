using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    //standard movement
    [SerializeField] float moveSpeed;
    Rigidbody rb;
    private Vector3 smoothedMoveDir;
    private Vector3 smoothedMoveVelo;
    private Vector3 moveDir;

    //Sprinting
    [Header("Sprinting")]
    Vector3 previousPos;
    private bool isSprinting;
    [SerializeField] float sprintTime;
    private bool onCoolDownFull;
    private bool onCoolDownNormal;
    [SerializeField] private float sprintSpeed;

    [SerializeField] private float maxSprintTime;

    [SerializeField] CinemachineInputProvider inputProvider;
    //Camera location
    private Transform CamTransform;
    private Player _player;
    private PlayerHotbar _playerHotbar;
    private Torch _torch;
    [SerializeField] AudioSource walking;
    [SerializeField] private Slider SprintBar;

    // Start is called before the first frame update
    void Start()
    {
        SprintBar.maxValue = maxSprintTime;
        SprintBar.value = sprintTime;
        _player = GetComponent<Player>();
        CamTransform = Camera.main.transform;
        _playerHotbar = GetComponent<PlayerHotbar>();
        _torch = _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>();
        rb = GetComponent<Rigidbody>();
        sprintTime = maxSprintTime;

    }
    //this handles movement 
    public void SetMoveDirection(Vector3 newDir)
    {
        if (!_player.dead)
        {
            moveDir = newDir.normalized;
        }

    }
    

    private void FixedUpdate()
    {
        if (!_player.dead)
        {
            //smoothed movement
           
            if (_playerHotbar.isOpen)
            {
                inputProvider.enabled = false;
                rb.velocity = Vector3.zero;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                inputProvider.enabled = true;
                smoothedMoveDir = Vector3.SmoothDamp(smoothedMoveDir, moveDir, ref smoothedMoveVelo, 0.1f);
                smoothedMoveDir = CamTransform.forward * moveDir.z + CamTransform.right * moveDir.x;
                rb.velocity = isSprinting && !onCoolDownFull && !onCoolDownNormal ? new Vector3(smoothedMoveDir.x * (moveSpeed * sprintSpeed) , -3, smoothedMoveDir.z * (moveSpeed * sprintSpeed)) : new Vector3(smoothedMoveDir.x * moveSpeed, -3, smoothedMoveDir.z * moveSpeed);
            }
        }
    }
    
    public void startSprint()
    {
        if(!onCoolDownFull && !onCoolDownNormal)
            isSprinting = true;
    }

    public void cancelSprint()
    {
        if(!onCoolDownFull && !onCoolDownNormal)
            isSprinting = false;

        if(!onCoolDownFull && !onCoolDownNormal)
        {
            onCoolDownNormal = true;
            StartCoroutine(onSprintEnd(1));
        }
       
    
    }

    public void stopWalkingSound()
    {
        if (walking != null)
        {
            walking.enabled = false;
        }
    }

    public void walkingSound()
    {
        if (walking is null || walking.isPlaying) return;
        walking.enabled = true;
        walking.Play();
    }

    private void CheckSprint()
    {
        if (isSprinting && !onCoolDownFull && !onCoolDownNormal && !_torch.torchActive)
        {
            sprintTime -= Time.deltaTime;
            if (sprintTime <= 0)
            {
                StartCoroutine(onSprintEmpty(3));
                isSprinting = false;
                rb.velocity = Vector3.zero;
                sprintTime = 0;
                return;
            }
            if (isSprinting && !(sprintTime <= 0)) return;
            if (!(sprintTime <= 2) && !(sprintTime <= maxSprintTime)) return;
        }
        if (!onCoolDownFull && !onCoolDownNormal && sprintTime !<= maxSprintTime)
        {
            sprintTime += Time.deltaTime;   
        }
    }
    private IEnumerator onSprintEnd(int cooldown)
    {
        if (!onCoolDownFull)
        {
            onCoolDownNormal = true;
            yield return new WaitForSecondsRealtime(cooldown);
            onCoolDownNormal = false;
        }
    }
    private IEnumerator onSprintEmpty(int cooldown )
    {
        if (!onCoolDownNormal)
        {
            onCoolDownFull = true;
            yield return new WaitForSecondsRealtime(cooldown);
            onCoolDownFull = false;
            sprintTime = maxSprintTime;
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        CheckSprint();
        SprintBar.value = sprintTime;
    }
}
