using Engine;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suvival_RPG {
    class ArrowTrap : Entity {
        Body body;
        float rate = 2f;
        public ArrowTrap(Vector2 pos, float rotation) {
            this.pos = pos;
            this.rotation = rotation;
            Sprite = 4;
            tex = SRPG.SpriteMap;
            body = FS.CreateBox(Vector2.One, Vector2.Zero, pos, this, BodyType.Static);
            this.rotation = rotation;
            body.Rotation = rotation;
            Timer.AddTimer(FireArrow, rate, this);
        }

        void FireArrow() {
            ERegistry.AddEntity(new Arrow(pos, rotation));
            Timer.AddTimer(FireArrow, rate, this);
        }
    }
}
