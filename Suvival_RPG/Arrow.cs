using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suvival_RPG {
    class Arrow : Entity {
        HitBox body;
        float speed = 12f;
        public Arrow(Vector2 pos, float rotation) {
            this.pos = pos;
            body = new HitBox(pos, Vector2.One, this);
            this.rotation = rotation;
            body.rotation = rotation;
            Sprite = 5;
            tex = SRPG.SpriteMap;
        }

        public override void Update(GameTime gt) {
            body.vel = new Vector2((float)Math.Cos(rotation + (Math.PI/2)), (float)Math.Sin(rotation + (Math.PI/2))) * speed;
        }

        public override void PostUpdate() {
            pos = body.pos;
        }
    }
}
