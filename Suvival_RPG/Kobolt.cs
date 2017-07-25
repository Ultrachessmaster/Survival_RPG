using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Kobolt : Entity, IDamagable {

        public override Vector2 pos {
            get {
                return base.pos;
            }

            set {
                base.pos = value;
                body.pos = value;
            }
        }

        public float Health { get; private set; }
        bool invincible = false;

        public HitBox body;

        float noticedistance = 9f * Eng.tilesize;
        float speed = 0.5f;

        public Kobolt(Vector2 pos) {
            body = new HitBox(pos, new Vector2(4f, 10f), this);
            this.pos = pos;
            Sprite = 1;
            tex = SRPG.SpriteMap;
            Health = 15f;
        }

        public override void Update(GameTime gt) {
            var player = ERegistry.GetEntity<Player>();
            if(player != null && Vector2.Distance(player.pos, pos) < noticedistance && !invincible) {
                var dir = player.pos - pos;
                dir.Normalize();
                body.vel = dir * speed;
            }
            if (Health <= 0) {
                ERegistry.RemoveEntity(this);
                Physics.RemoveCollider(body);
                if(Rng.r.Next(0, 4) == 0)
                    ERegistry.AddEntity(new Food(pos, FoodType.Kobolt_Meat));

                Player.XP += 10;
            }
        }

        public override void PostUpdate() {
            body.vel = Vector2.Zero;
            pos = body.pos;
            body.size += new Vector2(2, 2);
            body.UpdatePolygon();
            var playerbody = Physics.GetCollision<Player>(body);
            body.size -= new Vector2(2, 2);
            body.UpdatePolygon();
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
