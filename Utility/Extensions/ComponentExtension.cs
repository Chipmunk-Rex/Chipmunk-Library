using UnityEngine;

namespace Chipmunk.Library.Utility.Extensions
{
    public static class ComponentExtension
    {
        public static T GetOrAddComponent<T>(this Component target) where T : Component
        {
            var component = target.GetComponent<T>();
            if (component == null)
            {
                component = target.gameObject.AddComponent<T>();
            }

            return component;
        }

        public static void GetComponentWhenNull<T>(this Component target, ref T component) where T : Component
        {
            if (component == null)
                component = target.GetComponent<T>();
        }
    }
}