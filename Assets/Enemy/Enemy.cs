using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform _Target;
    [SerializeField] Transform player;
    [SerializeField] AudioSource soundSource;
    [SerializeField] public GameObject startPos;
    NavMeshAgent _Agent;
    bool _IsActive = true;
    public bool _IsHunting = false;
  

    public bool Spotted;
    // Start is called before the first frame update
    void Start()
    {
        _Agent = this.GetComponent<NavMeshAgent>();
        Destination();
        _Target = this.transform;
        _IsActive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Destination();
    }
    private void Destination()
    {

        if (Spotted)
        {
            _Agent.destination = player.position;
        }
        else if (_IsActive)
        {
            StartCoroutine(DesLocation());
            _IsActive = false;
        }
        else if (_IsHunting)
        {
            _Agent.destination = player.position;
            _Agent.speed = 15;
        }
        else if (player.GetComponent<Player>().dead == true)
        {
            _Agent.SetDestination(gameObject.transform.position);
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            
            Spotted = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && Spotted)
        {
            StartCoroutine(countdown());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            soundSource.Play();
        }
    }
    IEnumerator countdown()
    {
        yield return new WaitForSeconds(3);
        Spotted = false;
        Debug.Log("lost");
        StartCoroutine(DesLocation());
    }
    public IEnumerator DesLocation()
    {
        _IsActive = false;
        NavMeshHit hit;
        
        Vector3 BasicDestination = new Vector3(Random.Range(-219, 85),0, Random.Range(-53, 150));
        NavMesh.SamplePosition(BasicDestination, out hit,40 , NavMesh.AllAreas);
        Vector3 Endpoint = hit.position;
        _Agent.SetDestination(Endpoint);
        Debug.Log(_Agent.destination);
        yield return new WaitForSeconds(8);
        _IsActive = true;
        Debug.Log("new location");
    }
}
