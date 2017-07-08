using Box2D.XNA;
using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Kobolt : Entity, IDamagable {

        public float Health { get; private set; }
        bool invincible = false;

        Body body;

        float noticedistance = 9f * Eng.tilesize;
        float speed = 1f;

        public Kobolt(Vector2 pos) {
            this.pos = pos;
            Sprite = 1;
            tex = SRPG.SpriteMap;
            body = BoxWrapper.CreateBox(Vector2.One / 4, Vector2.Zero, pos, this);
            Health = 15f;
        }

        public override void Update(GameTime gt) {
            var player = ERegistry.GetEntity<Player>();
            if(player != null && Vector2.Distance(player.pos, pos) < noticedistance && !invincible) {
                var dir = player.pos - pos;
                dir.Normalize();
                body.SetLinearVelocity(dir * speed);
            }
            if (Health <= 0) {
                ERegistry.RemoveEntity(this);
                ERegistry.AddEntity(new Food(pos, FoodType.Kobolt_Meat));

                Player.XP += 10;
            }
        }

        public override void PostUpdate() {
            var playerbody = BoxWrapper.GetCollision<Player>(body);
            if(playerbody != null) {
                var player = (Player)playerbody.GetUserData();
                player.Damage(5f);
            }
            body.SetLinearVelocity(Vector2.Zero);
            pos = body.Position * Eng.tilesize;
        }

        public void Damage(float damage) {
            if(!invincible) {
                Health -= damage;

                Timer.AddTimer(() => invincible = false, 0.5f, this);
            }
            invincible = true;
        }
    }
}
