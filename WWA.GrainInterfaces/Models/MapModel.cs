using System.Collections.Generic;

namespace WWA.GrainInterfaces.Models
{
    public abstract class MapModel : EntityModel
    {
        public MapSizeModel Size { get; set; }
        public Dictionary<string, MapElevationModel> Elevations { get; set; }
    }

    public class MapSizeModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class MapElevationModel
    {
        public MapSpriteLayerModel BaseLayer { get; set; }
        public MapTerrainSpriteLayerModel TerrainLayer { get; set; }
        public MapObjectSpriteLayerModel StructureLayer { get; set; }
        public MapSpriteLayerModel PartitionLayer { get; set; }
        public MapObjectSpriteLayerModel CeilingObjectLayer { get; set; }
        public MapObjectSpriteLayerModel FloorObjectLayer { get; set; }
        public MapObjectSpriteLayerModel SuspendedObjectLayer { get; set; }
        public MapObjectSpriteLayerModel WallObjectLayer { get; set; }
        public MapGatewayLayerModel GatewayLayer { get; set; }
    }
    public class MapSpriteLayerModel
    {
        public Dictionary<string, MapCellModel> Grid { get; set; }
    }
    public class MapObjectSpriteLayerModel
    {
        public Dictionary<string, MapObjectCellModel> Grid { get; set; }
    }
    public class MapTerrainSpriteLayerModel
    {
        public Dictionary<string, MapTerrainCellModel> Grid { get; set; }
    }
    public class MapGatewayLayerModel
    {
        public Dictionary<string, MapGatewayModel> Grid { get; set; }
    }

    public class MapCellModel
    {
        public string SpriteId { get; set; }
        public MapCoordinateModel TileLocation { get; set; }
        public int Z { get; set; }
        public bool IsObstructed { get; set; }
    }

    public class MapCoordinateModel
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MapObjectCellModel : MapCellModel
    {
        public string ObjectKey { get; set; }
    }
    public class MapTerrainCellModel : MapCellModel
    {
        public TerrainSpriteType TerrainType { get; set; }
    }
    public enum TerrainSpriteType
    {
        RaisedBase, RaisedNonBase, RecessedVoid, RecessedFilled
    }

    public class MapGatewayModel
    {
        public string MapId { get; set; }
        public MapCoordinateModel MapCoordinate { get; set; }
    }
}
