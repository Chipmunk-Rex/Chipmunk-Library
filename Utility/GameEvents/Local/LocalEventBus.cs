using System;
using System.Collections.Generic;
using Chipmunk.Library.Utility.ComponentContainers;
using UnityEngine;

namespace Chipmunk.Library.Utility.GameEvents.Local
{
    public class LocalEventBus : MonoBehaviour, IContainerComponent
    {
        public delegate void Event(ILocalEvent localEvent);

        private Dictionary<Type, Event> events = new();
        public ComponentContainer ComponentContainer { get; set; }

        public void OnInitialize(ComponentContainer componentContainer)
        {
        }

        public void Raise<TEvent>(TEvent eventData) where TEvent : ILocalEvent
        {
            Type eventType = typeof(TEvent);
            if (events.TryGetValue(eventType, out Event eventHandlers))
            {
                eventHandlers?.Invoke(eventData);
            }
        }

        public void Subscribe<TEvent>(Event handler) where TEvent : ILocalEvent
        {
            Type eventType = typeof(TEvent);
            if (events.ContainsKey(eventType))
            {
                events[eventType] += handler;
            }
            else
            {
                events[eventType] = handler;
            }
        }

        public void Unsubscribe<TEvent>(Event handler) where TEvent : ILocalEvent
        {
            Type eventType = typeof(TEvent);
            if (events.ContainsKey(eventType))
            {
                events[eventType] -= handler;
            }
        }
    }
}