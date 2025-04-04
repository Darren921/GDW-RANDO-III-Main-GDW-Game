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
    [SerializeField] GameObject skip;
    [SerializeField] Animator panel;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        animator.enabled = true;
        StartCoroutine(GetOutPod());
        skip.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SKIP)
            {
                SKIP = false;
                animator.SetBool("skip", true);
                Camera.SetBool("skip", true);
                player.dead = false;
                animator.enabled = false;
                playerMovement.EnableInput();
                StopCoroutine(GetOutPod());

                PodDoorAnim.Play("Door Close");
                skip.SetActive(false);

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
        panel.Play("Wake");
        yield return new WaitForSeconds(14);
        PodDoorAnim.Play("Door Close");
        yield return new WaitForSeconds(10);
        player.dead = false;
        animator.enabled=false;
        playerMovement.EnableInput();
        
    }
}
