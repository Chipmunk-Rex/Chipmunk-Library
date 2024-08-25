using System;
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
        public static T GetNext<T>(T enumValue) where T : Enum
        {
            Array array = System.Enum.GetValues(typeof(T));
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (enumValue.Equals(array.GetValue(i)))
                    return (T)array.GetValue(i + 1);
            }
            return (T)array.GetValue(0);
        }
    }
}