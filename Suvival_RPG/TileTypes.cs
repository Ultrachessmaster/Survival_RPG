using System;
using System.Collections.Generic;
using Engine;

namespace Suvival_RPG {
    struct DirtFloor : ITile {
        public Tile TileType { get { return Tile.DIRT_FLOOR; } }

        public int textureX { get { return 0; } }
        public int textureY { get { return 0; } }
    }

    struct DirtWall : ISolid {
        public Tile TileType { get { return Tile.DIRT_WALL; } }

        public int textureX { get { return 1 * Eng.tilesize; } }
        public int textureY { get { return 0; } }
    }
}
