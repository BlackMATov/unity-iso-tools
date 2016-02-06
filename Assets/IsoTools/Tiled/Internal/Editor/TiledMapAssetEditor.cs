using UnityEngine;
using UnityEditor;
using IsoTools.Tiled;
using IsoTools.Internal;
using System;
using System.IO;
using System.Linq;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMapAsset))]
	class TiledMapAssetEditor : Editor {
		TiledMapAsset _asset = null;

		// ------------------------------------------------------------------------
		//
		// Functions
		//
		// ------------------------------------------------------------------------

		void CreateTiledMap(GameObject map_go) {
			var iso_object      = map_go.AddComponent<IsoObject>();
			iso_object.mode     = IsoObject.Mode.Mode3d;
			iso_object.position = Vector3.zero;
			iso_object.size     = IsoUtils.Vec3FromXY(_asset.Data.Height, _asset.Data.Width);

			var tiled_map  = map_go.AddComponent<TiledMap>();
			tiled_map.Asset      = _asset;
			tiled_map.Properties = new TiledMapProperties(_asset.Data.Properties);
			for ( int i = 0, e = _asset.Data.Layers.Count; i < e; ++i ) {
				CreateTiledMapLayer(tiled_map, i);
			}
		}

		void CreateTiledMapLayer(TiledMap map, int layer_index) {
			var layer_data = _asset.Data.Layers[layer_index];

			var layer_go = new GameObject(layer_data.Name);
			layer_go.transform.SetParent(map.transform, false);
			layer_go.transform.localPosition = IsoUtils.Vec3FromXY(
				 layer_data.OffsetX / _asset.PixelsPerUnit,
				-layer_data.OffsetY / _asset.PixelsPerUnit);
			layer_go.SetActive(layer_data.Visible);

			var tiled_layer = layer_go.AddComponent<TiledMapLayer>();
			tiled_layer.Asset      = _asset;
			tiled_layer.Properties = new TiledMapProperties(layer_data.Properties);
			for ( var tile_y = 0; tile_y < _asset.Data.Height; ++tile_y ) {
				for ( var tile_x = 0; tile_x < _asset.Data.Width; ++tile_x ) {
					CreateTileMapTile(tiled_layer, layer_index, tile_x, tile_y);
				}
			}
		}

		void CreateTileMapTile(TiledMapLayer layer, int layer_index, int tile_x, int tile_y) {
			var layer_data = _asset.Data.Layers[layer_index];

			var tile_gid = layer_data.Tiles[tile_y * _asset.Data.Width + tile_x];
			if ( tile_gid > 0 ) {
				var asset_path = AssetDatabase.GetAssetPath(_asset);
				if ( string.IsNullOrEmpty(asset_path) ) {
					throw new UnityException(string.Format(
						"not found tiled map asset ({0}) path",
						_asset.name));
				}

				var iso_world = GameObject.FindObjectOfType<IsoWorld>();
				if ( !iso_world ) {
					throw new UnityException("not found IsoWorld");
				}

				var tileset = FindTilesetByTileGid(tile_gid);
				if ( tileset == null ) {
					throw new UnityException(string.Format(
						"tileset for tile ({0}) on layer ({1}) not found",
						tile_gid, layer_data.Name));
				}

				var tile_tileset_sprite_name = string.Format(
					"{0}_{1}",
					Path.GetFileNameWithoutExtension(tileset.ImageSource),
					tile_gid);
				var tileset_assets = AssetDatabase.LoadAllAssetsAtPath(
					Path.Combine(Path.GetDirectoryName(asset_path), tileset.ImageSource));
				var tile_sprite = tileset_assets
					.Where(p => p is Sprite && p.name == tile_tileset_sprite_name)
					.Select(p => p as Sprite)
					.FirstOrDefault();
				if ( !tile_sprite ) {
					throw new UnityException(string.Format(
						"sprite ({0}) for tile ({1}) on layer ({2}) not found",
						tile_tileset_sprite_name, tile_gid, layer_data.Name));
				}

				var iso_x = -tile_y + _asset.Data.Height - 1;
				var iso_y = -tile_x + _asset.Data.Width  - 1;

				var tile_go = new GameObject(string.Format(
					"Tile_{0}_{1}", iso_x, iso_y));
				tile_go.transform.SetParent(layer.transform, false);

				tile_go.transform.localPosition =
					iso_world.IsoToScreen(IsoUtils.Vec3FromXY(iso_x, iso_y));
				
				tile_go.transform.localPosition += new Vector3(
					tileset.TileOffsetX / _asset.PixelsPerUnit,
					tileset.TileOffsetY / _asset.PixelsPerUnit,
					(iso_x + iso_y + layer_index) * iso_world.stepDepth);

				var tiled_tile = tile_go.AddComponent<TiledMapTile>();
				tiled_tile.Asset      = _asset;
				tiled_tile.Properties = new TiledMapProperties(tileset.Properties);

				var tile_spr = tile_go.AddComponent<SpriteRenderer>();
				tile_spr.sprite = tile_sprite;
			}
		}

		TiledMapTilesetData FindTilesetByTileGid(int tile_gid) {
			return _asset.Data.Tilesets.Find(p => {
				return tile_gid >= p.FirstGid && tile_gid < p.FirstGid + p.TileCount;
			});
		}

		// ------------------------------------------------------------------------
		//
		// Functions
		//
		// ------------------------------------------------------------------------

		void CreateTiledMapPrefab() {
			var tiled_map = CreateTiledMapOnScene();
			if ( tiled_map ) {
				var asset_path  = AssetDatabase.GetAssetPath(_asset);
				var prefab_path = Path.Combine(Path.GetDirectoryName(asset_path), _asset.Name + ".prefab");
				PrefabUtility.CreatePrefab(prefab_path, tiled_map);
				DestroyImmediate(tiled_map, true);
				/// \TODO undo support
			}
		}

		GameObject CreateTiledMapOnScene() {
			var map_go = new GameObject(_asset.Name);
			try {
				CreateTiledMap(map_go);
			} catch ( Exception e ) {
				Debug.LogErrorFormat("Create tiled map error: {0}", e.Message);
				DestroyImmediate(map_go, true);
				map_go = null;
			}
			Undo.RegisterCreatedObjectUndo(map_go, "Create TiledMap");
			return map_go;
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
			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button("Create map prefab") ) {
				CreateTiledMapPrefab();
			}
			if ( GUILayout.Button("Create map on scene") ) {
				CreateTiledMapOnScene();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
