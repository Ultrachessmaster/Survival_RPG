﻿using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Survival {
    class Sword : Entity {

        public float Attack { get; private set; }
        public string Name { get; private set; }

        public Sword(Vector2 pos, GameTime gt, float rotation) {
            this.pos = pos;
            Sprite = 2;
            tex = SRPG.SpriteMap;
            hitbox = new HitBox(pos, Vector2.One, this);
            Vector2 f = new Vector2(13f / Eng.tilesize, 5f / Eng.tilesize);
            hitbox.rotation = rotation;
            this.rotation = rotation;
            Timer.AddTimer(DestroySelf, Player.swordWaitTIme, this);

            Attack = 5f;
            Name = "Sword";
        }

        public override void PostUpdate() {
            var damagecols = Physics.GetCollisions<IDamagable>(hitbox);
            foreach(HitBox col in damagecols) {
                var dmg = (IDamagable)col.entity;
                if(dmg is Player)
                    continue;
                dmg.Damage(Attack);
            }

        }

        void DestroySelf() {
            Area.RemoveEntity(this);
            Physics.RemoveCollider(hitbox);
        }
    }
}
