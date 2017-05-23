using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suvival_RPG {
    class Food : Entity {
        FoodType ft;
        public string Name;
        public float hunger_restore;
        HitBox hb;

        public Food(Vector2 pos, FoodType ft) {
            this.pos = pos;
            this.ft = ft;
            tex = SRPG.SpriteMap;
            Sprite = 3 + (int)ft;

            hb = new HitBox(pos, new Vector2(Eng.tilesize), this);
            hb.trigger = true;
            Physics.AddCollider(hb);

            switch(ft) {
                case FoodType.Kobolt_Meat:
                    hunger_restore = 15f;
                    Name = "Kobolt Meat";
                    break;
            }
        }
    }
}
