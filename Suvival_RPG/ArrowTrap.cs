﻿using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suvival_RPG {
    class ArrowTrap : Entity {
        HitBox body;
        float rate = 2f;
        public ArrowTrap(Vector2 pos, float rotation) {
            this.pos = pos;
            this.rotation = rotation;
            Sprite = 4;
            tex = SRPG.SpriteMap;
            body = new HitBox(pos, Vector2.One, this);
            this.rotation = rotation;
            body.rotation = rotation;
            Timer.AddTimer(FireArrow, rate, this);
        }

        void FireArrow() {
            ERegistry.AddEntity(new Arrow(pos, rotation));
            Timer.AddTimer(FireArrow, rate, this);
        }
    }
}
