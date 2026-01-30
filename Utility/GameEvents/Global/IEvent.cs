using Chipmunk.Library.Utility.GameEvents.Global;

namespace Chipmunk.Library.Utility.GameEvents.Global
{
    public interface IEvent
    {
        void Raise()
        {
            EventBus.Raise(this);
        }
    }
}