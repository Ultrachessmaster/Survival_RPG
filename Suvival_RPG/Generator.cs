using Engine;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;

namespace Suvival_RPG {
    class Generator {
        public Generator() {
            
        }

        public Tilemap GenerateFloor() {
            const int pathsize = 20;
            const int roommapsize = 41;
            bool[,] roommap = new bool[roommapsize, roommapsize];
            List<XY> mainpath = new List<XY>();
            XY center = new XY(roommapsize / 2, roommapsize / 2);
            roommap[center.X, center.Y] = true;
            XY currentpos = center;
            for (int i = 0; i < pathsize; i++) {
                var nextdir = RandomDirection();
                var nextpos = currentpos + nextdir;
                if (!roommap[nextpos.X, nextpos.Y]) {
                    roommap[nextpos.X, nextpos.Y] = true;
                    mainpath.Add(nextpos);
                    currentpos = nextpos;
                } else if(!roommap[currentpos.X + 1, currentpos.Y]) {
                    roommap[currentpos.X + 1, currentpos.Y] = true;
                    XY pos = new XY(currentpos.X + 1, currentpos.Y);
                    mainpath.Add(pos);
                    currentpos = pos;
                } else if(!roommap[currentpos.X - 1, currentpos.Y]) {
                    roommap[currentpos.X - 1, currentpos.Y] = true;
                    XY pos = new XY(currentpos.X - 1, currentpos.Y);
                    mainpath.Add(pos);
                    currentpos = pos;
                } else if(!roommap[currentpos.X, currentpos.Y + 1]) {
                    roommap[currentpos.X, currentpos.Y + 1] = true;
                    XY pos = new XY(currentpos.X, currentpos.Y + 1);
                    mainpath.Add(pos);
                    currentpos = pos;
                } else if(!roommap[currentpos.X, currentpos.Y - 1]) {
                    roommap[currentpos.X, currentpos.Y - 1] = true;
                    XY pos = new XY(currentpos.X, currentpos.Y - 1);
                    mainpath.Add(pos);
                    currentpos = pos;
                } else {
                    i = -1;
                    currentpos = center;
                    roommap = new bool[roommapsize, roommapsize];
                    mainpath.Clear();
                }
            }

            Tilemap tm = new Tilemap(roommapsize * Room.MaxRoomSize, roommapsize * Room.MaxRoomSize);
            for (int i = 1; i < mainpath.Count; i++) {
                Room r = new Room(mainpath[i] - mainpath[i - 1]);
                for(int x = mainpath[i].X * Room.MaxRoomSize; x < (mainpath[i].X * Room.MaxRoomSize) + r.Width; x++) {
                    for(int y = mainpath[i].Y * Room.MaxRoomSize; y < (mainpath[i].Y * Room.MaxRoomSize) + r.Height; y++) {
                        tm[x, y] = r.tiles[x - (mainpath[i].X * Room.MaxRoomSize), y - (mainpath[i].Y * Room.MaxRoomSize)];
                    }
                }
            }




            /*int fails = 0;
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
                        prevexitdirection = RandomDirection();
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

            for(int x = 0; x < tm.Width; x++) {
                for (int y = 0; y < tm.Height; y++) {
                    if (tm[x, y] is ISolid)
                        FS.CreateBox(Vector2.One, new Vector2(8f, 8f), new Vector2(x * Eng.tilesize, y * Eng.tilesize), null, BodyType.Static);
                }
            }*/

            return tm;
        }

        public XY RandomDirection() {
            int dir = Rng.r.Next(0, 4);
            XY direction = XY.Zero;
            switch (dir) {
                case 0:
                    direction = XY.Right;
                    break;
                case 1:
                    direction = XY.Left;
                    break;
                case 2:
                    direction = XY.Up;
                    break;
                case 3:
                    direction = XY.Down;
                    break;
            }
            return direction;
        }

        public List<Entity> GenerateEnemies(Tilemap tm) {
            List<Entity> enemies = new List<Entity>();
            for (int x = 0; x < tm.Width; x++) {
                for (int y = 0; y < tm.Height; y++) {
                    if (tm[x, y] is ISolid || tm[x, y] == null)
                        continue;
                    if (Rng.r.Next(0, 20) == 0)
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

        public const int MaxRoomSize = 15;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public ITile[,] tiles;
        public Room(XY entrydir) {

            Width = Rng.r.Next(5, MaxRoomSize);
            Height = Rng.r.Next(5, MaxRoomSize);
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
            /*switch(direction) {
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
            }*/
            
        }

    }
}
