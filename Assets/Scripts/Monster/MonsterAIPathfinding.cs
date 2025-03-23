using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

public class MonsterAIPathfinding : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float walkValue;
    [SerializeField] float RunValue;
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




    private Coroutine footstepCoroutine;
    private bool spotCue = true;

    private string metal = "Monster Metal Footstep";
    private string stone = "Monster Stone Footstep";
    private string marble = "Monster Marble Footstep";
    private string carpet = "Monster Carpet Footstep";

    private string currentFootstep;  
    private string lastFootstep;

    float lastSpotted = 0f;
    float spotCD = 10f;

    // Start is called before the first frame update
    void Start()
    {
        CanRoam = true;
        agent = gameObject.GetComponent<NavMeshAgent>();

        StartCoroutine(DelayWalkingSound(0.15f));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (agent.speed == 0 )
        {
            if (walkValue > 0)
            {
                walkValue -= 0.1f;
            }
            if (RunValue > 0)
            {
                RunValue -= 0.1f;
            }
        }
        if (agent.speed == 10)
        {
            if (walkValue < 1)
            {
                walkValue += 0.1f;
            }
            if (RunValue > 0)
            {
                RunValue -= 0.1f;
            }

        }
        if (agent.speed == 14)
        {
            if (walkValue > 0 )
            {
                walkValue -= 0.1f;
            }
            if ( RunValue < 1 )
            {

                RunValue += 0.1f;
            }
        }
        if (Spotted)
        {
            agent.speed = 14;
        }
        else
        {
            agent.speed = 10;
        }
        animator.SetLayerWeight(1, walkValue);
        animator.SetLayerWeight(2, RunValue);

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


        DetectTerrain();
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
                Debug.Log("Spotted");

                if (Spotted && spotCue && Time.time >= lastSpotted + spotCD)
                {
                    spotCue = false;
                    AudioManager.Instance.PlayMonsterSingleSFX("Monster Spotted Sound");
                    AudioManager.Instance.PlaySpotCueSFX("Spot Cue Sound");
                    lastSpotted = Time.time;
                }
            }
            else
            {
                spotCue = true;
                Spotted = false;
            }
            

        }
        else
        {
            spotCue = true;
            Spotted = false;
        }
    }
    void NewRoamDestination()
    {
        int transform = UnityEngine.Random.Range(0, positions.Length);
        
        agent.SetDestination(positions[transform].transform.position);

    }

    public void DetectTerrain()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            // Debug.Log($"Hit: {hit.collider.name}, Tag: {hit.collider.tag}");

            switch (hit.collider.tag)
            {
                case "MetalFloor":
                    currentFootstep = metal;
                    break;
                case "StoneFloor":
                    currentFootstep = stone;
                    break;
                case "MarbleFloor":
                    currentFootstep = marble;
                    break;
                case "CarpetFloor":
                    currentFootstep = carpet;
                    break;
            }

            if (lastFootstep != currentFootstep)
            {
                if (footstepCoroutine != null)
                {
                    StopCoroutine(footstepCoroutine);
                }
                AudioManager.Instance.StopMonsterFootstepSFX(lastFootstep);
                StartCoroutine(DelayWalkingSound(0.15f));

                lastFootstep = currentFootstep;
            }

        }
    }

    private IEnumerator DelayWalkingSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        AudioManager.Instance.StopMonsterFootstepSFX(lastFootstep);
        AudioManager.Instance.PlayMonsterFootstepSFX(currentFootstep);
    }
}
