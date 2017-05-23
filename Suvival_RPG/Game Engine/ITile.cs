namespace Engine
{
    public interface ITile
    {
        Tile TileType { get; }
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
    }
}
