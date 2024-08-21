using UnityEngine;

namespace Chipmunk.Library
{
    public class ChipmunkLibrary
    {
        public T GetComponentWhenNull<T>(GameObject gameObject, T component = null) where T : Component
        {
            if (component == null)
                component = gameObject.GetComponent<T>();
            return component;
        }
    }
}