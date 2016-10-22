using UnityEngine;
using UnityEditor;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMap)), CanEditMultipleObjects]
	class TiledMapMapEditor : Editor {
		TiledMap _map = null;

		void OnEnable() {
			_map = target as TiledMap;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( _map && _map.Properties != null ) {
				_map.Properties.OnInspectorGUI("Map properties");
			}
		}
	}
}
