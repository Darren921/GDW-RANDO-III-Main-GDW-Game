using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

public class MonsterAIPathfinding : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject player;
    [SerializeField] GameObject forwardFace;
    [SerializeField] bool LOS;
    [SerializeField] Vector3 facing;
    [SerializeField] Camera cam;
    [SerializeField] bool Spotted;
    [SerializeField] bool RoamDestination;
    [SerializeField] bool CanRoam;
    [SerializeField] public GameObject[] positions;

    // Start is called before the first frame update
    void Start()
    {
        CanRoam = true;
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Debug.DrawRay(gameObject.transform.position, forwardFace.transform.position - gameObject.transform.position);
        GetLineOfSight();
        
        CreateRays();
        if (Spotted)
        {
            CanRoam = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            CanRoam = true;
            if (Vector3.Distance(gameObject.transform.position, agent.destination) < 3)
            {
                RoamDestination = true;
            }
            else
            {
                RoamDestination=false;
            }
        }
        if (RoamDestination) 
        {
            if (CanRoam) 
            {
                NewRoamDestination();
                RoamDestination = false;
            
            }
        }
        


    }
    void GetLineOfSight()
    {
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Wall", "Player");
        if (Physics.Raycast(gameObject.transform.position, player.transform.position - gameObject.transform.position, out hit, layerMask))
        {
            if (hit.transform.tag == "Player")
            {
                Debug.DrawRay(gameObject.transform.position, player.transform.position - gameObject.transform.position, Color.green);
                LOS = true;
            }
            else
            {
                Debug.DrawRay(gameObject.transform.position, player.transform.position - gameObject.transform.position, Color.red);
                LOS = false;
            }
        }
    }

    void CreateRays()
    {
        if (LOS) 
        {
            facing = cam.WorldToScreenPoint(player.transform.position);
            if (facing.z > 0)
            {
                Spotted = true;
            }
            else
            {
                Spotted = false; 
            }
            

        }
        else
        {
            Spotted = false;
        }
    }
    void NewRoamDestination()
    {
        int transform = UnityEngine.Random.Range(0, positions.Length);
        
        agent.SetDestination(positions[transform].transform.position);

    }

}
