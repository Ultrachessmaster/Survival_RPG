using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Tilemap
    {
        public ITile[,] Tiles;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Tilemap(int width, int height) {
            Width = width;
            Height = height;
            Tiles = new ITile[width, height];
        }

        public void Draw(SpriteBatch sb, Texture2D texturemap) {
            for(int x = 0; x < Width; x++) {
                for(int y = 0; y < Height; y++) {
                    var tilesize = Eng.tilesize;
                    var pxlratio = Eng.pxlsize;

                    var tile = Tiles[x, y];
                    if (tile == null)
                        continue;
                    Rectangle sourcerect = new Rectangle(tile.textureX, tile.textureY, tilesize, tilesize);
                    var xpos = ((x * tilesize) - Camera.X) * pxlratio;
                    var ypos = ((y * tilesize) - Camera.Y) * pxlratio;
                    Rectangle destrect = new Rectangle((int)Math.Round((double)xpos), (int)Math.Round((double)ypos), tilesize * pxlratio, tilesize * pxlratio);
                    sb.Draw(texturemap, destrect, sourcerect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                }
                
            }
        }

        /*public int TileTypesSurrounding<T>(XYZ pos)
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
        }*/

        /*public bool CanWalk(XYZ pos)
        {
            if (pos.X >= Tiles.GetUpperBound(0) || pos.Y >= Tiles.GetUpperBound(1) || pos.X < 0 || pos.Y < 0 || pos.Z >= Tiles.GetUpperBound(2) || pos.Z < 0)
                return false;
            return !(Tiles[pos.X, pos.Y] is ISolid);
        }*/

        public ITile this[int x, int y] {
            get { return Tiles[x, y]; }
            set { Tiles[x, y] = value; }
        }
    }
}
