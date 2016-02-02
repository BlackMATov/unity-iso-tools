using UnityEngine;
using UnityEditor;
using IsoTools.Tiled;
using IsoTools.Internal;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMap)), CanEditMultipleObjects]
	public class TiledMapMapEditor : Editor {
		TiledMap _map = null;

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
			_map = target as TiledMap;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( _map && _map.Properties != null ) {
				GUILayout.Label(string.Format(
					"Property count: {0}", _map.Properties.Count));
			}
		}
	}
} // namespace IsoTools.Tiled.Internal