using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// If in doubt, ask Mr. Blue (or just skip the middleman and watch/read the Unity tutorial: https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system )
// Drop this on an empty object in the scene to have access to it.
// Sample of starting to listen: EventManager.StartListening("SHOW_RESTART_BUTTON", displayRestartButton);
// Sample of firing off an event: EventManager.TriggerEvent("SHOW_RESTART_BUTTON");

public class EventManager : MonoBehaviour
{

    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {

            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("Need the EventManager script on an object in the scene.");
                }
                else
                {
                    eventManager.ManualInit();
                }
            }

            return eventManager;
        }
    }

    void ManualInit()
    {
        if (this.eventDictionary == null)
        {
            this.eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (instance.eventDictionary == null) return;

        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
