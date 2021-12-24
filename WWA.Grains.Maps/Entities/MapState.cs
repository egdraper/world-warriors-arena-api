using WWA.Grains.Entities;

namespace WWA.Grains.Maps.Entities
{
    public class MapState : TrackedEntity
    {
        public MapSize Size { get; set; }
        public Dictionary<string, MapElevation>? Elevations { get; set; }
    }

    public class MapSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class MapElevation
    {
        public BaseLayer? BaseLayer { get; set; }
        public TerrainLayer? TerrainLayer { get; set; }
        public StructureLayer? StructureLayer { get; set; }
        public PartitionLayer? PartitionLayer { get; set; }
        public CeilingObjectLayer? CeilingObjectLayer { get; set; }
        public FloorObjectLayer? FloorObjectLayer { get; set; }
        public SuspendedObjectLayer? SuspendedObjectLayer { get; set; }
        public WallObjectLayer? WallObjectLayer { get; set; }
        public GatewayLayer? GatewayLayer { get; set; }
    }
    public class SpriteLayer
    {
        public SpriteCategory Category { get; internal set; }
        public Dictionary<string, Cell>? Grid { get; set; }
    }
    public class ObjectSpriteLayer
    {
        public SpriteCategory Category { get; internal set; }
        public Dictionary<string, ObjectCell>? Grid { get; set; }
    }
    public class TerrainSpriteLayer
    {
        public SpriteCategory Category { get; internal set; }
        public Dictionary<string, TerrainCell>? Grid { get; set; }
    }
    public class GatewayLayer
    {
        public SpriteCategory Category { get; internal set; }
        public Dictionary<string, Gateway>? Grid { get; set; }
    }

    public enum SpriteCategory
    {
        Base,
        Terrain,
        Structure,
        Partition,
        CeilingObject,
        FloorObject,
        SuspendedObject,
        WallObject
    }

    public class Cell
    {
        public string? SpriteId { get; set; }
        public MapCoordinate TileLocation { get; set; }
        public int Z { get; set; }
        public bool IsObstructed { get; set; }
    }

    public class MapCoordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class ObjectCell : Cell
    {
        public string? ObjectKey { get; set; }
    }
    public class TerrainCell : Cell
    {
        public TerrainSpriteType TerrainType { get; set; }
    }
    public enum TerrainSpriteType
    {
        RaisedBase, RaisedNonBase, RecessedVoid, RecessedFilled
    }

    public class Gateway
    {
        public string? MapId { get; set; }
        public MapCoordinate MapCoordinate { get; set; }
    }

    public class BaseLayer : SpriteLayer { public BaseLayer() { Category = SpriteCategory.Base; } }
    public class TerrainLayer : TerrainSpriteLayer { public TerrainLayer() { Category = SpriteCategory.Terrain; } }
    public class StructureLayer : ObjectSpriteLayer { public StructureLayer() { Category = SpriteCategory.Structure; } }
    public class PartitionLayer : SpriteLayer { public PartitionLayer() { Category = SpriteCategory.Partition; } }
    public class CeilingObjectLayer : ObjectSpriteLayer { public CeilingObjectLayer() { Category = SpriteCategory.CeilingObject; } }
    public class FloorObjectLayer : ObjectSpriteLayer { public FloorObjectLayer() { Category = SpriteCategory.FloorObject; } }
    public class SuspendedObjectLayer : ObjectSpriteLayer { public SuspendedObjectLayer() { Category = SpriteCategory.SuspendedObject; } }
    public class WallObjectLayer : ObjectSpriteLayer { public WallObjectLayer() { Category = SpriteCategory.WallObject; } }
}
