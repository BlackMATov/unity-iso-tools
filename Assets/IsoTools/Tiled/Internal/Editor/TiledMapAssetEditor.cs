using UnityEngine;
using UnityEditor;
using IsoTools.Tiled;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMapAsset))]
	public class TiledMapAssetEditor : Editor {
		TiledMapAsset _asset = null;

		void OnEnable() {
			_asset = target as TiledMapAsset;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( GUILayout.Button("Create prefab") ) {
			}
		}
	}
} // namespace IsoTools.Tiled.Internal