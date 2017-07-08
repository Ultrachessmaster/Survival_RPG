using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using Engine;
namespace Suvival_RPG {
    class BoxWrapper {
        public static Body CreateBox(Vector2 size, Vector2 offset, Vector2 pos, Entity e) {
            BodyDef bd = new BodyDef();
            bd.type = BodyType.Dynamic;
            bd.position = pos / Eng.tilesize;
            Body body = SRPG.World.CreateBody(bd);
            PolygonShape ps = new PolygonShape();
            ps.SetAsBox(size.X, size.Y);
            FixtureDef fd = new FixtureDef();
            fd.shape = ps;
            fd.density = 1f;
            fd.friction = 0.3f;
            body.CreateFixture(fd);
            body.SetUserData(e);
            return body;
        }

        public static Body GetCollision<T>(Body body) {
            for (ContactEdge ce = body.GetContactList(); ce != null; ce = ce.Next) {
                Contact c = ce.Contact;
                var coll = c.GetFixtureA().GetBody();
                var ent = coll.GetUserData();
                if (ent is T) {
                    return coll;
                }
            }
            return null;
        }
    }
}
