using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationHolder : MonoBehaviour
{
    [SerializeField] GameObject deadmonster;
    [SerializeField] GameObject othermonster;
    [SerializeField] Animator gooddoor;
    [SerializeField] GameObject fade;
    public void Death()
    {
        othermonster.SetActive(false);
        deadmonster.SetActive(true);
        StartCoroutine(daaaaaa());
    }

    IEnumerator daaaaaa()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }
    public void life()
    {
        gooddoor.Play("Door Open");
    }
    public void limbnb()
    {
        fade.SetActive(true);
        StartCoroutine(fadeAway());
    }

    IEnumerator fadeAway()
    {
        yield return new WaitForSeconds (2);
        SceneManager.LoadScene("NarrativeTest");
    }
}
