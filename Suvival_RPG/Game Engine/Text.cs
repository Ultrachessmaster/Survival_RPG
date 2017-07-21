using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine {
    public class Text {
        public Vector2 pos;
        public string text;
        public SpriteFont spritefont;
        public Color col;
        public Text(Vector2 pos, string text, SpriteFont spritefont, Color col) {
            this.pos = pos;
            this.text = text;
            this.spritefont = spritefont;
            this.col = col;
        }
        public void Draw(SpriteBatch sb) {
            Vector2 renderpos = new Vector2((int)Math.Round((pos.X - Camera.X) * Eng.pxlsize), (int)Math.Round((pos.Y - Camera.Y) * Eng.pxlsize));
            sb.DrawString(spritefont, text, renderpos, col);
        }
    }
}
