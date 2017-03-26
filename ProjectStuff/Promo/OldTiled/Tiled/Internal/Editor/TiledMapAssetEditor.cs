using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using IsoTools.Internal;
using System;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMapAsset))]
	class TiledMapAssetEditor : Editor {
		TiledMapAsset _asset = null;

		// ------------------------------------------------------------------------
		//
		// Functions
		//
		// ------------------------------------------------------------------------

		void CreateTiledMapOnScene() {
			var map_go = new GameObject(_asset.Name);
			try {
				CreateTiledMap(map_go);
			} catch ( Exception e ) {
				Debug.LogErrorFormat("Create tiled map error: {0}", e.Message);
				DestroyImmediate(map_go, true);
			}
			Undo.RegisterCreatedObjectUndo(map_go, "Create Tiled Map");
		}

		void CreateTiledMap(GameObject map_go) {
			var map_data = _asset.Data;

			var iso_object           = map_go.AddComponent<IsoObject>();
			iso_object.renderersMode = IsoObject.RenderersMode.Mode3d;
			iso_object.position      = Vector3.zero;
			iso_object.size          = IsoUtils.Vec3FromXY(map_data.Height, map_data.Width);

			var tiled_map        = map_go.AddComponent<TiledMap>();
			tiled_map.Asset      = _asset;
			tiled_map.Properties = new TiledMapProperties(map_data.Properties);
			for ( int i = map_data.Layers.Count - 1; i >= 0; --i ) {
				CreateTiledLayer(tiled_map, i);
			}
		}

		void CreateTiledLayer(TiledMap map, int layer_index) {
			var layer_data = _asset.Data.Layers[layer_index];

			var layer_go = new GameObject(layer_data.Name);
			layer_go.transform.SetParent(map.transform, false);
			layer_go.transform.localPosition = IsoUtils.Vec3FromXY(
				 layer_data.OffsetX / _asset.PixelsPerUnit,
				-layer_data.OffsetY / _asset.PixelsPerUnit);
			layer_go.transform.localPosition = IsoUtils.Vec3ChangeZ(
				layer_go.transform.localPosition, - layer_index * _asset.LayersDepthStep);
			layer_go.SetActive(layer_data.Visible);

			var tiled_layer        = layer_go.AddComponent<TiledMapLayer>();
			tiled_layer.Asset      = _asset;
			tiled_layer.Properties = new TiledMapProperties(layer_data.Properties);
			for ( int i = 0, e = _asset.Data.Tilesets.Count; i < e; ++i ) {
				CreateTiledTileset(tiled_layer, layer_index, i);
			}
		}

		void CreateTiledTileset(TiledMapLayer layer, int layer_index, int tileset_index) {
			var tileset_data = _asset.Data.Tilesets[tileset_index];

			var tileset_go = new GameObject(tileset_data.Name);
			tileset_go.transform.SetParent(layer.transform, false);

			var tiled_tileset        = tileset_go.AddComponent<TiledMapTileset>();
			tiled_tileset.Asset      = _asset;
			tiled_tileset.Properties = new TiledMapProperties(tileset_data.Properties);
			CreateTiledTilesetMesh(tiled_tileset, tileset_index, layer_index);
		}

		void CreateTiledTilesetMesh(TiledMapTileset tileset, int tileset_index, int layer_index) {
			var mesh_filter              = tileset.gameObject.AddComponent<MeshFilter>();
			mesh_filter.mesh             = GetTilesetMesh(tileset_index, layer_index);
			var mesh_renderer            = tileset.gameObject.AddComponent<MeshRenderer>();
			mesh_renderer.sharedMaterial = GetTilesetMaterial(tileset_index);
		}

		Mesh GetTilesetMesh(int tileset_index, int layer_index) {
			var mesh_name = string.Format("mesh_{0}_{1}", tileset_index, layer_index);
			var subassets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_asset));
			foreach ( var subasset in subassets ) {
				if ( subasset.name == mesh_name && subasset is Mesh ) {
					return subasset as Mesh;
				}
			}
			throw new UnityException(string.Format(
				"not found tileset mesh ({0})",
				mesh_name));
		}

		Material GetTilesetMaterial(int tileset_index) {
			var material_name = string.Format("material_{0}", tileset_index);
			var subassets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_asset));
			foreach ( var subasset in subassets ) {
				if ( subasset.name == material_name && subasset is Material ) {
					return subasset as Material;
				}
			}
			throw new UnityException(string.Format(
				"not found tileset material ({0})",
				material_name));
		}

		// ------------------------------------------------------------------------
		//
		// Messages
		//
		// ------------------------------------------------------------------------

		void OnEnable() {
			_asset = target as TiledMapAsset;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( GUILayout.Button("Create tiled map on scene") ) {
				CreateTiledMapOnScene();
			}
		}
	}
}
