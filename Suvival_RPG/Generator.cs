using Engine;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Suvival_RPG {
    class Generator {
        public Generator() {
            
        }

        public Tilemap GenerateFloor() {
            var roommapsize = 21;
            Tilemap tm = new Tilemap(roommapsize * Room.MaxRoomSize, roommapsize * Room.MaxRoomSize);

            XY start = new XY(roommapsize * Room.MaxRoomSize / 2, roommapsize * Room.MaxRoomSize / 2);
            var mainpath = MakePath(start, tm, 20);

            ERegistry.AddEntity(new Player(new Vector2(roommapsize * Room.MaxRoomSize * Eng.tilesize / 2)));

            foreach (Room r in mainpath) {
                r.PlaceEnemies();
                foreach(Entity e in r.entities) {
                    e.pos += new Vector2(r.roombeg.X * Eng.tilesize, r.roombeg.Y * Eng.tilesize);
                }
                ERegistry.AddRangeE(r.entities);
            }

            for(int x = 0; x < tm.Width; x++) {
                for (int y = 0; y < tm.Height; y++) {
                    if (tm[x, y] is ISolid) {
                        HitBox hb = new HitBox(new Vector2(x * Eng.tilesize, y * Eng.tilesize), Vector2.One * Eng.tilesize, null);
                        hb.kinematic = true;
                        hb.offset = new Vector2(8f, 8f);
                    }
                        
                }
            }

            return tm;
        }

        public List<Room> MakePath(XY start, Tilemap tm, int length) {
            var prevexitglobal = start;
            var prevexitdirection = RandomDirection();
            var rooms = new List<Room>();
            int fails = 0;
            for (int i = 0; i < length; i++) {
                var roomfit = true;
                Room r = new Room(prevexitdirection);
                XY roombeg = prevexitglobal - r.entry;
                for (int x = roombeg.X + 1; x < r.Width + roombeg.X - 1; x++) {
                    for (int y = roombeg.Y + 1; y < r.Height + roombeg.Y - 1; y++) {
                        if (tm[x, y] != null)
                            roomfit = false;
                    }
                }
                if (!roomfit) {
                    fails++;
                    i--;
                    if (fails >= 5) {
                        fails = 0;
                        i = -1;
                        prevexitglobal = start;
                        prevexitdirection = RandomDirection();
                        tm = new Tilemap(tm.Width, tm.Height);
                        rooms.Clear();
                    }
                    continue;
                }

                for (int x = roombeg.X; x < r.Width + roombeg.X; x++) {
                    for (int y = roombeg.Y; y < r.Height + roombeg.Y; y++) {
                        tm[x, y] = r.tiles[x - roombeg.X, y - roombeg.Y];
                    }
                }

                r.roombeg = roombeg;
                rooms.Add(r);

                prevexitglobal += (r.exit - r.entry);
                prevexitdirection = r.exitdirection;
            }
            return rooms;
        }

        public static XY RandomDirection() {
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
    }

    class Room {
        public XY entry;
        public XY exitdirection;
        public XY exit;

        public XY roombeg;

        public const int MaxRoomSize = 15;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public ITile[,] tiles;

        public List<Entity> entities = new List<Entity>();
        public Room(XY prevexitdir) {

            Width = Rng.r.Next(5, MaxRoomSize);
            Height = Rng.r.Next(5, MaxRoomSize);
            tiles = new ITile[Width, Height];
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    if (y == Height - 1 || y == 0 || x == Width - 1 || x == 0) {
                        tiles[x, y] = new DirtWall();
                    } else {
                        tiles[x, y] = new DirtFloor();
                    }

                }
            }

            

            CreateEntrance(prevexitdir);
            CreateExit();
        }

        public void PlaceEnemies() {
            for(int x = 1; x < Width - 1; x++) {
                for(int y = 1; y < Height - 1; y++) {
                    if (Rng.r.Next(0, 20) == 0) {
                        Vector2 enemypos = new Vector2(x * Eng.tilesize, y * Eng.tilesize);
                        entities.Add(new Kobolt(enemypos));
                    }
                        
                }
            }
        }

        void CreateEntrance(XY prevexitdir) {
            if (prevexitdir == XY.Right) {
                int entryrightY = Rng.r.Next(1, Height - 2);
                tiles[0, entryrightY] = new DirtFloor();
                entry = new XY(0, entryrightY);
            }
            else if (prevexitdir == XY.Left) {
                int entryleftY = Rng.r.Next(1, Height - 2);
                tiles[Width - 1, entryleftY] = new DirtFloor();
                entry = new XY(Width - 1, entryleftY);
            }
            else if (prevexitdir == XY.Up) {
                int entryupX = Rng.r.Next(1, Width - 2);
                tiles[entryupX, 0] = new DirtFloor();
                entry = new XY(entryupX, 0);
            }
            else if (prevexitdir == XY.Down) {
                int entrydownX = Rng.r.Next(1, Width - 2);
                tiles[entrydownX, Height - 1] = new DirtFloor();
                entry = new XY(entrydownX, Height - 1);
            }
        }

        void CreateExit() {
            exitdirection = Generator.RandomDirection();

            if (exitdirection == XY.Right) {
                int exitrightY = Rng.r.Next(1, Height - 2);
                tiles[Width - 1, exitrightY] = new DirtFloor();
                exit = new XY(Width - 1, exitrightY);
            }
            else if (exitdirection == XY.Left) {
                int exitleftY = Rng.r.Next(1, Height - 2);
                tiles[0, exitleftY] = new DirtFloor();
                exit = new XY(0, exitleftY);
            }
            else if (exitdirection == XY.Up) {
                int exitupX = Rng.r.Next(1, Width - 2);
                tiles[exitupX, Height - 1] = new DirtFloor();
                exit = new XY(exitupX, Height - 1);
            }
            else if (exitdirection == XY.Down) {
                int exitupX = Rng.r.Next(1, Width - 2);
                tiles[exitupX, 0] = new DirtFloor();
                exit = new XY(exitupX, 0);
            }
        }

    }
}
