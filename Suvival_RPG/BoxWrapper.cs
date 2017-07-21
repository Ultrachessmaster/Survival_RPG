using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Engine;
using FarseerPhysics.Dynamics.Contacts;

namespace Suvival_RPG {
    class FS {
        public static Body CreateBox(Vector2 size, Vector2 offset, Vector2 pos, Entity e, BodyType bt, bool sensor = false) {
            /*BodyDef bd = new BodyDef();
            bd.type = bt;
            bd.position = (pos + offset) / Eng.tilesize;
            bd.fixedRotation = true;
            Body body = SRPG.World.CreateBody(bd);
            PolygonShape ps = new PolygonShape();
            ps.SetAsBox(size.X / 2f, size.Y / 2f);
            FixtureDef fd = new FixtureDef();
            fd.shape = ps;
            fd.density = 1f;
            fd.friction = 0.3f;
            fd.isSensor = sensor;
            body.CreateFixture(fd);
            body.SetUserData(e);
            return body;*/
            Body body = BodyFactory.CreateRectangle(SRPG.World, size.X, size.Y, 1f, (pos + offset) / Eng.tilesize, e);
            body.BodyType = bt;
            body.Friction = 0.3f;
            body.IsSensor = sensor;
            body.FixedRotation = true;
            return body;
        }

        public static Body GetCollision<T>(Body body) {
            for (ContactEdge ce = body.ContactList; ce != null; ce = ce.Next) {
                Contact c = ce.Contact;
                var coll = c.FixtureA.Body;
                var ent = coll.UserData;
                if (ent is T && c.IsTouching) {
                    return coll;
                }
            }
            return null;
        }

        public static List<Body> GetCollisions<T>(Body body) {
            List<Body> bodies = new List<Body>();
            for (ContactEdge ce = body.ContactList; ce != null; ce = ce.Next) {
                Contact c = ce.Contact;
                var coll = c.FixtureA.Body;
                var ent = coll.UserData;
                if (ent is T && c.IsTouching) {
                    bodies.Add(coll);
                } else {
                    var collB = c.FixtureB.Body;
                    var entB = collB.UserData;
                    if (entB is T && !collB.Equals(body) && c.IsTouching)
                        bodies.Add(collB);
                }
            }
            return bodies;
        }
    }
}
