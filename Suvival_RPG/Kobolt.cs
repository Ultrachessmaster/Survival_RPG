using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Kobolt : Entity, IDamagable {

        public float Health { get; private set; }
        bool invincible = false;

        HitBox hb;

        float noticedistance = 9f * Eng.tilesize;
        float speed = 0.6f;

        public Kobolt(Vector2 pos) {
            this.pos = pos;
            Sprite = 1;
            tex = SRPG.SpriteMap;
            hb = new HitBox(pos, new Vector2(Eng.tilesize), this);
            Physics.AddCollider(hb);
            Health = 15f;
        }

        public override void Update(GameTime gt) {
            var player = ERegistry.GetEntity<Player>();
            if(player != null && Vector2.Distance(player.pos, pos) < noticedistance && !invincible) {
                var dir = player.pos - pos;
                dir.Normalize();
                hb.vel = dir * speed;
            }
            if (Health <= 0) {
                ERegistry.RemoveEntity(this);
                ERegistry.AddEntity(new Food(pos, FoodType.Kobolt_Meat));

                Player.XP += 10;
            }
        }

        public override void PostUpdate() {
            var damagehb = new HitBox(hb.pos, hb.size + Vector2.One, this);
            var playercol = Physics.GetCollision<Player>(damagehb);
            if(playercol != null) {
                var player = (Player)playercol.entity;
                player.Damage(5f);
            }
            hb.vel = Vector2.Zero;
            pos = hb.pos;
        }

        public void Damage(float damage) {
            if(!invincible) {
                Health -= damage;

                Timer.AddTimer(() => invincible = false, 0.5f, enabled);
            }
            invincible = true;
        }
    }
}
