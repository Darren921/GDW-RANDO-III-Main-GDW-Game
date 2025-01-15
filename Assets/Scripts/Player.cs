using System;
using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Sprinting
    [Header("Sprinting")]
    Vector3 previousPos;
    private bool isSprinting;
    [SerializeField] float sprintTime;
    private bool onCoolDownFull;
    private bool onCoolDownNormal;
    [SerializeField] private float sprintSpeed;

    [SerializeField] private float maxSprintTime;

    //Camera location
    private Transform CamTransform;
    
    [Header("Movement")]
    //standard movement
    [SerializeField] float moveSpeed;
    Rigidbody rb;
    private Vector3 smoothedMoveDir;
    private Vector3 smoothedMoveVelo;
    private Vector3 moveDir;

    //Death and Sounds
    [Header("Death and Sounds")]
    [SerializeField] AudioSource sound;
    [SerializeField] CinemachineVirtualCamera cineCam;
    public bool dead;
    [SerializeField] GameObject enemylookat;

    //testing 
    private Vector3 mousePos;
    Camera playerCam;
    RaycastHit hit;

    //hiding 
    public CinemachineVirtualCamera HidingCam;
    bool inhidingRange;
    public bool isHiding;
    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject flashlight;
    
    //Heartbeat
    [Header("HeartBeat")]    
    private float distance;
    private bool isPlaying;
    [SerializeField] AudioSource heartBeat;
    private Enemy _enemy;
    [SerializeField] AudioClip heartbeatS, heartbeatSM, heartbeatM, heartbeatF;

    
    [SerializeField] AudioSource walking;
    private CapsuleCollider _capsuleCollider;

    //Item switching 
    [Header("Item switching ")]
    private int _slotNumber;
    [SerializeField] GameObject Flashlight,Torch,flashlightSource,torchSource;
    //Torch 
    private bool torchActive,flashlightActive;
    private float fuelLeft;
    [SerializeField] Slider TorchSlider;
    private bool AtMeltingPoint;

    //Flashlight
    private bool _equipedTorch, _equipedFlashlight;
    private float _chargeleft;
    private bool CheckActive;
    private bool IsLooking;
    [SerializeField] Slider FlashlightSlider;
    [SerializeField]LayerMask CollisionLayer;
    private float slotNumber;

    void Start()
    {
        slotNumber = 1;
        //turn on and off when needed
       // torchActive = true;
       //fuelLeft = 500;
      // fuelLeft = 100;
      fuelLeft = 50;
       _chargeleft = 150;
       _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
       _enemy = FindObjectOfType<Enemy>();
       Flashlight.SetActive(false);
       Torch.SetActive(false);
       flashlightSource.SetActive(false);
       torchSource.SetActive(false);
       playerCam = gameObject.GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        InputManager.Init(this);
        InputManager.EnableInGame();
        CamTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        sprintTime = maxSprintTime;
    }


    void Update()
    {
        //HeartBeats sounds
        TorchSlider.value = fuelLeft;
        FlashlightSlider.value = _chargeleft;
        // distance = Vector3.Distance(transform.position, _enemy.transform.position);

        if (distance < 150f)
        {
            //  StartCoroutine(CheckDistance());
        }
        else
        {
            heartBeat.Stop();
        }
        
        //sprinting
        CheckSprint();
        
        if (_chargeleft > 0 && flashlightActive)
        {
            _chargeleft -= Time.deltaTime;
        }
        if (fuelLeft > 0 && torchActive)
        {
            fuelLeft -= Time.deltaTime;
        }
        if(fuelLeft < 0)
        {
            torchSource.SetActive(false);
            torchActive = false;
        }
        if(_chargeleft < 0) 
        { 
            flashlightSource.SetActive(false);
            flashlightActive = false;

        }
        
        
    }

  
    
   

    public void CheckIfActive()
    {
        if (flashlightSource == null)
        {
            flashlightActive = GameObject.Find("Spot Light");
        }
       // print("working");
        if (_equipedTorch )
        {
           torchActive = !torchActive;
           if (fuelLeft > 0)
           {
               if (torchActive && !CheckActive)
               {
                   CheckActive = true;
                   torchSource.SetActive(true);
                   StartCoroutine(CheckCharge("Torch")) ;
               }
               else
               {
                   torchSource.SetActive(false);
               }
           }
           else
           {
               torchSource.SetActive(false);
           }
        }
        if (_equipedFlashlight)
        {
            flashlightActive = !flashlightActive;
            if (_chargeleft > 0)
            {
                if (flashlightActive && !CheckActive)
                {
                    CheckActive = true;
                    flashlightSource.SetActive(true);
                   StartCoroutine(CheckCharge("Flashlight")) ;
                }
                else
                {
                   
                    flashlightSource.SetActive(false);

                }
            }
            else
            {
                flashlightSource.SetActive(false);
            }
        }
    }

    private IEnumerator CheckCharge(string item)
    {
        if (item == "Flashlight")
        {
            yield return new WaitUntil(() => flashlightActive == false );
            CheckActive = false;

        }
        else if (item == "Torch")
        {
            yield return new WaitUntil(() => torchActive == false );
            CheckActive = false;

        }
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            //smoothed movement
            smoothedMoveDir = Vector3.SmoothDamp(smoothedMoveDir, moveDir, ref smoothedMoveVelo, 0.1f);
            smoothedMoveDir = CamTransform.forward * moveDir.z + CamTransform.right * moveDir.x;
            rb.velocity = isSprinting && !onCoolDownFull && !onCoolDownNormal ? new Vector3(smoothedMoveDir.x * (moveSpeed * sprintSpeed) , -3, smoothedMoveDir.z * (moveSpeed * sprintSpeed)) : new Vector3(smoothedMoveDir.x * moveSpeed, -3, smoothedMoveDir.z * moveSpeed);
        }


    }

    //this handles movement 
    public void SetMoveDirection(Vector3 newDir)
    {
        if (!dead)
        {
            moveDir = newDir.normalized;
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

   

    private void CheckSprint()
    {
        if (isSprinting && !onCoolDownFull && !onCoolDownNormal && !torchActive)
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


    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
           
            case "Batteries":
                if (_chargeleft < 150)
                {
                    _chargeleft += 30;
                    if (_chargeleft > 150)
                    {
                        _chargeleft = 150;
                    }
                }
                break;
            case "Fuel":
                if ( fuelLeft < 100)
                {
                    fuelLeft += 25;
                    if (fuelLeft > 100)
                    {
                        fuelLeft = 100;
                    }
                }
                break;
            case "Exit":
                SceneManager.LoadScene("NewMainMenu");
                Cursor.lockState = CursorLockMode.None;

                break;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            dead = true;
            StartCoroutine(LookatDeath());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("hidingSpot")) return;
        inhidingRange = true;
        HidingCam = other.gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hidingSpot"))
        {
            HidingCam = null;
        }
    }

    public IEnumerator LookatDeath()
    {
        //heartBeat.clip = heartbeatF;
        //heartBeat.Play();
        //cineCam.Priority = 100;
        //sound.Play();
        //yield return new WaitForSeconds(3);
        InputManager.DisableInGame();
        yield return null;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("NewMainMenu");
    }

    internal void Hide()
    {
        StartCoroutine(GoIntoHiding());
    }

    public IEnumerator GoIntoHiding()
    {
        if (!isHiding)
        {
            if (inhidingRange && !dead)
            {
                hitbox.SetActive(false);
                previousPos = gameObject.transform.position;
                HidingCam.Priority = 100;
                isHiding = true;
                GameObject.FindWithTag("Enemy").GetComponent<Enemy>().Spotted = false;
                GameObject.FindWithTag("Enemy").GetComponent<Enemy>().DesLocation();
                flashlight.SetActive(false);
                yield return new WaitForSeconds(2);
                _capsuleCollider.enabled = false;
                gameObject.transform.position = HidingCam.transform.position;

            }
        }
        else if (isHiding)
        {
            HidingCam.Priority = 9;
            _capsuleCollider.enabled = true;
            isHiding = false;
            gameObject.transform.position = previousPos;
            flashlight.SetActive(true);
            HidingCam = null;
            hitbox.SetActive(true);
        }
    }
    
    public void ChangeItem(float slot)
    {
         slotNumber = slot;
        switch (slotNumber)
        {
            case 1:
               // print("equipped flashlight");
                _equipedFlashlight = true;
                _equipedTorch = false;
                torchActive = false;
                flashlightActive = false;
                flashlightSource.SetActive(false);
                Flashlight.gameObject.SetActive(true);
                torchSource.gameObject.SetActive(false);
                Torch.gameObject.SetActive(false);
                break;
            case 2:
             //   print("equipped torch");
                _equipedTorch = true;
                _equipedFlashlight = false;
                flashlightActive = false;
                torchActive = false;
                torchSource.gameObject.SetActive(false);
                Torch.gameObject.SetActive(true);
                flashlightSource.SetActive(false);
                Flashlight.gameObject.SetActive(false);
                break;
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
        if (walking != null && !walking.isPlaying)
        {
            walking.enabled = true;
            walking.Play();
        }
    }

    public bool returnTorchState()
    {
        return torchActive;
    }
}
    

