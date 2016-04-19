using UnityEngine;
using UnityEditor;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMapTileset)), CanEditMultipleObjects]
	class TiledMapTileEditor : Editor {
		TiledMapTileset _tileset = null;

		void OnEnable() {
			_tileset = target as TiledMapTileset;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( _tileset && _tileset.Properties != null ) {
				_tileset.Properties.OnInspectorGUI("Tileset properties");
			}
		}
	}
}
