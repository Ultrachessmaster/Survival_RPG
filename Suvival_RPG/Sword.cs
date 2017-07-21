using FarseerPhysics.Dynamics;
using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Sword : Entity {

        public float Attack { get; private set; }
        public string Name { get; private set; }

        Body body;

        public Sword(Vector2 pos, GameTime gt, float rotation) {
            this.pos = pos;
            Sprite = 2;
            tex = SRPG.SpriteMap;
            body = FS.CreateBox(new Vector2(13f / Eng.tilesize, 5f / Eng.tilesize), Vector2.Zero, pos, this, BodyType.Static, true);
            Vector2 f = new Vector2(13f / Eng.tilesize, 5f / Eng.tilesize);
            body.Rotation = rotation;
            this.rotation = rotation;
            Timer.AddTimer(DestroySelf, Player.swordWaitTIme, this);

            Attack = 5f;
            Name = "Sword";
        }

        public override void PostUpdate() {
            var damagecols = FS.GetCollisions<IDamagable>(body);
            foreach(Body col in damagecols) {
                var dmg = (IDamagable)col.UserData;
                if(dmg is Player)
                    continue;
                dmg.Damage(Attack);
            }

        }

        void DestroySelf() {
            ERegistry.RemoveEntity(this);
            SRPG.World.RemoveBody(body);
        }
    }
}
