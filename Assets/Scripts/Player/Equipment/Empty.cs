using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : EquipmentBase
{
    protected internal override void CheckIfActive()
    {
        
    }

    protected internal override IEnumerator CheckCharge()
    {
        yield return null;
    }

    public override void LimitCheck(GameObject other)
    {
    }
}
