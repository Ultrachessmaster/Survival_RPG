using FarseerPhysics.Dynamics;
using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Kobolt : Entity, IDamagable {

        public float Health { get; private set; }
        bool invincible = false;

        public Body body;

        float noticedistance = 9f * Eng.tilesize;
        float speed = 1f;
        public Kobolt(Vector2 pos) {
            this.pos = pos;
        }

        public void Load() {
            Sprite = 1;
            tex = SRPG.SpriteMap;
            body = FS.CreateBox(new Vector2(4f / Eng.tilesize, 10f / Eng.tilesize), Vector2.Zero, pos, this, BodyType.Dynamic);
            Health = 15f;
        }

        public override void Update(GameTime gt) {
            var player = ERegistry.GetEntity<Player>();
            if(player != null && Vector2.Distance(player.pos, pos) < noticedistance && !invincible) {
                var dir = player.pos - pos;
                dir.Normalize();
                body.LinearVelocity = dir * speed;
            }
            if (Health <= 0) {
                ERegistry.RemoveEntity(this);
                SRPG.Wld.RemoveBody(body);
                if(Rng.r.Next(0, 4) == 0)
                    ERegistry.AddEntity(new Food(pos, FoodType.Kobolt_Meat));

                Player.XP += 10;
            }
        }

        public override void PostUpdate() {
            body.LinearVelocity = Vector2.Zero;
            pos = body.Position * Eng.tilesize;
            var playerbody = FS.GetCollision<Player>(body);
            if(playerbody != null) {
                var player = (Player)playerbody.UserData;
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
