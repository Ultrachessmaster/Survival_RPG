﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Engine {

    

    class ERegistry {
        static private List<Entity> entities = new List<Entity>();
        public static int EntityCount { get { return entities.Count; } }

        public void Update(GameTime gameTime) {
            for (int i = entities.Count - 1; i >= 0; i--) {
                var ent = entities[i];
                if (ent.enabled.Value)
                    ent.Update(gameTime);
                else
                    entities.RemoveAt(i);
            }
        }
        public void PostUpdate(GameTime gameTime) {
            for (int i = entities.Count - 1; i >= 0; i--) {
                var ent = entities[i];
                if (ent.enabled.Value)
                    ent.PostUpdate();
                else
                    entities.RemoveAt(i);
            }
        }
        public void Draw(SpriteBatch sb, int pxlratio, int tilesize, Color color) {
            for (int i = 0; i < entities.Count; i++) {
                var ent = entities[i];
                ent.Draw(sb, pxlratio, tilesize, color);
            }
        }
        public static List<Entity> GetEntities<T>() {
            var ents = new List<Entity>();
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i] is T)
                    ents.Add(entities[i]);
            }
            return ents;
        }

        public static Entity GetEntity<T>() {
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i] is T)
                    return entities[i];
            }
            return null;
        }

        public static void AddEntity(Entity e) {
            entities.Add(e);
        }
        public static void RemoveEntity(Entity e) {
            e.enabled.Value = false;
            entities.Remove(e);
        }
        public static void AddRangeE(List<Entity> e) {
            entities.AddRange(e);
        }
    }
}
