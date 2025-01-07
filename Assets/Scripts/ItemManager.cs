using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
  public event Action<string, object> OnDictionaryChanged;
  

  public Dictionary<Items.Item, GameObject> itemsList = new Dictionary<Items.Item, GameObject>();

  private void Start()
  {
    
  }
}
