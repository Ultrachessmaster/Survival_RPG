using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
    class XY {
        public int X;
        public int Y;

        public XY(int X, int Y) {
            this.X = X;
            this.Y = Y;
        }

        public XY() {
            X = 0;
            Y = 0;
        }

        public float Magnitude() {
            double z = Math.Pow(X, 2) + Math.Pow(Y, 2);
            return (float)Math.Sqrt(z);
        }

        public static XY operator +(XY a, XY b) {
            return new XY(a.X + b.X, a.Y + b.Y);
        }
        public static XY operator +(XY a, int b) {
            return new XY(a.X + b, a.Y + b);
        }
        public static XY operator -(XY a, XY b) {
            return new XY(a.X - b.X, a.Y - b.Y);
        }
        public static XY operator -(XY a) {
            return new XY(-a.X, -a.Y);
        }
        public static XY operator -(XY a, int b) {
            return new XY(a.X - b, a.Y - b);
        }
        public static XY Zero { get { return new XY(); } }
        public static XY Right { get { return new XY(1, 0); } }
        public static XY Left { get { return new XY(-1, 0); } }
        public static XY Up { get { return new XY(0, 1); } }
        public static XY Down { get { return new XY(0, -1); } }
        public static bool operator ==(XY a, XY b) {
            return (a.X == b.X && a.Y == b.Y);
        }
        public static bool operator !=(XY a, XY b) {
            return (a.X != b.X || a.Y != b.Y);
        }
        public static float Distance(XY a, XY b) {
            var distsq = Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
            return (float)Math.Sqrt(distsq);
        }
    }
}
