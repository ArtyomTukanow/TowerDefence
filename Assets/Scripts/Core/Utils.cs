using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class Utils
    {
        public static Vector3 Set(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
        }
        
        public static T MinBy<T>(this IEnumerable<T> list, Func<T, float> compare)
        {
            float? min = null;
            T result = default;
            
            foreach (var element in list)
            {
                var nextMin = compare.Invoke(element);
                if (min == null || min > nextMin)
                {
                    min = nextMin;
                    result = element;
                }
            }

            return result;
        }
        
        public static float GetDistance2DTo(this Vector3 fromPos, Vector3 toPos)
        {
            return new Vector2(fromPos.x - toPos.x, fromPos.z - toPos.z).magnitude;
        }

        public static float NearestRotate(this float rotation)
        {
            rotation %= 360;
            if (rotation > 180)
                rotation -= 360;
            else if (rotation < -180)
                rotation += 360;
            return rotation;
        }
    }
}