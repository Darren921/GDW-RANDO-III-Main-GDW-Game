using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator PodDoorAnim;
    [SerializeField] Animator Camera;
    [SerializeField] Player player;
    
    
    


    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        animator.enabled = true;
        StartCoroutine(GetOutPod());

    }

    IEnumerator GetOutPod()
    {
        
        player.dead = true;
        animator.Play("GetOutOfPod");
        Camera.Play("Wake up");
        yield return new WaitForSeconds(14);
        PodDoorAnim.Play("Door Close");
        yield return new WaitForSeconds(10);
        player.dead = false;
        animator.enabled=false;
        
    }
}
