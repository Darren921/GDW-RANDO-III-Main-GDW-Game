using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Inv/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
     public ItemObj[] ItemObjects;
    
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.Id = i;
           // GetItem.Add(i,Items[i]);

        }
    }

    public void OnBeforeSerialize()
    {
     //   GetItem = new Dictionary<int, ItemObj>();
    }
}
