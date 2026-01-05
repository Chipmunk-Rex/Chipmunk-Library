using Chipmunk.Library.Utility.GameEvents;

namespace Chipmunk.Library.Utility.GameEvents
{
    public static class EventBus<T> where T : IEvent
    {
        public delegate void Event(T evt);

        public static Event OnEvent;
        public static Event OnComplete;

        public static void Raise(T evt)
        {
            OnEvent?.Invoke(evt);
            OnComplete?.Invoke(evt);
        }
    }

    public class EventBus
    {
        public static void Raise<T>(T evt) where T : IEvent => EventBus<T>.Raise(evt);
        public static void Subscribe<T>(EventBus<T>.Event handler) where T : IEvent => EventBus<T>.OnEvent += handler;
        public static void Unsubscribe<T>(EventBus<T>.Event handler) where T : IEvent => EventBus<T>.OnEvent -= handler;
    }

    public class Bus
    {
        public static void Raise<T>(T evt) where T : IEvent => EventBus<T>.Raise(evt);
        public static void Subscribe<T>(EventBus<T>.Event handler) where T : IEvent => EventBus<T>.OnEvent += handler;
        public static void Unsubscribe<T>(EventBus<T>.Event handler) where T : IEvent => EventBus<T>.OnEvent -= handler;
    }
}