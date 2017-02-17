using UnityEngine;
using UnityEditor;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMapLayer)), CanEditMultipleObjects]
	class TiledMapLayerEditor : Editor {
		TiledMapLayer _layer = null;

		void OnEnable() {
			_layer = target as TiledMapLayer;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( _layer && _layer.Properties != null ) {
				_layer.Properties.OnInspectorGUI("Layer properties");
			}
		}
	}
}
