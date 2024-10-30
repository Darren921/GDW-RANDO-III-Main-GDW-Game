using Cinemachine;
using System.Collections;
using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    //Sprinting
    Vector3 previousPos;
    private bool isSprinting; 
    [SerializeField]  float sprintTime;
    private bool onCoolDown;
    [SerializeField] private float maxSprintTime;
    //Camera location
    private Transform CamTransform;
    [SerializeField]bool canusePanel;
    [SerializeField] float f;
    [SerializeField] GameObject codePanelInfo;
    //standard movement
    [SerializeField] float moveSpeed;
    Rigidbody rb;
    private Vector3 smoothedMoveDir;
    private Vector3 smoothedMoveVelo;
    private Vector3 moveDir;
    //Death and sounds
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
    [SerializeField]  GameObject flashlight;
    //Heartbeat 
    private float distance;
    private bool isPlaying;
    [SerializeField] AudioSource heartBeat;
    private Enemy _enemy;
    [SerializeField] AudioClip heartbeatS, heartbeatSM, heartbeatM, heartbeatF;
    
    //Storybased
    public bool isTalking { get;  set; }
    [SerializeField] AudioSource walking;


    void Start()
    {
        _enemy = FindObjectOfType<Enemy>();
      
        playerCam = gameObject.GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        InputManager.Init(this);
        InputManager.EnableInGame();
        CamTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        

    }

    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            stopWalkingSound();
        }
        
        // remove this after
        // distance = Vector3.Distance(transform.position, _enemy.transform.position);
       
        if (distance < 150f)
        { 
          //  StartCoroutine(CheckDistance());
        }
        else
        {
            heartBeat.Stop();
        }
        /*
        Input.GetMouseButtonDown(0);
        {
            Mouse mouse = Mouse.current;
            if (mouse.leftButton.wasPressedThisFrame)
            {
                mousePos = playerCam.ScreenToWorldPoint(InputManager.GetMousePos());

              
                Ray ray = playerCam.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 10);
                    print(hit.collider.name);
                }

            }
        }
    */

    if (isSprinting )
        {
            sprintTime -= Time.deltaTime;
            if (sprintTime >= 0)
            {
                rb.velocity = smoothedMoveDir * (moveSpeed * 1.5f);
            }
            else if (sprintTime <= 0)
            {
                StartCoroutine(Cooldown(2));
                isSprinting = false;
                rb.velocity = Vector3.zero; 
                sprintTime = 0;
            }
        }
          if (!isSprinting ||  sprintTime <= 0 )
        {
            if (sprintTime <= 2 || sprintTime <= maxSprintTime)
            {
                if (!onCoolDown)
                {
                    sprintTime += Time.deltaTime;
                }
            }
        } 
        
    }

   

    private void FixedUpdate()
    {
        if (!dead)
        {
            //smoothed movement
            smoothedMoveDir = Vector3.SmoothDamp(smoothedMoveDir, moveDir, ref smoothedMoveVelo, 0.1f);
            smoothedMoveDir = CamTransform.forward * moveDir.z + CamTransform.right * moveDir.x;


            rb.velocity = new Vector3(smoothedMoveDir.x * moveSpeed, -3, smoothedMoveDir.z * moveSpeed);
        }
        

    }

    //this handles movement 
    public void SetMoveDirection(Vector3 newDir)
    {
        if (!dead)
        {
            moveDir = newDir;
        }
        
    }

    

    public void startSprint()
    {
        isSprinting = true;
    }

    public void cancelSprint()
    {
        isSprinting = false;
        
    }

    public IEnumerator Cooldown(float maxCoolDown)
    {
        onCoolDown = true;
        yield return new WaitForSeconds(maxCoolDown);
        onCoolDown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Exit"))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("WinArea");
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            dead = true;
            StartCoroutine(LookatDeath());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "hidingSpot")
        {
            inhidingRange = true;
            HidingCam = other.gameObject.GetComponent<CinemachineVirtualCamera>();
        }
        if (other.tag == "CodePanel")
        {
            canusePanel = true;
            codePanelInfo.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CodePanel")
        {
            canusePanel = false;
            codePanelInfo.SetActive(false); 
            
        }
        if (other.tag == "hidingSpot")
        {
            HidingCam = null;
        }
    }
    public IEnumerator LookatDeath()
    {
        heartBeat.clip = heartbeatF;
        heartBeat.Play();
        cineCam.Priority = 100;
        sound.Play();
        yield return new WaitForSeconds (3);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
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
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                
                gameObject.transform.position = HidingCam.transform.position;
                
            }
        }
        else if (isHiding)
        {
            HidingCam.Priority = 9;
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            isHiding = false;
            gameObject.transform.position = previousPos;
            flashlight.SetActive(true);
            HidingCam = null;
            hitbox.SetActive(true);
        }

        
    }

    public void leftClick()
    {
        rightClick();
    }

    public void interactable()
    {
        isTalking = true;
    }
    public void notInteractable()
    {
        isTalking = false;
    }
        
    

   


    public void rightClick()
    {
        if (canusePanel)
        {
            codePanelInfo.SetActive(true);
            f++;
        }
        else
        {
            Debug.Log(f);
        }
    
    }
    public void stopWalkingSound()
    {
       walking.enabled = false;
    }

    public void walkingSound()
    { 
        walking.enabled = true;
    }
}
    

