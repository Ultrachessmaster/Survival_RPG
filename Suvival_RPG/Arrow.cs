using Engine;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suvival_RPG {
    class Arrow : Entity {
        Body body;
        float speed = 12f;
        public Arrow(Vector2 pos, float rotation) {
            this.pos = pos;
            body = FS.CreateBox(Vector2.One, Vector2.Zero, pos, this, BodyType.Dynamic, true);
            this.rotation = rotation;
            body.Rotation = rotation;
            Sprite = 5;
            tex = SRPG.SpriteMap;
        }

        public override void Update(GameTime gt) {
            body.LinearVelocity = new Vector2((float)Math.Cos(rotation + (Math.PI/2)), (float)Math.Sin(rotation + (Math.PI/2))) * speed;
        }

        public override void PostUpdate() {
            pos = body.Position * Eng.tilesize;
        }
    }
}
