using Chipmunk.Library.Utility.GameEvents;

namespace Chipmunk.Library.Utility.GameEvents
{
    public interface IEvent
    {
        void Raise()
        {
            EventBus.Raise(this);
        }
    }
}