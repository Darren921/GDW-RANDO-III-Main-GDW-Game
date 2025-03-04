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

    private CapsuleCollider _capsuleCollider;

    private bool isWalking = false;
    private Coroutine footstepCoroutine;

    private string metal = "Metal Footstep Sound";
    private string stone = "Stone Footstep Sound";

    private string currentFootstep;

    private string lastFootstep;


    //Item switching 
    [Header("Item switching ")]

    private int CurrentItem;
    
    public static bool isDead;
    private PlayerHotbar _playerHotbar;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerHotbar = GetComponent<PlayerHotbar>();
        _playerMovement = GetComponent<PlayerMovement>(); 
        isDead = false;
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        _enemy = FindFirstObjectByType<Enemy>();
        playerCam = gameObject.GetComponentInChildren<Camera>();
        InputManager.Init(this);
        InputManager.EnableInGame();
        Cursor.lockState = CursorLockMode.Locked;

        currentFootstep = metal;
        lastFootstep = currentFootstep;
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

        DetectTerrain();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Exit")) return;
        SceneManager.LoadScene("DeathScreen");
        Cursor.lockState = CursorLockMode.None;
        isDead = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        dead = true;
        StartCoroutine(LookAtDeath("Monster"));
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

    public static IEnumerator LookAtDeath(string deathmessage)
    {
        //heartBeat.clip = heartbeatF;
        //heartBeat.Play();
        //cineCam.Priority = 100;
        //sound.Play();
        //yield return new WaitForSeconds(3);
        InputManager.DisableInGame();
        print($"death by {deathmessage}");
        yield return null;
        Cursor.lockState = CursorLockMode.None;
        isDead = true;
        SceneManager.LoadScene("DeathScreen");
    }

    private void DetectTerrain()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            switch (hit.collider.tag)
            {
                case "MetalFloor":
                    currentFootstep = metal;
                    break;

                case "StoneFloor":
                    currentFootstep = stone;
                    break;
            }

            if (lastFootstep != currentFootstep)
            {
                if (footstepCoroutine != null)
                {
                    StopCoroutine(footstepCoroutine);
                }
                StartCoroutine(DelayWalkingSound(0.15f));

                lastFootstep = currentFootstep;
            }

        }
    }

    public void walkingSound()
    {
        if (!isWalking)
        {
            isWalking = true;
            footstepCoroutine = StartCoroutine(DelayWalkingSound(0.15f));
        }
    }

    public void stopWalkingSound()
    {
        if (isWalking)
        {
            isWalking = false;
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
            }
            StartCoroutine(DelayStopWalkingSound(0.15f));
        }
    }

    private IEnumerator DelayStopWalkingSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isWalking)
        {
            AudioManager.Instance.StopSFX(currentFootstep);
        }
    }

    private IEnumerator DelayWalkingSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isWalking)
        {
            AudioManager.Instance.StopSFX(lastFootstep);
            AudioManager.Instance.PlaySFX(currentFootstep);
        }
    }

}
    

