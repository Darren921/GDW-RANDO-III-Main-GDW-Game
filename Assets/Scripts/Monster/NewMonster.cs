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
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 facing;
    [SerializeField] bool GoToPlayer;
    [SerializeField] bool RandomDestination;
    [SerializeField] float MaxDistance;
    [SerializeField] Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        RandomDestination = true;
    }
    

    // Update is called once per frame
    void Update()
    {
        
//        Debug.Log(cube.transform.position - gameObject.transform.position);
        facing =  (cube.transform.position - gameObject.transform.position );
         LayerMask layer = LayerMask.GetMask("Player", "Wall");
        cube.transform.position = player.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, cube.transform.position - gameObject.transform.position, out hit, layer)) 
        {
          
            Debug.DrawRay(transform.position,cube.transform.position - gameObject.transform.position, Color.red);
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Player")
            {
                
                if (facing.z <= 0 )
                {
                    GoToPlayer = true;

                }
                else
                {
                    GoToPlayer= false;
                }
                
                
               
            }
            else
            {
               GoToPlayer = false;
            }
            // Debug.Log(cube.transform.position);


            //agent.destination = player.transform.position;
        }
        
        if (GoToPlayer == true)
        {
            agent.destination = cube.transform.position;
        }
        else if ( GoToPlayer == false && RandomDestination == true)
        {
            RandomDestination = false;
            Destination();
            
        }
        if (GoToPlayer == false && Vector3.Distance(transform.position, agent.destination) < 3)
        {
            Destination();
        }
        
            
        
        

           
        
    }
    private void Destination()
    {
        direction = Random.insideUnitSphere * MaxDistance;
        direction += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(direction, out hit, Random.Range(0, MaxDistance), 1);
        Vector3 Destination = hit.position;
        
        agent.SetDestination(Destination);

    }
}