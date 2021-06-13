using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void EventReceiver();

    private static Dictionary<EventsType, EventReceiver> _events;

    public static void SubscribeToEvent(EventsType eventType, EventReceiver listener)
    {
        if (_events == null)
        {
            _events = new Dictionary<EventsType, EventReceiver>();
        }

        if (!_events.ContainsKey(eventType))
        {
            _events.Add(eventType, null);
        }

        _events[eventType] += listener;
    }

    public static void Unsubscribe(EventsType eventType, EventReceiver listener)
    {
        if (_events != null)
        {
            if (_events.ContainsKey(eventType))
            {
                _events[eventType] -= listener; 
            }
        }
    }

    
    public static void TriggerEvent(EventsType eventType) 
    {
        if (_events == null)
        {
            Debug.Log("No events subscribed");
            return;
        }

        if (_events.ContainsKey(eventType))
        {
            if (_events[eventType] != null)
            {
                _events[eventType]();
            }
        }
    }

    public enum EventsType
    {
        Event_Hurt,
        Event_HPUpdate,
        Event_ShieldHit,
        Event_SpeedOn,
        Event_SpeedOff,
        Event_ActivateShield,
        Event_DeactivateShield,
        Event_ActivateMagnet,
        Event_DeactivateMagnet
    }

}
