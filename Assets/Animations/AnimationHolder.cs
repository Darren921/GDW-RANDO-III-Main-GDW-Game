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
    [SerializeField] GameObject particle;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip cutsceneSFX;

    [SerializeField] bool Skip;
    private void Start()
    {
        Skip = false;
        _frostTexture.SetFloat("_Opacity", -1);

        audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(GameScene);
        }
    }
    public void OnStart()
    {
        audioSource.clip = cutsceneSFX;
        audioSource.Play();
        StartCoroutine(PlayCutsceneMainMenu());
    }
    IEnumerator PlayCutsceneMainMenu()
    {
        Skip = true;
        animator.Play("MenuTransition");
        PlayerAnim.Play("Move");
        
        yield return new WaitForSeconds(36.15f);
        door.GetComponent<Animator>().Play("Slide");
        yield return new WaitForSeconds(4);
        particle.SetActive(true);
        yield return new WaitForSeconds(3);
        
        
        Fade.Play("Fade out");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(GameScene);
        
    }
}
