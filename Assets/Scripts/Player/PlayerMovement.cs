using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    //standard movement
    [SerializeField] internal float moveSpeed;
    Rigidbody rb;
    private Vector3 smoothedMoveDir;
    private Vector3 smoothedMoveVelo;
    private Vector3 moveDir;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    //Sprinting
    [Header("Sprinting")]
    Vector3 previousPos;

    internal bool isSprinting;
    [SerializeField] float sprintTime;
    private bool onCoolDownFull;
    private bool onCoolDownNormal;
    [SerializeField] internal float sprintSpeed;

    [SerializeField] private float maxSprintTime;
    [SerializeField] private float sprintRegenModifer;

    [SerializeField] CinemachineInputProvider inputProvider;
    //Camera location
    private Transform CamTransform;
    private Player _player;
    private PlayerHotbar _playerHotbar;
    private Torch _torch;
    [SerializeField]private Slider _SprintSlider;
    [SerializeField]Color currentColor; 
    [SerializeField]  private Image _SprintSliderfill;
    [SerializeField]  private Image _SprintSliderBackground;
    // Start is called before the first frame update
    void Start()
    {
        currentColor = _SprintSliderfill.color;
        _player = GetComponent<Player>();
        CamTransform = Camera.main.transform;
        _playerHotbar = GetComponent<PlayerHotbar>();
        _torch = _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>();
        rb = GetComponent<Rigidbody>();
        sprintTime = maxSprintTime;
        _SprintSlider.maxValue = maxSprintTime;
    }
    //this handles movement 
    public void SetMoveDirection(Vector3 newDir)
    {
        if (!_player.dead)
        {
            moveDir = newDir.normalized;
        }

    }

    public IEnumerator SetStimSpeed()
    {
        _SprintSliderBackground.color = Color.red;
        _SprintSliderfill.color = Color.red;
        print("Speed Changed");
        sprintTime = maxSprintTime;
        moveSpeed = 17;
        sprintSpeed = 1.7f;
        yield return new WaitForSecondsRealtime(15);
        moveSpeed = 15f;
        sprintSpeed = 1.5f;
        _SprintSliderfill.color = currentColor;
        _SprintSliderBackground.color = Color.white;
        
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
//        if(!onCoolDownFull && !onCoolDownNormal)
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

    public void DisableInput()
    {
        inputProvider.enabled = false;
        InputManager.DisableInGame();
        
    }

    public void EnableInput()
    {
        inputProvider.enabled = true;
        InputManager.EnableInGame();
    }

    private void CheckSprint()
    {
        
        if (isSprinting && !onCoolDownFull && !onCoolDownNormal && !_torch.torchActive)
        {
            virtualCamera.m_Lens.FieldOfView = 45;
//            print("Decreasing sprint");
            sprintTime -= Time.deltaTime;
            if (sprintTime <= 0)
            {
                StartCoroutine(onSprintEmpty(3));
                isSprinting = false;
                rb.velocity = Vector3.zero;
                sprintTime = 0;
  //              print("Sprint End");
                return;
            }
            if (isSprinting && !(sprintTime <= 0)) return;
            if (!(sprintTime <= 2) && !(sprintTime <= maxSprintTime)) return;
        }
        else if (!onCoolDownFull && !onCoolDownNormal && sprintTime !<= maxSprintTime)
        {
      //      print("Increasing sprint");
            sprintTime +=  sprintRegenModifer * Time.deltaTime;   
        }
        
    }
    private IEnumerator onSprintEnd(int cooldown)
    {
        if (!onCoolDownFull)
        {
            virtualCamera.m_Lens.FieldOfView = 40;

            onCoolDownNormal = true;
  //          print("cooldown normal sprint");
            yield return new WaitForSecondsRealtime(cooldown);
            onCoolDownNormal = false;

        }
    }
    private IEnumerator onSprintEmpty(int cooldown )
    {
        if (!onCoolDownNormal)
        {
            virtualCamera.m_Lens.FieldOfView = 40;

            onCoolDownFull = true;
  //          print("cooldown full sprint");

            yield return new WaitForSecondsRealtime(cooldown);
            onCoolDownFull = false;
            sprintTime = maxSprintTime;

        }
    
    }

    // Update is called once per frame
    void Update()
    {
        CheckSprint();
        _SprintSlider.value = sprintTime;
    }
}
