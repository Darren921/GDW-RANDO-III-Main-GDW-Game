using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRenderer : MonoBehaviour
{
    [SerializeField] FullScreenPassRendererFeature render1;
    [SerializeField] FullScreenPassRendererFeature render2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void num1()
    {
        render1.SetActive(true);
        render2.SetActive(false);
    }
    public void num2()
    {
        render2 .SetActive(true);
        render1.SetActive(false);
    }
}
