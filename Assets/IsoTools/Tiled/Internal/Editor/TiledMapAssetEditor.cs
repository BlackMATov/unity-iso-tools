using UnityEngine;
using UnityEditor;
using IsoTools.Tiled;
using IsoTools.Internal;
using System;
using System.IO;
using System.Linq;

namespace IsoTools.Tiled.Internal {
	[CustomEditor(typeof(TiledMapAsset))]
	public class TiledMapAssetEditor : Editor {
		TiledMapAsset _asset = null;

		// ------------------------------------------------------------------------
		//
		// Functions
		//
		// ------------------------------------------------------------------------

		void CreateTiledMap(GameObject map_go) {
			var iso_object          = map_go.AddComponent<IsoObject>();
			iso_object.mode         = IsoObject.Mode.Mode3d;
			iso_object.position     = Vector3.zero;
			iso_object.size         = IsoUtils.Vec3FromXY(_asset.Data.Height, _asset.Data.Width);
			iso_object.isAlignment  = true;
			iso_object.isShowBounds = true;

			var tiled_map  = map_go.AddComponent<TiledMap>();
			tiled_map.Asset      = _asset;
			tiled_map.Properties = new TiledMapProperties(_asset.Data.Properties);
			foreach ( var layer in _asset.Data.Layers ) {
				CreateTiledMapLayer(tiled_map, layer);
			}
		}

		void CreateTiledMapLayer(TiledMap map, TiledMapLayerData layer_data) {
			var layer_go = new GameObject(layer_data.Name);
			layer_go.transform.SetParent(map.transform, false);
			layer_go.transform.localPosition = IsoUtils.Vec3FromXY(layer_data.OffsetX, -layer_data.OffsetY);
			layer_go.SetActive(layer_data.Visible);

			var tiled_layer = layer_go.AddComponent<TiledMapLayer>();
			tiled_layer.Asset      = _asset;
			tiled_layer.Properties = new TiledMapProperties(layer_data.Properties);
			for ( var i = 0; i < _asset.Data.Height; ++i ) {
				for ( var j = 0; j < _asset.Data.Width; ++j ) {
					CreateTileMapTile(tiled_layer, layer_data, j, i);
				}
			}
		}

		void CreateTileMapTile(TiledMapLayer layer, TiledMapLayerData layer_data, int j, int i) {
			var tile_gid = layer_data.Tiles[i*_asset.Data.Width + j];
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

				var tile_go = new GameObject(string.Format("Tile_{0}_{1}", j, i));
				tile_go.transform.SetParent(layer.transform, false);

				tile_go.transform.localPosition =
					iso_world.IsoToScreen(IsoUtils.Vec3FromXY(
						-i + _asset.Data.Height - 1,
						-j + _asset.Data.Width  - 1));
				
				tile_go.transform.localPosition += new Vector3(
					tileset.TileOffsetX / tile_sprite.pixelsPerUnit,
					tileset.TileOffsetY / tile_sprite.pixelsPerUnit,
					-(i + j) * iso_world.stepDepth);

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
		// Messages
		//
		// ------------------------------------------------------------------------

		void OnEnable() {
			_asset = target as TiledMapAsset;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if ( GUILayout.Button("Create tiled map on scene") ) {
				var map_go = new GameObject("TiledMap");
				try {
					CreateTiledMap(map_go);
				} catch ( Exception e ) {
					Debug.LogErrorFormat("Create tiled map error: {0}", e.Message);
					DestroyImmediate(map_go, true);
				}
				Undo.RegisterCreatedObjectUndo(map_go, "Create TiledMap");
			}
		}
	}
} // namespace IsoTools.Tiled.Internal