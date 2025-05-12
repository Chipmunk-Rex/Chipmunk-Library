using UnityEngine;

namespace Chipmunk.Library.Utility.Extensions
{
    public static class TransformExtension
    {
        public static void SetPositionX(this Transform transform, float x)
        {
            Vector3 position = transform.position;
            position.x = x;
            transform.position = position;
        }
        public static void SetPositionY(this Transform transform, float y)
        {
            Vector3 position = transform.position;
            position.y = y;
            transform.position = position;
        }
        public static void SetPositionZ(this Transform transform, float z)
        {
            Vector3 position = transform.position;
            position.z = z;
            transform.position = position;
        }
        public static void DestroyAllChild(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (child != null)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
        public static void DestroyAllChildImmediate(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (child != null)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}