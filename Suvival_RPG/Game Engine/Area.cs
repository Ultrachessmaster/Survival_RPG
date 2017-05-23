using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace Engine
{
    public class Area
    {
        public static ITile[,,] Tiles;
        static private List<Entity> entities = new List<Entity>();
        public static int EntityCount { get { return entities.Count; } }
        public void Update(GameTime gameTime)
        {
            for (int i = entities.Count - 1; i >= 0; i--)
            {
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
        public void Draw(SpriteBatch sb, int pxlratio, int tilesize, Color color)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var ent = entities[i];
                ent.Draw(sb, pxlratio, tilesize, color);
            }
        }
        /*public static int TilesSurrounding(XYZ pos, Tile t)
        {
            int count = 0;
            for (int i = pos.X - 1; i < pos.X + 2; i++)
            {
                for (int j = pos.Y - 1; j < pos.Y + 2; j++)
                {
                    if (i == pos.X && j == pos.Y)
                        continue;
                    if (i < 0 || j < 0)
                        continue;
                    if (i >= Tiles.GetUpperBound(0) + 1 || j >= Tiles.GetUpperBound(1) + 1 || pos.Z >= Tiles.GetUpperBound(2) + 1)
                        continue;
                    if (Tiles[i, j, pos.Z].TileType == t)
                        count++;
                }
            }
            return count;
        }*/
        public static int TileTypesSurrounding<T>(XYZ pos)
        {
            int count = 0;
            for (int i = pos.X - 1; i < pos.X + 2; i++)
            {
                for (int j = pos.Y - 1; j < pos.Y + 2; j++)
                {
                    if (i == pos.X && j == pos.Y)
                        continue;
                    if (i < 0 || j < 0)
                        continue;
                    if (i >= Tiles.GetUpperBound(0) + 1 || j >= Tiles.GetUpperBound(1) + 1 || pos.Z >= Tiles.GetUpperBound(2) + 1)
                        continue;
                    if (Tiles[i, j, pos.Z] is T)
                        count++;
                }
            }
            return count;
        }

        public static bool CanWalk(XYZ pos)
        {
            if (pos.X >= Tiles.GetUpperBound(0) || pos.Y >= Tiles.GetUpperBound(1) || pos.X < 0 || pos.Y < 0 || pos.Z >= Tiles.GetUpperBound(2) || pos.Z < 0)
                return false;
            return !(Tiles[pos.X, pos.Y, pos.Z] is ISolid);
        }

        public static List<Entity> GetEntities<T>()
        {
            var ents = new List<Entity>();
            for (int i = 0; i < entities.Count; i++)
            {
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

        public static void AddEntity(Entity e)
        {
            entities.Add(e);
        }
        public static void RemoveEntity(Entity e)
        {
            e.enabled.Value = false;
            entities.Remove(e);
        }
        public static void AddRangeE(List<Entity> e)
        {
            entities.AddRange(e);
        }
    }
}
