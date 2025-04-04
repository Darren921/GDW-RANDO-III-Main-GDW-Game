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
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] bool SKIP;
    


    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        animator.enabled = true;
        StartCoroutine(GetOutPod());
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SKIP)
            {
                SKIP = false;
                animator.SetBool("",false);
                Camera.SetBool("", false);
            }
        }
    }

    IEnumerator GetOutPod()
    {
        SKIP = true;
        playerMovement.DisableInput();
        player.dead = true;
        animator.Play("GetOutOfPod");
        Camera.Play("Wake up");
        yield return new WaitForSeconds(14);
        PodDoorAnim.Play("Door Close");
        yield return new WaitForSeconds(10);
        player.dead = false;
        animator.enabled=false;
        playerMovement.EnableInput();
        
    }
}
