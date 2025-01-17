using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBase : MonoBehaviour
{
    [SerializeField]EquipmentObj equipmentObj;
    private float Limit;
    internal GameObject baseObj;
    internal Light lightObj;
    private float LimitLeft;
    internal bool equipped;
    internal bool active;

    internal bool checkActive;
    // Start is called before the first frame update
    void Start()
    {
        LimitLeft = equipmentObj.Limit;
        baseObj = gameObject;
        lightObj = baseObj.gameObject.GetComponentInChildren<Light>();
        baseObj.SetActive(false);
        lightObj.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LimitLeft > 0 && active)
        {
            LimitLeft -= Time.deltaTime;
        }
        
        if(Limit < 0) 
        { 
            active = false;
        }
    }
    
    public void CheckIfActive()
    {
        if (lightObj == null)
        {
            lightObj = gameObject.GetComponentInChildren<Light>();
        }
        // print("working");
        if (equipped )
        {
            active = !active;
            if (LimitLeft > 0)
            {
                if (active && !checkActive)
                {
                    checkActive = true;
                    lightObj.enabled = true;
                    StartCoroutine(CheckCharge()) ;
                }
                else
                {
                    lightObj.enabled = false;
                }
            }
            else
            {
                lightObj.enabled = false;
            }
        }
     
    }
    private IEnumerator CheckCharge()
    {
            yield return new WaitUntil(() => active == false );
            checkActive = false;
    }
    
    
}
