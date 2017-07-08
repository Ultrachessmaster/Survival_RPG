using Box2D.XNA;
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

        Body body;

        public Inventory inventory = new Inventory();

        bool invincible = false;

        float speed = 2f;

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

            body = BoxWrapper.CreateBox(Vector2.One / 4, Vector2.Zero, pos, this);

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
                var vel = body.GetLinearVelocity();
                if (Input.IsKeyDown(Keys.Up))
                    vel.Y = -speed;
                if (Input.IsKeyDown(Keys.Down))
                    vel.Y = speed;
                if (Input.IsKeyDown(Keys.Left))
                    vel.X = -speed;
                if (Input.IsKeyDown(Keys.Right))
                    vel.X = speed;
                body.SetLinearVelocity(vel);
                var offset = new Vector2(Math.Sign(vel.X), Math.Sign(vel.Y)) * Eng.tilesize;
                if (offset != Vector2.Zero)
                    swordoffset = offset;
                if (Input.IsKeyPressed(Keys.Z)) {
                    Sword s = new Sword(pos + swordoffset, gt);
                    var correctedoffset = new Vector2(swordoffset.X, -swordoffset.Y);
                    s.rotation = correctedoffset.Angle(-Vector2.UnitX);
                    ERegistry.AddEntity(s);

                    canmove = false;
                    Timer.AddTimer(() => canmove = true, WaitTime, this);
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
            /*var foods = Physics.GetCollisions<Food>(hb, true);
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
            }*/
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
            body.SetLinearVelocity(Vector2.Zero);
            pos = body.Position * Eng.tilesize;
            UpdateText();
            Camera.X = (int)pos.X - (7 * Eng.tilesize);
            Camera.Y = (int)pos.Y - (6 * Eng.tilesize);

            foreach(Entity e in ERegistry.entities) {
                if (Vector2.Distance(e.pos, pos) < 15 * Eng.tilesize)
                    e.enabled = true;
                else
                    e.enabled = false;
            }

            if(Input.IsKeyPressed(Keys.LeftControl) || Input.IsKeyPressed(Keys.RightControl)) {
                SRPG.GameSt = GameState.Inventory;
            }
        }

        public void Damage( float damage ) {  
            if(!invincible) {
                Health -= damage;
                Timer.AddTimer(() => invincible = false, 0.8f, this);
                hit.Play(0.05f, 0f, 0f);
            }
            invincible = true;
            
        }
    }
}
