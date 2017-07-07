namespace Engine
{
    public interface ITile
    {
        Tile TileType { get; }
        int textureX { get; }
        int textureY { get; }
    }
    //TODO: Make new tile types
    interface ISolid : ITile { }
    struct GenericTile : ITile
    {
        public Tile TileType { get { return tileType; } }
        Tile tileType;
        public GenericTile(Tile t)
        {
            tileType = t;
        }

        public int textureX { get { return 0; } }
        public int textureY { get { return 0; } }
    }
}
