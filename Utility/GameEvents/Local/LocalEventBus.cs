using System;
using System.Collections.Generic;
using System.Reflection;
using Chipmunk.Library.Utility.ComponentContainers;
using UnityEngine;

namespace Chipmunk.Library.Utility.GameEvents.Local
{
    public class LocalEventBus : MonoBehaviour, IContainerComponent
    {
        private static readonly MethodInfo RegisterSubscriberMethod =
            typeof(LocalEventBus).GetMethod(nameof(RegisterSubscriber), BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly Dictionary<Type, Delegate> _events = new();

        public ComponentContainer ComponentContainer { get; set; }

        public void OnInitialize(ComponentContainer componentContainer)
        {
            RegisterInterfaceSubscribers();
        }

        public void Raise<TEvent>(TEvent eventData) where TEvent : ILocalEvent
        {
            if (_events.TryGetValue(typeof(TEvent), out Delegate handler))
            {
                ((Action<TEvent>)handler).Invoke(eventData);
            }
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : ILocalEvent
        {
            if (handler == null)
                return;

            Type eventType = typeof(TEvent);
            if (_events.TryGetValue(eventType, out Delegate existingHandler))
            {
                _events[eventType] = Delegate.Combine(existingHandler, handler);
            }
            else
            {
                _events[eventType] = handler;
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : ILocalEvent
        {
            if (handler == null)
                return;

            Type eventType = typeof(TEvent);
            if (_events.TryGetValue(eventType, out Delegate existingHandler) == false)
                return;

            Delegate updated = Delegate.Remove(existingHandler, handler);
            if (updated == null)
            {
                _events.Remove(eventType);
            }
            else
            {
                _events[eventType] = updated;
            }
        }
        private void RegisterInterfaceSubscribers()
        {
            if (ComponentContainer == null)
                return;

            MonoBehaviour[] behaviours = ComponentContainer.GetComponentsInChildren<MonoBehaviour>(true);
            for (int i = 0; i < behaviours.Length; i++)
            {
                MonoBehaviour behaviour = behaviours[i];
                if (behaviour == null || behaviour == this)
                    continue;

                Type[] interfaces = behaviour.GetType().GetInterfaces();
                for (int interfaceIndex = 0; interfaceIndex < interfaces.Length; interfaceIndex++)
                {
                    Type interfaceType = interfaces[interfaceIndex];
                    if (interfaceType.IsGenericType == false ||
                        interfaceType.GetGenericTypeDefinition() != typeof(ILocalEventSubscriber<>))
                        continue;

                    Type eventType = interfaceType.GetGenericArguments()[0];
                    if (typeof(ILocalEvent).IsAssignableFrom(eventType) == false)
                        continue;

                    MethodInfo registerMethod = RegisterSubscriberMethod.MakeGenericMethod(eventType);
                    registerMethod.Invoke(this, new object[] { behaviour });
                }
            }
        }

        private void RegisterSubscriber<TEvent>(ILocalEventSubscriber<TEvent> subscriber)
            where TEvent : ILocalEvent
        {
            if (subscriber == null)
                return;

            Subscribe<TEvent>(subscriber.OnLocalEvent);
        }
    }
}
