using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Player : Entity, IDamagable {

        public float Health { get; private set; }
        public float Hunger { get; private set; }
        public static int XP { get; set; }

        Text healthtext;
        Text hungertext;
        Text XPtext;

        public Inventory inventory = new Inventory();

        bool invincible = false;

        float speed = 1.2f;

        HitBox hb;

        bool canmove = true;
        public const float WaitTime = 0.3f;

        SoundEffect hit;

        Vector2 swordoffset = new Vector2(0, 32);

        public Player (Vector2 pos) {
            this.pos = pos;

            Sprite = 0;
            tex = SRPG.SpriteMap;

            Health = 50;
            Hunger = 100;

            hb = new HitBox(pos, new Vector2(Eng.tilesize), this);
            Physics.AddCollider(hb);

            hit = SRPG.CM.Load<SoundEffect>("hit audio 2");

            healthtext = new Text(pos, "", SRPG.Arial, Color.White);
            hungertext = new Text(pos, "", SRPG.Arial, Color.White);
            XPtext     = new Text(pos, "", SRPG.Arial, Color.White);
            UpdateText();
            SRPG.Texts.Add(healthtext);
            SRPG.Texts.Add(hungertext);

            inventory.AddWeapon(new SwordWeapon());
        }

        public override void Update(GameTime gt) {
            if(canmove) {
                if (Input.IsKeyDown(Keys.Up))
                    hb.vel.Y = -speed;
                if (Input.IsKeyDown(Keys.Down))
                    hb.vel.Y = speed;
                if (Input.IsKeyDown(Keys.Left))
                    hb.vel.X = -speed;
                if (Input.IsKeyDown(Keys.Right))
                    hb.vel.X = speed;

                var offset = new Vector2(Math.Sign(hb.vel.X), Math.Sign(hb.vel.Y)) * Eng.tilesize;
                if (offset != Vector2.Zero)
                    swordoffset = offset;
                if (Input.IsKeyPressed(Keys.Z)) {
                    Sword s = new Sword(pos + swordoffset, gt);
                    var correctedoffset = new Vector2(swordoffset.X, -swordoffset.Y);
                    s.rotation = correctedoffset.Angle(-Vector2.UnitX);
                    ERegistry.AddEntity(s);

                    canmove = false;
                    Timer.AddTimer(() => canmove = true, WaitTime, enabled);
                }
            }

            GetItems();
            Hunger -= 0.015f;
            Hunger = Math.Max(Hunger, 0);
            if (Hunger <= 0)
                Health -= 0.05f;

            if (Health <= 0)
                ERegistry.RemoveEntity(this);
        }

        void GetItems() {
            var foods = Physics.GetCollisions<Food>(hb, true);
            foreach(HitBox hit in foods) {
                Food f = (Food)hit.entity;
                inventory.AddFood(f);
                ERegistry.RemoveEntity(f);
            }

            var weapons = Physics.GetCollisions<IWeapon>(hb, true);
            foreach (HitBox hit in weapons) {
                IWeapon w = (IWeapon)hit.entity;
                inventory.AddWeapon(w);
                ERegistry.RemoveEntity(hit.entity);
            }
        }

        void UpdateText() {
            Vector2 textpos = pos + (new Vector2(-Eng.tilesize / 2, -Eng.tilesize * 1.5f));
            XPtext.pos      = textpos - Vector2.UnitY * Eng.tilesize;
            healthtext.pos  = textpos - Vector2.UnitY * Eng.tilesize / 2;
            hungertext.pos  = textpos;
            healthtext.text = "Health: " + Math.Round(Health);
            hungertext.text = "Hunger: " + Math.Round(Hunger);

        }

        public override void PostUpdate() {
            hb.vel = Vector2.Zero;
            pos = hb.pos;
            UpdateText();
            Camera.X = (int)pos.X - (7 * Eng.tilesize);
            Camera.Y = (int)pos.Y - (6 * Eng.tilesize);

            if(Input.IsKeyPressed(Keys.LeftControl) || Input.IsKeyPressed(Keys.RightControl)) {
                SRPG.GameSt = GameState.Inventory;
            }
        }

        public void Damage( float damage ) {  
            if(!invincible) {
                Health -= damage;
                Timer.AddTimer(() => invincible = false, 0.8f, enabled);
                hit.Play(0.05f, 0f, 0f);
            }
            invincible = true;
            
        }
    }
}
