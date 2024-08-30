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
        public static T GetComponentWithChild<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out T compo))
            {
                return compo;
            }
            else
            {
                T component = null;
                foreach (Transform child in gameObject.transform)
                {
                    component = GetComponentWithChild<T>(child.gameObject);
                    if (component != null)
                    {
                        return component;
                    }
                }
                return component;
            }
        }
        public static T GetComponentWithChild<T>(Component parent) where T : Component
        {
            if (parent.TryGetComponent<T>(out T compo))
            {
                return compo;
            }
            else
            {
                T component = null;
                foreach (Transform child in parent.transform)
                {
                    component = GetComponentWithChild<T>(child.gameObject);
                    if (component != null)
                    {
                        return component;
                    }
                }
                return component;
            }
        }
        public static T GetComponenetWhenNullChild<T>(GameObject gameObject, ref T component) where T : Component
        {
            if (component == null)
            {
                component = GetComponentWithChild<T>(gameObject);
            }
            return component;
        }
        public static T GetComponenetWhenNullChild<T>(Component targetCompo, ref T component) where T : Component
        {
            if (component == null)
            {
                component = GetComponentWithChild<T>(targetCompo);
            }
            return component;
        }
        public static T GetComponentWithParent<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out T compo))
            {
                return compo;
            }
            else
            {
                T component = null;
                if (gameObject.transform.parent != null)
                {
                    component = GetComponentWithParent<T>(gameObject.transform.parent.gameObject);
                    if (component != null)
                    {
                        return component;
                    }
                }
                return component;
            }
        }
        public static T GetComponentWithParent<T>(Component targetCompo) where T : Component
        {
            return GetComponentWithParent<T>(targetCompo.gameObject);
        }
    }
}