using UnityEngine;

namespace Chipmunk.Library
{
    public class ChipmunkLibrary
    {
        public static T GetComponentWhenNull<T>(GameObject gameObject, ref T component) where T : Component
        {
            if (component == null)
                component = gameObject.GetComponent<T>();
            return component;
        }
        public static T GetComponentWhenNull<T>(Component targetCompo, ref T component) where T : Component
        {
            if (component == null)
                component = targetCompo.GetComponent<T>();
            return component;
        }
    }
}