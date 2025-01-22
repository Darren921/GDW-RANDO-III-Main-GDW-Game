using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    [SerializeField] protected Transform parentTransform ;
    [SerializeField]private Player Player;
    public InventoryObj inventory;
    protected Dictionary<GameObject, InventoryObj.InventorySlot > itemsDisplayed = new Dictionary<GameObject, InventoryObj.InventorySlot>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }
        CreateSlots();
    }



    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }


    protected abstract void CreateSlots();
    

    private void UpdateSlots()
    {
        foreach (var _slot in itemsDisplayed)
        {
            if (_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? ""  : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "" ;

            }
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter( GameObject obj )
    {
        Player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            Player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
    public void OnExit( GameObject obj )
    {
        Player.mouseItem.hoverObj = null;
        Player.mouseItem.hoverItem = null;
        
    }
    public void OnDragStart( GameObject obj )
    {
        print("active");
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }

        Player.mouseItem.obj = mouseObject;
        Player.mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDragEnd( GameObject obj )
    {
        var itemOnMouse = Player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var GetItemObject = inventory.database.GetItem;
        
        if (mouseHoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj],mouseHoverItem.parent.itemsDisplayed[itemOnMouse.hoverObj]);
        }
        else
        {
            
        }
        Destroy(itemOnMouse.obj);
        itemOnMouse.item = null;
    }
    public void OnDrag( GameObject obj )
    {
        if (Player.mouseItem.obj != null)
        {
            Player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    
}
