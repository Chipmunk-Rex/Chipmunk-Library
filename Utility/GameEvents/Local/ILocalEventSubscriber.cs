namespace Chipmunk.Library.Utility.GameEvents.Local
{
    public interface ILocalEventSubscriber<TEvent> where TEvent : ILocalEvent
    {
        void OnLocalEvent(TEvent eventData);
    }
}
