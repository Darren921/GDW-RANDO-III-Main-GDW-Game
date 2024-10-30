using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform _Target;
    [SerializeField] Transform player;
    NavMeshAgent _Agent;
    bool Spotted;
    // Start is called before the first frame update
    void Start()
    {
        _Agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Destination();
    }
    private void Destination()
    {
        if (Spotted)
        {
            _Agent.SetDestination(player.position);
        }
        else if  (gameObject.transform.position.x != _Target.position.x && gameObject.transform.position.z != _Target.position.z)
        {
            _Agent.SetDestination(_Target.position);
        
        }
        else
        {
            _Agent.SetDestination(new Vector3(Random.Range(-219,85), Random.Range(0,0), Random.Range(-53,150)));
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Spotted = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && Spotted)
        {
            StartCoroutine(countdown());
        }
    }
    IEnumerator countdown()
    {
        yield return new WaitForSeconds(3);
        Spotted = false;
    }
}
