using System.Collections.Generic;

namespace WWA.RestApi.ViewModels.Maps
{
    public class MapViewModel
    {
        public MapSizeViewModel Size { get; set; }
        /// <summary>
        /// Elevations key is an int that represents the y index of the corresponding Elevation object.
        /// </summary>
        public Dictionary<string, ElevationViewModel> Elevations { get; set; }
    }

    public class MapSizeViewModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class ElevationViewModel
    {
        public SpriteLayerViewModel BaseLayer { get; set; }
        public TerrainSpriteLayerViewModel TerrainLayer { get; set; }
        public ObjectSpriteLayerViewModel StructureLayer { get; set; }
        public SpriteLayerViewModel PartitionLayer { get; set; }
        public ObjectSpriteLayerViewModel CeilingObjectLayer { get; set; }
        public ObjectSpriteLayerViewModel FloorObjectLayer { get; set; }
        public ObjectSpriteLayerViewModel SuspendedObjectLayer { get; set; }
        public ObjectSpriteLayerViewModel WallObjectLayer { get; set; }
        public GatewayLayerViewModel GatewayLayer { get; set; }
    }

    public class SpriteLayerViewModel
    {
        /// <summary>
        /// Grid key represents a coordinate and should be formatted as "{int x}:{int y}".
        /// </summary>
        public Dictionary<string, CellViewModel> Grid { get; set; }
    }

    public class ObjectSpriteLayerViewModel
    {
        /// <summary>
        /// Grid key represents a coordinate and should be formatted as "{int x}:{int y}".
        /// </summary>
        public Dictionary<string, ObjectCellViewModel> Grid { get; set; }
    }

    public class TerrainSpriteLayerViewModel
    {
        /// <summary>
        /// Grid key represents a coordinate and should be formatted as "{int x}:{int y}".
        /// </summary>
        public Dictionary<string, TerrainCellViewModel> Grid { get; set; }
    }

    public class GatewayLayerViewModel
    {
        /// <summary>
        /// Layer key represents a coordinate and should be formatted as "{int x}:{int y}".
        /// </summary>
        public Dictionary<string, GatewayViewModel> Grid { get; set; }
    }

    public class CoordinateViewModel
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class CellViewModel
    {
        public string SpriteId { get; set; }
        public CoordinateViewModel TileLocation { get; set; }
        public int Z { get; set; }
        public bool IsObstructed { get; set; }
    }
    
    public class ObjectCellViewModel : CellViewModel
    {
        public string ObjectKey { get; set; }
    }

    public class TerrainCellViewModel : CellViewModel
    {
        public TerrainTypeViewModel TerrainType { get; set; }
    }

    public enum TerrainTypeViewModel
    {
        RaisedBase, RaisedNonBase, RecessedVoid, RecessedFilled
    }

    public class GatewayViewModel
    {
        public string MapId { get; set; }
        public CoordinateViewModel MapCoordinate { get; set; }
    }
}
