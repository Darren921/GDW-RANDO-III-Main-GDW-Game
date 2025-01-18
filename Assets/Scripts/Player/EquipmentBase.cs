using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipmentBase : MonoBehaviour
{
    [SerializeField]EquipmentObj equipmentObj;
    public float MaxLimit { get; private set; }
    internal GameObject baseObj;
    internal GameObject lightObj;
    private float LimitLeft { get; set; }
    internal bool equipped;
    internal bool active;
    internal bool torchActive;
    internal bool checkActive;
    private int refillAmount;
    private Slider slider;
    public EquipmentObj EquipmentObj { get { return equipmentObj; } }
  
    // Start is called before the first frame update
    void Start()
    {
        MaxLimit = equipmentObj.Limit;
        LimitLeft = equipmentObj.Limit;
        refillAmount = equipmentObj.refuel;
        baseObj = gameObject;
        lightObj = FindChildWithNameContaining(baseObj.transform, "Light");
        slider = gameObject.GetComponentInChildren<Slider>();
        baseObj.SetActive(false);
        lightObj.SetActive(false);  
    }
   

    // Update is called once per frame
    void Update()
    {
        slider.value = LimitLeft;
        if (LimitLeft > 0 && active)
        {
            LimitLeft -= Time.deltaTime;
        }
        
        if(LimitLeft < 0)
        {
            LimitLeft = 0;
            active = false;
            lightObj.SetActive(false);
        }
        
    }
    
    public void CheckIfActive()
    {
        if (lightObj == null)
        {
            lightObj = gameObject.transform.Find("Light").gameObject;
        }
         print("working");
        if (equipped )
        {
            active = !active;
            if (LimitLeft > 0)
            {
                if (active && !checkActive)
                {
                    print("Active");
                    if (gameObject.name.Contains("Torch") )
                    {
                        torchActive = true;
                    }
                    checkActive = true;
                    StartCoroutine(CheckCharge()) ;
                    lightObj.SetActive(true);
                }
                else
                {
                    if (gameObject.name.Contains("Torch") )
                    {
                        torchActive = false;
                    }
                    active = false;
                    lightObj.SetActive(false);
                    
                }
            }
            else
            {
                if (gameObject.name.Contains("Torch") )
                {
                    torchActive = false;
                }
                active = false;
                lightObj.SetActive(false);

            }
        }
     
    }
    private IEnumerator CheckCharge()
    {
            yield return new WaitUntil(() => active == false || checkActive == false );
            checkActive = false;
    }


    public void LimitCheck(GameObject other)
    {
        var totalRefillAmount = LimitLeft += refillAmount;
        if (LimitLeft > MaxLimit)
        {
            LimitLeft = MaxLimit;
            return;
        }

        if (LimitLeft < MaxLimit)
        {
            LimitLeft = totalRefillAmount;
            Destroy(other.gameObject);
        }
    }
    public GameObject FindChildWithNameContaining(Transform parent, string substring)
    {
        foreach (Transform child in parent)
        {
            if (child.name.Contains(substring))
            {
                return child.gameObject; 
            }
        }

        return null; 
    }
}
