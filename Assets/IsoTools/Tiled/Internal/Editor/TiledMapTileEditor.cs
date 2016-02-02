using UnityEngine;
using UnityEditor;
using IsoTools.Tiled;
using IsoTools.Internal;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMapTile)), CanEditMultipleObjects]
	public class TiledMapTileEditor : Editor {
		TiledMapTile _tile = null;

		// ------------------------------------------------------------------------
		//
		// Functions
		//
		// ------------------------------------------------------------------------

		// ------------------------------------------------------------------------
		//
		// Messages
		//
		// ------------------------------------------------------------------------

		void OnEnable() {
			_tile = target as TiledMapTile;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( _tile && _tile.Properties != null ) {
				GUILayout.Label(string.Format(
					"Property count: {0}", _tile.Properties.Count));
			}
		}
	}
}
