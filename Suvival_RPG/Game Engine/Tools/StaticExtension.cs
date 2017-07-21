using Microsoft.Xna.Framework;
using System;

namespace Engine
{
    static class StaticExtension
    {
        public static float Cross(this Vector2 v, Vector2 a)
        {
            return 0;
        }

        public static void SetZero(this Vector2 v)
        {
            v.X = 0;
            v.Y = 0;
        }

        public static void Set(this Vector2 v, float x, float y)
        {
            v.X = x;
            v.Y = y;
        }

        public static float Angle(this Vector2 a, Vector2 b)
        {
            if (float.IsNaN(a.X))
                return (float)Math.PI / 2f;
            var ysign = Math.Sign(a.Y);
            var angle = (float)Math.Acos((double)Vector2.Dot(a, b) / (a.Length() * b.Length()));
            return (ysign == -1 ? -angle
            : angle);
        }

        public static float Range(this Random r, float min, float max)
        {
            return (float)r.NextDouble() * (max - min) + min;
        }
    }
}
