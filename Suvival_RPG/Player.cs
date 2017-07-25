using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suvival_RPG {
    class Player : Entity, IDamagable {

        public float Health { get; private set; }
        public float Hunger { get; private set; }
        public static int XP { get; set; }

        Text healthtext;
        Text hungertext;
        Text XPtext;

        HitBox hb;

        public Inventory inventory = new Inventory();

        bool invincible = false;

        float speed = 1f;

        PlayerState ps = PlayerState.Normal;

        public const float swordWaitTIme = 0.25f;
        public const float rollTime = 0.125f;

        SoundEffect hit;

        Vector2 swordoffset = new Vector2(0, 32);

        public Player (Vector2 pos) {
            this.pos = pos;

            Sprite = 0;
            tex = SRPG.SpriteMap;

            Health = 50;
            Hunger = 100;

            hb = new HitBox(pos, new Vector2(4f, 10f), this);

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
            Assert.AreEqual(pos, hb.pos);
            switch(ps) {
                case PlayerState.Normal:
                    var vel = hb.vel;
                    if (Input.IsKeyDown(Keys.Up))
                        vel.Y = -speed;
                    if (Input.IsKeyDown(Keys.Down))
                        vel.Y = speed;
                    if (Input.IsKeyDown(Keys.Left))
                        vel.X = -speed;
                    if (Input.IsKeyDown(Keys.Right))
                        vel.X = speed;
                    hb.vel = vel;
                    var offset = new Vector2(Math.Sign(vel.X), Math.Sign(vel.Y));
                    if (offset != Vector2.Zero)
                        swordoffset = offset * Eng.tilesize / 2;
                    if(Input.IsKeyPressed(Keys.X)) {
                        if(offset != Vector2.Zero) {
                            ps = PlayerState.Rolling;
                            hb.vel *= 7;
                            Timer.AddTimer(() => ps = PlayerState.Normal, rollTime, this);
                        }
                    }
                    if (Input.IsKeyPressed(Keys.Z)) {
                        var correctedoffset = new Vector2(swordoffset.X, -swordoffset.Y);
                        Sword s = new Sword(pos + swordoffset, gt, correctedoffset.Angle(-Vector2.UnitX));

                        ERegistry.AddEntity(s);

                        ps = PlayerState.Frozen;
                        Timer.AddTimer(() => ps = PlayerState.Normal, swordWaitTIme, this);
                    }
                    break;
            }

            //GetItems();
            Hunger -= 0.0015f;
            Hunger = Math.Max(Hunger, 0);
            if (Hunger <= 0)
                Health -= 0.001f;

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
            Vector2 textpos = (pos + (new Vector2(-Eng.tilesize / 2, -Eng.tilesize * 1.5f)));
            XPtext.pos = textpos - Vector2.UnitY * Eng.tilesize;
            healthtext.pos = textpos - Vector2.UnitY * Eng.tilesize / 2;
            hungertext.pos  = textpos;
            healthtext.text = "Health: " + Math.Round(Health);
            hungertext.text = "Hunger: " + Math.Round(Hunger);

        }

        public override void PostUpdate() {
            switch(ps) {
                case PlayerState.Rolling:
                    pos = hb.pos;
                    break;
                default:
                    hb.vel = Vector2.Zero;
                    pos = hb.pos;
                    break;
            }
            UpdateText();
            Camera.X = (int)pos.X - (7 * Eng.tilesize);
            Camera.Y = (int)pos.Y - (6 * Eng.tilesize);

            foreach(Entity e in ERegistry.entities) {
                if (e is Kobolt) {
                    if (Vector2.Distance(e.pos, pos) < 15 * Eng.tilesize) {
                        e.enabled = true;
                        (e as Kobolt).body.enabled = true;
                    } else {
                        e.enabled = false;
                        (e as Kobolt).body.enabled = false;
                    }
                }
                
            }

            if(Input.IsKeyPressed(Keys.LeftControl) || Input.IsKeyPressed(Keys.RightControl)) {
                SRPG.GameSt = GameState.Inventory;
            }
        }

        public void Damage( float damage ) {  
            if(!invincible && ps != PlayerState.Rolling) {
                Health -= damage;
                invincible = true;
                Timer.AddTimer(() => invincible = false, 0.8f, this);
                hit.Play(0.1f, 0f, 0f);
            }
        }
    }
}
