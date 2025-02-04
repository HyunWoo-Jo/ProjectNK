using UnityEngine;
using UnityEngine.EventSystems;

namespace N.UI
{
    public static class EventTriggerUtills 
    {
       public static void AddEventButton(this EventTrigger eventTrigger, EventTrigger.Entry entry, string entryClassMethodName) {
            eventTrigger.triggers.Add(entry);
#if UNITY_EDITOR
            var pointChker =  eventTrigger.gameObject.GetComponent<ButtonEntryPointChker>();
            if(pointChker == null) pointChker = eventTrigger.gameObject.AddComponent<ButtonEntryPointChker>();
            pointChker.AddEntry(entryClassMethodName, entry.eventID);
#endif
        }
    }
}
