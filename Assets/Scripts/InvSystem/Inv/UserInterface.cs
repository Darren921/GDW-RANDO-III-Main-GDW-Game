using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    [SerializeField] protected Transform parentTransform ;
    public InventoryObj inventory;
    protected Dictionary<GameObject, InventoryObj.InventorySlot > slotsOnInterface = new Dictionary<GameObject, InventoryObj.InventorySlot>();
    protected Dictionary<InventoryObj.InventorySlot, EquipmentBase> _equipmentBases = new Dictionary<InventoryObj.InventorySlot, EquipmentBase>();

    // Start is called before the first frame update
    void Awake()
    {
        for (var i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
           inventory.GetSlots[i].OnBeforeUpdate += OnSlotUpdate;
           inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;

        }
        
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });

    }

    private void Start()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            OnSlotUpdate(inventory.GetSlots[i]);
        }
    }

    private void OnDisable()
    {
        for (var i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnBeforeUpdate -= OnSlotUpdate;
            inventory.GetSlots[i].OnAfterUpdate -= OnSlotUpdate;

        }
    }

    private void  OnSlotUpdate(InventoryObj.InventorySlot _slot)
    {
        if (_slot.item.Id >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().enabled = true;
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObj.data.UiDisplay;
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? ""  : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().enabled = false;
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "" ;

        }
    }


    // Update is called once per frame
    void Update()
    {
     
    }


    protected abstract void CreateSlots();
    

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    protected void OnEnter( GameObject obj )
    {

        MouseData.slotHoveredOver = obj;

    }

    protected void OnExit( GameObject obj )
    {
        MouseData.slotHoveredOver = null;

        
    }

    protected void OnDragStart( GameObject obj )
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    private GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            if (slotsOnInterface[obj].item.Id >= 0)
            {
                var img = tempItem.AddComponent<Image>();
                img.sprite = slotsOnInterface[obj].ItemObj.data.UiDisplay;
                img.raycastTarget = false;
            }
        }
       return tempItem;
    }

    private void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    private void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    protected void OnDragEnd( GameObject obj )
    {
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver is null)
        {
            // use this for dropping/ removing items
        }

        if (MouseData.slotHoveredOver)
        {
            var mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];

            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    protected void OnDrag( GameObject obj )
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    private static class MouseData
    {
        public static UserInterface interfaceMouseIsOver;
        public static GameObject tempItemBeingDragged;
        public static GameObject slotHoveredOver;
    }
}
