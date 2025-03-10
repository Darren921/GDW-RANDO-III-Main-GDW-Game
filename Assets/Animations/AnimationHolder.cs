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
    [SerializeField] Animator PlayerAnim;
    [SerializeField] Animator Fade;
    [SerializeField] string GameScene;
    [SerializeField] GameObject door;
    
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] private Material _frostTexture;
    

    private void Start()
    {
        _frostTexture.SetFloat("_Opacity", -1);
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
        PlayerAnim.Play("Move");
        
        yield return new WaitForSeconds(36.15f);
        door.GetComponent<Animator>().Play("Slide");
        yield return new WaitForSeconds(6);
       
        yield return new WaitForSeconds(2);
        Fade.Play("Fade out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(GameScene);
        
    }
}
