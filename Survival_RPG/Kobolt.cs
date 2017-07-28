using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Survival {
    class Kobolt : Entity, IDamagable {

        public override Vector2 pos {
            get {
                return base.pos;
            }

            set {
                base.pos = value;
                hitbox.pos = value;
            }
        }

        public float Health { get; private set; }
        bool invincible = false;

        float noticedistance = 9f * Eng.tilesize;
        float speed = 0.5f;

        public Kobolt(Vector2 pos) {
            hitbox = new HitBox(pos, new Vector2(4f, 10f), this);
            this.pos = pos;
            Sprite = 1;
            tex = SRPG.SpriteMap;
            Health = 15f;
        }

        public override void Update(GameTime gt) {
            var player = Area.GetEntity<Player>();
            if(player != null && Vector2.Distance(player.pos, pos) < noticedistance && !invincible) {
                var dir = player.pos - pos;
                dir.Normalize();
                hitbox.vel = dir * speed;
            }
            if (Health <= 0) {
                Area.RemoveEntity(this);
                Physics.RemoveCollider(hitbox);
                if(Rng.r.Next(0, 4) == 0)
                    Area.AddEntity(new Food(pos, FoodType.Kobolt_Meat));

                Player.XP += 10;
            }
        }

        public override void PostUpdate() {
            hitbox.vel = Vector2.Zero;
            pos = hitbox.pos;
            hitbox.size += new Vector2(2, 2);
            hitbox.UpdatePolygon();
            var playerbody = Physics.GetCollision<Player>(hitbox);
            hitbox.size -= new Vector2(2, 2);
            hitbox.UpdatePolygon();
            if(playerbody != null) {
                var player = (Player)playerbody.entity;
                player.Damage(5f);
            }
            
        }

        public void Damage(float damage) {
            if(!invincible) {
                Health -= damage;
                invincible = true;
                Timer.AddTimer(() => invincible = false, 0.5f, this);
            }
        }
    }
}
