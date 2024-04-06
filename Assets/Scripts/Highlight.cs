using UnityEngine;
using UnityEngine.EventSystems; 

[RequireComponent(typeof(EventTrigger))]
public class Highlight : MonoBehaviour
{
    private Outline outline;

    void Start()
    {
       
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

        AddEventTrigger(OnPointerEnter, EventTriggerType.PointerEnter);
        AddEventTrigger(OnPointerExit, EventTriggerType.PointerExit);
    }

    // Method to add an event trigger listener
    private void AddEventTrigger(UnityEngine.Events.UnityAction action, EventTriggerType triggerType)
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry triggerEntry = new EventTrigger.Entry();
        triggerEntry.eventID = triggerType;
        triggerEntry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(triggerEntry);
    }

    // Method to call when the pointer enters the object
    public void OnPointerEnter()
    {
        if (outline != null)
        {
            outline.enabled = true;
            // outline.OutlineColor = Color.red;
            outline.OutlineWidth =5;
        }
    }

    // Method to call when the pointer exits the object
    public void OnPointerExit()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}