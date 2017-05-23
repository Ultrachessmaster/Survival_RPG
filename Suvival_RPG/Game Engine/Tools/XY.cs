using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public struct XYZ
    {
        public int X;
        public int Y;
        public int Z;

        public XYZ(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            Z = 0;
        }

        public XYZ(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public float Magnitude ()
        {
            double z = Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2);
            return (float)Math.Sqrt(z);
        }

        public static XYZ operator +(XYZ a, XYZ b)
        {
            return new XYZ(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static XYZ operator +(XYZ a, int b)
        {
            return new XYZ(a.X + b, a.Y + b, a.Z + b);
        }
        public static XYZ operator -(XYZ a, XYZ b)
        {
            return new XYZ(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static XYZ operator -(XYZ a)
        {
            return new XYZ(-a.X, -a.Y, -a.Z);
        }
        public static XYZ operator -(XYZ a, int b)
        {
            return new XYZ(a.X - b, a.Y - b, a.Z - b);
        }
        public static XYZ Zero { get { return new XYZ(); } }
        public static bool operator ==(XYZ a, XYZ b)
        {
            return (a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }
        public static bool operator !=(XYZ a, XYZ b)
        {
            return (a.X != b.X || a.Y != b.Y || a.Z != b.Z);
        }
        public static float Distance(XYZ a, XYZ b)
        {
            var distsq = Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2);
            return (float)Math.Sqrt(distsq);
        }

    }
}
