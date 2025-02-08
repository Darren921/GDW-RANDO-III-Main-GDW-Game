using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NewMonster : MonoBehaviour
{
    [SerializeField] GameObject player;
    NavMeshAgent agent;
    [SerializeField] GameObject cube;
   
   
    [SerializeField] bool GoToPlayer;
    
    [SerializeField] float MaxDistance;
    


    [SerializeField] GameObject RayTarget;
    [SerializeField] bool IsSpotted;
    [SerializeField] float range;
    [SerializeField] bool canGo;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 4;
        
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, cube.transform.position - gameObject.transform.position, out hit, Mathf.Infinity);
        if (hit.transform.tag == "Player")
        {
            IsSpotted = true;
        }
        else
        {
            IsSpotted= false;
        }


        Debug.DrawRay(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position, Color.red);

        //        Debug.Log(cube.transform.position - gameObject.transform.position);
        

        cube.transform.position = player.transform.position;
        LayerMask layer = LayerMask.GetMask("Player", "Wall");
        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;
        RaycastHit hit5;
        RaycastHit hit6;
        RaycastHit hit7;
        RaycastHit hit8;
        RaycastHit hit9;

        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position, out hit1, range, layer))
        {
            if (hit.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }
        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position + new Vector3(0.5f,0,0), out hit2, range, layer))
        {
            if (hit2.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }
        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position + new Vector3(1f, 0, 0), out hit3, range, layer))
        {
            if (hit3.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }
        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position + new Vector3(1.5f, 0, 0), out hit4, range, layer))
        {
            if (hit4.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }
        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position + new Vector3(2, 0, 0), out hit5, range, layer))
        {
            if (hit5.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }
        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position - new Vector3(0.5f, 0, 0), out hit6, range, layer))
        {
            if (hit6.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }
        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position - new Vector3(1, 0, 0), out hit7, range, layer))
        {
            if (hit7.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }
        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position - new Vector3(1.5f, 0, 0), out hit8, range, layer))
        {
            if (hit8.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }
        if (Physics.Raycast(gameObject.transform.position, RayTarget.transform.position - gameObject.transform.position - new Vector3(2, 0, 0), out hit9, range, layer))
        {
            if (hit9.transform.tag == "Player")
            {
                GoToPlayer = true;
            }


        }

        if (GoToPlayer == true)
        {
            if (IsSpotted)
            {
                canGo = true;
                agent.destination = cube.transform.position;
                agent.speed = 7;
            }
            else if (canGo)
            {
                GoToPlayer = false;
                IsSpotted = false;
                LastPosition();
            }
        }






    }
    void LastPosition()
    {
        canGo = false;
        agent.SetDestination(cube.gameObject.transform.position);
        agent.speed = 4;
        GoToPlayer = false;
    }
    
}