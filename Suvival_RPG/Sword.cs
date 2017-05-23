using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Sword : Entity {

        public float Attack { get; private set; }
        public string Name { get; private set; }

        HitBox hb;

        public Sword(Vector2 pos, GameTime gt) {
            this.pos = pos;
            Sprite = 2;
            tex = SRPG.SpriteMap;
            hb = new HitBox(pos, new Vector2(Eng.pxlsize), this);
            hb.trigger = true;
            Timer.AddTimer(() => enabled.Value = false, Player.WaitTime, enabled);

            Attack = 5f;
            Name = "Sword";
        }

        public override void PostUpdate() {
            var damagecols = Physics.GetCollisions<IDamagable>(hb);
            foreach(HitBox col in damagecols) {
                var dmg = (IDamagable)col.entity;
                if(dmg is Player)
                    continue;
                dmg.Damage(Attack);
            }

        }
    }
}
