using Engine;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Generator {
        public Generator() {
            
        }

        public Tilemap GenerateFloor(int width, int height) {
            Tilemap tm = new Tilemap(width, height);
            XY prevexitglobal = new XY(width/2, width/2);
            XY prevexitdirection = XY.Right;
            int fails = 0;
            for(int i = 0; i < 20; i++) {
                var roomfit = true;
                Room r = new Room(prevexitdirection);
                XY roombeg = prevexitglobal - r.entry;
                for (int x = roombeg.X + 1; x < r.Width + roombeg.X - 1; x++) {
                    for (int y = roombeg.Y + 1; y < r.Height + roombeg.Y - 1; y++) {
                        if (tm[x, y] != null)
                            roomfit = false;
                    }
                }
                if(!roomfit) {
                    fails++;
                    i--;
                    if(fails >= 5) {
                        fails = 0;
                        i = -1;
                        tm = new Tilemap(width, height);
                        prevexitglobal = new XY(width / 2, width / 2);
                        prevexitdirection = XY.Right;
                    }
                    continue;
                }
                for (int x = roombeg.X; x < r.Width + roombeg.X; x++) {
                    for (int y = roombeg.Y; y < r.Height + roombeg.Y; y++) {
                        tm[x, y] = r.tiles[x - roombeg.X, y - roombeg.Y];
                    }
                }
                

                prevexitglobal += (r.exit - r.entry);
                prevexitdirection = r.exitdirection;
            }

            return tm;
        }

        public List<Entity> GenerateEnemies(Tilemap tm) {
            List<Entity> enemies = new List<Entity>();
            for (int x = 0; x < tm.Width; x++) {
                for (int y = 0; y < tm.Height; y++) {
                    if (tm[x, y] is ISolid || tm[x, y] == null)
                        continue;
                    if (Rng.r.Next(0, 15) == 0)
                        enemies.Add(new Kobolt(new Vector2(x * Eng.tilesize, y * Eng.tilesize)));
                }
            }
            return enemies;
        }
    }

    class Room {
        public XY entry;
        public XY exitdirection;
        public XY exit;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public ITile[,] tiles;
        public Room(XY entrydir) {

            Width = Rng.r.Next(5, 15);
            Height = Rng.r.Next(5, 15);
            tiles = new ITile[Width, Height];
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    if (y == Height - 1 || y == 0 || x == Width - 1 || x == 0) {
                        tiles[x, y] = new DirtWall();
                    }
                    else {
                        tiles[x, y] = new DirtFloor();
                    }

                }
            }
            int direction = Rng.r.Next(0, 4);
            switch(direction) {
                case 0: if (entrydir == XY.Right) direction = 3; break;
                case 1: if (entrydir == XY.Left) direction = 2; break;
                case 2: if (entrydir == XY.Up) direction = 0; break;
                case 3: if (entrydir == XY.Down) direction = 1; break;
            }
            switch(direction) {
                case 0://right
                    int exitrightY = Rng.r.Next(1, Height - 2);
                    tiles[Width - 1, exitrightY] = new DirtFloor();
                    exitdirection = XY.Left;
                    exit = new XY(Width - 1, exitrightY);
                    break;
                case 1://left
                    int exitleftY = Rng.r.Next(1, Height - 2);
                    tiles[0, exitleftY] = new DirtFloor();
                    exitdirection = XY.Right;
                    exit = new XY(0, exitleftY);
                    break;
                case 2://up
                    int exitupX = Rng.r.Next(1, Width - 2);
                    tiles[exitupX, Height - 1] = new DirtFloor();
                    exitdirection = XY.Down;
                    exit = new XY(exitupX, Height - 1);
                    break;
                case 3://down
                    int exitdownX = Rng.r.Next(1, Width - 2);
                    tiles[exitdownX, 0] = new DirtFloor();
                    exitdirection = XY.Up;
                    exit = new XY(exitdownX, 0);
                    break;
            }

            if(entrydir == XY.Right) {
                int entryY = Rng.r.Next(1, Height - 2);
                tiles[Width - 1, entryY] = new DirtFloor();
                entry = new XY(Width - 1, entryY);
            } else if(entrydir == XY.Left) {
                int entryY = Rng.r.Next(1, Height - 2);
                tiles[0, entryY] = new DirtFloor();
                entry = new XY(0, entryY);
            } else if(entrydir == XY.Up) {
                int entryX = Rng.r.Next(1, Width - 2);
                tiles[entryX, Height - 1] = new DirtFloor();
                entry = new XY(entryX, Height - 1);
            } else if(entrydir == XY.Down) {
                int entryX = Rng.r.Next(1, Width - 2);
                tiles[entryX, 0] = new DirtFloor();
                entry = new XY(entryX, 0);
            }
        }

    }
}
