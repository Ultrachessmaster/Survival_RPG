using Engine;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Survival {
    class Generator {
        public const int RoomMapSize = 41;
        public static XY Start = new XY(RoomMapSize / 2, RoomMapSize / 2);

        public List<Room> GenerateFloor() {
            bool[,] roommap = new bool[RoomMapSize, RoomMapSize];
            XY current = Start;
            roommap[Start.X, Start.Y] = true;
            List<XY> rpos = new List<XY>();
            for(int i = 0; i < 20; i++) {
                Action<XY> AssignRoom = (XY direc) => {
                    current += direc;
                    roommap[current.X, current.Y] = true;
                    rpos.Add(current);
                };

                XY dir = RandomDirection();
                XY newpos = current + dir;

                if (!roommap[newpos.X, newpos.Y]) {
                    roommap[newpos.X, newpos.Y] = true;
                    current = newpos;
                    rpos.Add(current);
                } else if(!roommap[current.X + 1, current.Y]) {
                    AssignRoom(XY.Right);
                } else if(!roommap[current.X - 1, current.Y]) {
                    AssignRoom(XY.Left);
                } else if(!roommap[current.X, current.Y + 1]) {
                    AssignRoom(XY.Up);
                } else if(!roommap[current.X, current.Y - 1]) {
                    AssignRoom(XY.Down);
                } else {
                    roommap = new bool[RoomMapSize, RoomMapSize];
                    roommap[Start.X, Start.Y] = true;
                    current = Start;
                    rpos.Clear();
                    i = -1;
                }
            }
            List<Room> rooms = new List<Room>();
            for(int i = 1; i < rpos.Count - 1; i++) {
                Room r = new Room(rpos[i] - rpos[i - 1], rpos[i + 1] - rpos[i], i - 1);
                r.roompos = rpos[i];
                foreach(Entity e in r.entities) {
                    e.Enabled = false;
                }
                Area.AddRangeE(r.entities);
                rooms.Add(r);
            }

            XY playerpos = rooms[0].entry;
            Player p = new Player(new Vector2((playerpos.X + 2) * Eng.tilesize, playerpos.Y * Eng.tilesize));
            rooms[0].AddEntity(p);
            Area.AddEntity(p);
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

        public XY roompos;

        public const int MaxRoomSize = 25;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public ITile[,] tiles;

        public List<Entity> entities = new List<Entity>();

        XY prevexitdir;
        XY exitdir;
        public Room(XY prevexitdir, XY exitdir, int id) {
            this.prevexitdir = prevexitdir;
            this.exitdir = exitdir;
            Generate(id);
        }

        public void Generate(int id) {
            Width = Rng.r.Next(10, MaxRoomSize);
            Height = Rng.r.Next(10, MaxRoomSize);
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

            CreateEntrance(prevexitdir, id);
            CreateExit(exitdir, id);
            PlaceEnemies();
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

        public void AddEntity(Entity e) {
            entities.Add(e);
        }

        void CreateEntrance(XY prevexitdir, int id) {
            if (prevexitdir == XY.Right) {
                int entryrightY = Rng.r.Next(1, Height - 2);
                tiles[0, entryrightY] = new DirtFloor();
                entry = new XY(0, entryrightY);
                Vector2 doorpos = new Vector2(entry.X * Eng.tilesize, entry.Y * Eng.tilesize);
                entities.Add(new Door(doorpos, id + 1));
            }
            else if (prevexitdir == XY.Left) {
                int entryleftY = Rng.r.Next(1, Height - 2);
                tiles[Width - 1, entryleftY] = new DirtFloor();
                entry = new XY(Width - 1, entryleftY);
                Vector2 doorpos = new Vector2(entry.X * Eng.tilesize, entry.Y * Eng.tilesize);
                entities.Add(new Door(doorpos, id + 1));
            }
            else if (prevexitdir == XY.Up) {
                int entryupX = Rng.r.Next(1, Width - 2);
                tiles[entryupX, 0] = new DirtFloor();
                entry = new XY(entryupX, 0);
                Vector2 doorpos = new Vector2(entry.X * Eng.tilesize, entry.Y * Eng.tilesize);
                entities.Add(new Door(doorpos, id + 1));
            }
            else if (prevexitdir == XY.Down) {
                int entrydownX = Rng.r.Next(1, Width - 2);
                tiles[entrydownX, Height - 1] = new DirtFloor();
                entry = new XY(entrydownX, Height - 1);
                Vector2 doorpos = new Vector2(entry.X * Eng.tilesize, entry.Y * Eng.tilesize);
                entities.Add(new Door(doorpos, id + 1));
            }
        }

        void CreateExit(XY exitdirection, int id) {
            if (exitdirection == XY.Right) {
                int exitrightY = Rng.r.Next(1, Height - 2);
                tiles[Width - 1, exitrightY] = new DirtFloor();
                exit = new XY(Width - 1, exitrightY);
                Vector2 doorpos = new Vector2(exit.X * Eng.tilesize, exit.Y * Eng.tilesize);
                entities.Add(new Door(doorpos, id));
            }
            else if (exitdirection == XY.Left) {
                int exitleftY = Rng.r.Next(1, Height - 2);
                tiles[0, exitleftY] = new DirtFloor();
                exit = new XY(0, exitleftY);
                Vector2 doorpos = new Vector2(exit.X * Eng.tilesize, exit.Y * Eng.tilesize);
                entities.Add(new Door(doorpos, id));
            }
            else if (exitdirection == XY.Up) {
                int exitupX = Rng.r.Next(1, Width - 2);
                tiles[exitupX, Height - 1] = new DirtFloor();
                exit = new XY(exitupX, Height - 1);
                Vector2 doorpos = new Vector2(exit.X * Eng.tilesize, exit.Y * Eng.tilesize);
                entities.Add(new Door(doorpos, id));
            }
            else if (exitdirection == XY.Down) {
                int exitupX = Rng.r.Next(1, Width - 2);
                tiles[exitupX, 0] = new DirtFloor();
                exit = new XY(exitupX, 0);
                Vector2 doorpos = new Vector2(exit.X * Eng.tilesize, exit.Y * Eng.tilesize);
                entities.Add(new Door(doorpos, id));
            }
        }

        public void SetEnabledAllEntities(bool enabled) {
            foreach(Entity e in entities) {
                e.Enabled = enabled;
            }
        }

    }
}
