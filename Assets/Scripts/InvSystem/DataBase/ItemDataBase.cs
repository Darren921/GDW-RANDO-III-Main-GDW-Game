using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Inv/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObj[] Items;
    public Dictionary<int, ItemObj > GetItem = new Dictionary<int, ItemObj>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].data.Id = i;
            GetItem.Add(i,Items[i]);

        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObj>();
    }
}
