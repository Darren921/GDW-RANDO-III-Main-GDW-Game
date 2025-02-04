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
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
        Debug.Log(cube.transform.position - gameObject.transform.position);
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
                if (gameObject.transform.rotation.y > 90 || gameObject.transform.rotation.y < -90)
                {
                    if (facing.z * gameObject.transform.rotation.y > 1)
                    {
                        facing.z = +facing.z;
                    }
                }
                Debug.Log("True hit");
                if (facing.z  >= 0 )
                {

                    Debug.Log("Go to player");
                    agent.destination = player.transform.position;



                }
                
                
                
                
                else 
                {
                    agent.destination = gameObject.transform.position;
                
                }
            }
            else
            {
                agent.destination = gameObject.transform.position;
            }
            // Debug.Log(cube.transform.position);


            //agent.destination = player.transform.position;
        }
        
        

           
        
    }
}