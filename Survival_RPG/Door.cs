using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival {
    class Door : Entity {
        int goID;
        public Door(Vector2 pos, int goID) {
            this.pos = pos;
            Sprite = 6;
            tex = SRPG.SpriteMap;
            this.goID = goID;
            hitbox = new HitBox(pos, Vector2.One * Eng.tilesize, this);
            hitbox.trigger = true;
            
        }

        public override void Update(GameTime gt) {
            var player = Physics.GetCollision<Player>(hitbox);
            if(player != null) {
                SRPG.RoomID = goID;
                SRPG.LoadRoom(SRPG.RoomID);
            }
        }
    }
}
