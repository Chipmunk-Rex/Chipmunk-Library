using UnityEngine;

namespace Chipmunk.Library.Utility.Extensions
{
    public static class VectorExtension
    {
        public static Vector3 ToVector3XZ(this Vector2 vector)
        {
            return new Vector3(vector.x, 0f, vector.y);
        }

        public static Vector2 ToVector2XY(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }
        
    }
}