using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventBus<TEnum> where TEnum : Enum
{
    private Dictionary<TEnum, UnityEvent> eventTable = new Dictionary<TEnum, UnityEvent>();
    public void AddListener(TEnum eventType, UnityAction listener)
    {
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, new UnityEvent());
        }
        eventTable[eventType].AddListener(listener);
    }
    public void RemoveListener(TEnum eventType, UnityAction listener)
    {
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType].RemoveListener(listener);
        }
    }
    public void Publish(TEnum eventType)
    {
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType]?.Invoke();
        }
    }
    public void Invoke(TEnum eventType)
    {
        Publish(eventType);
    }
}
