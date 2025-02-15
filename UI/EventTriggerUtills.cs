using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace N.UI
{
    public static class EventTriggerUtills 
    {
       public static void AddEventButton(this EventTrigger eventTrigger, EventTrigger.Entry entry, string entryClassMethodName) {
            eventTrigger.triggers.Add(entry);
#if UNITY_EDITOR
            var pointChker =  eventTrigger.gameObject.GetComponent<EditorButtonEntryPointChker>();
            if(pointChker == null) pointChker = eventTrigger.gameObject.AddComponent<EditorButtonEntryPointChker>();
            pointChker.AddEntry(entryClassMethodName, entry.eventID);
#endif
        }
        public static void AddEventButton(this EventTrigger eventTigger, EventTriggerType type, Action action, string entryClassMethodName) {
            EventTrigger.Entry entry = new();
            entry.eventID = type;
            entry.callback.AddListener((e) => { action(); });
            AddEventButton(eventTigger, entry, entryClassMethodName);
        }
    }
}
