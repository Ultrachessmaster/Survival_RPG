using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival {
    class Food : Entity {
        FoodType ft;
        public string Name;
        public float hunger_restore;

        public Food(Vector2 pos, FoodType ft) {
            this.pos = pos;
            this.ft = ft;
            tex = SRPG.SpriteMap;
            Sprite = 3 + (int)ft;

            switch(ft) {
                case FoodType.Kobolt_Meat:
                    hunger_restore = 15f;
                    Name = "Kobolt Meat";
                    break;
            }
        }
    }
}
