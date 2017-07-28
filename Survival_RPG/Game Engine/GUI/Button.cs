using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class Button
    {
        public XYZ size;
        public XYZ pos;
        Action<int> act0;
        Action<int> act1;
        public int sprite;
        public int id;
        public Button (XYZ s, XYZ p, Action<int> a, int spr, int id)
        {
            size = s;
            pos = p;
            act0 = a;
            sprite = spr;
            this.id = id;
        }

        public Button(XYZ s, XYZ p, Action<int> left, Action<int> right, int spr, int id)
        {
            size = s;
            pos = p;
            act0 = left;
            act1 = right;
            sprite = spr;
            this.id = id;
        }

        bool InHitBox(XYZ point)
        {
            XYZ corner = pos + size;
            bool inx = (point.X < corner.X && point.X > pos.X);
            bool iny = (point.Y < corner.Y && point.Y > pos.Y);
            return inx && iny;
        }

        public void Update()
        {
            var state = Mouse.GetState();
            XYZ point = new XYZ();
            point.X = state.X;
            point.Y = state.Y;
            bool inbox = InHitBox(point);
            bool mouse0clicked = Input.IsMouseButtonPressed(0);
            bool mouse1clicked = Input.IsMouseButtonPressed(1);
            if (mouse0clicked && inbox)
                act0.Invoke(id);
            if (mouse1clicked && inbox && act1 != null)
                act1.Invoke(id);
        }
        public void Draw(SpriteBatch sb, Texture2D tex)
        {
            Rectangle dest = new Rectangle(pos.X, pos.Y, size.X, size.Y);
            int xsource = (sprite % (tex.Width / Eng.tilesize)) * Eng.tilesize;
            int ysource = (int)Math.Floor((decimal)(sprite) / (tex.Width / Eng.tilesize)) * Eng.tilesize;
            Rectangle sourcerect = new Rectangle(xsource, ysource, Eng.tilesize, Eng.tilesize);

            sb.Draw(tex, dest, sourcerect, Color.White);
        }
    }
}
