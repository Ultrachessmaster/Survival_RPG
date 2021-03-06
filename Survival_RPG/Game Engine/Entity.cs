﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Entity {
        public Texture2D tex;
	    public int Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        private int sprite;
        protected int width = 16;
        protected int height = 16;
        
        public virtual Vector2 pos { get; set; }
        public Vector2 scale = new Vector2(1, 1);
        public float rotation = 0f;
        public virtual bool Enabled {
            get {
                return enabled;
            }
            set {
                enabled = value;
                if (hitbox != null)
                    hitbox.enabled = false;
            }
        }
        bool enabled;

        public HitBox hitbox;
        
        public string Tag { get { return tag; } }
        protected string tag = "";

        protected bool visible = true;
        public virtual void Update(GameTime gt)
        {

        }
        public virtual void PostUpdate() {

        }

        public virtual void Draw (SpriteBatch sb, int pxlratio, int tilesize, Color col)
        {
            if (Enabled && visible)
            {
                int xsource = (Sprite % (tex.Width / tilesize)) * tilesize;
                int ysource = (int)Math.Floor((decimal)(Sprite) / (tex.Height / tilesize)) * tilesize;

                Rectangle sourcerect = new Rectangle(xsource, ysource, tilesize, tilesize);
                
                Rectangle destrect = new Rectangle((int)Math.Round((pos.X - Camera.X) * pxlratio), (int)Math.Round((pos.Y - Camera.Y) * pxlratio), width * pxlratio, height * pxlratio);
                sb.Draw(tex, destrect, sourcerect, col, rotation, new Vector2((tilesize/2), (tilesize / 2)), SpriteEffects.None, 0f);
            }
        }
    }
}
