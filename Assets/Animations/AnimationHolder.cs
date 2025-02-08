using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationHolder : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string GameScene;
    [SerializeField] GameObject door;
    [SerializeField] CinemachineVirtualCamera cam;
    
    

    private void Start()
    {
        
        
    }
    private void Update()
    {
        
    }
    public void OnStart()
    {
        StartCoroutine(PlayCutsceneMainMenu());
    }
    IEnumerator PlayCutsceneMainMenu()
    {
        animator.Play("MenuTransition");
        
        yield return new WaitForSeconds(19);
        door.GetComponent<Animator>().Play("Slide");
        yield return new WaitForSeconds(6);
       
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(GameScene);
        
    }
}
