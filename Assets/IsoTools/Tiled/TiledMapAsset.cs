using UnityEngine;
using System.Collections.Generic;

namespace IsoTools.Tiled {
	public enum TiledMapOrientation {
		Isometric
	}

	[System.Serializable]
	public class TiledMapLayerData {
		public string       Name       = "";
		public int          OffsetX    = 0;
		public int          OffsetY    = 0;
		public bool         Visible    = true;
		public List<int>    Tiles      = new List<int>();
		public List<string> Properties = new List<string>();
	}

	[System.Serializable]
	public class TiledMapTilesetData {
		public int          FirstGid    = 0;
		public string       Name        = "";
		public int          Margin      = 0;
		public int          Spacing     = 0;
		public int          TileWidth   = 0;
		public int          TileHeight  = 0;
		public int          TileCount   = 0;
		public int          TileOffsetX = 0;
		public int          TileOffsetY = 0;
		public string       ImageSource = "";
		public List<string> Properties  = new List<string>();
	}

	[System.Serializable]
	public class TiledMapData {
		public int                       Width       = 0;
		public int                       Height      = 0;
		public int                       TileWidth   = 0;
		public int                       TileHeight  = 0;
		public TiledMapOrientation       Orientation = TiledMapOrientation.Isometric;
		public List<string>              Properties  = new List<string>();
		public List<TiledMapLayerData>   Layers      = new List<TiledMapLayerData>();
		public List<TiledMapTilesetData> Tilesets    = new List<TiledMapTilesetData>();
	}

	public class TiledMapAsset : ScriptableObject {
		public TiledMapData Data = new TiledMapData();
	}
}
