using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] CinemachineInputProvider inputProvider;
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

    private int CurrentItem;
    
    public static bool isDead;
    private PlayerHotbar _playerHotbar;
    private Torch _torch;

    void Start()
    {
        _playerHotbar = GetComponent<PlayerHotbar>();
        _torch = _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>();
        isDead = false;
        //turn on and off when needed
       // torchActive = true;
       //fuelLeft = 500;
      // fuelLeft = 100;
       _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
       _enemy = FindObjectOfType<Enemy>();
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
            /*if (Input.GetMouseButtonDown(0)) // Left-click
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                // Raycast results
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                foreach (var result in results)
                {
                    Debug.Log($"Hit UI Element: {result.gameObject.name}");
                }

                if (results.Count == 0)
                {
                    Debug.Log("No UI Element hit.");
                }
            }*/
        }
    

    

    private void FixedUpdate()
    {
        if (!dead)
        {
            //smoothed movement
           
            if (isOpen)
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
        {
            SceneManager.LoadScene("DeathScreen");
            Cursor.lockState = CursorLockMode.None;
            isDead = false;
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
        isDead = true;
        SceneManager.LoadScene("DeathScreen");
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

    public void ManageHotbar ()
    {
        if (!isOpen)
        {
            isOpen = true;
           

        }
        else
        {
            isOpen = false;
          

        }



    }

    public bool isOpen { get; set; }
}
    

