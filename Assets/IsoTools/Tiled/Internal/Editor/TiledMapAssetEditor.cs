using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using IsoTools.Tiled;
using IsoTools.Internal;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

			var iso_object       = map_go.AddComponent<IsoObject>();
			iso_object.mode      = IsoObject.Mode.Mode3d;
			iso_object.position  = Vector3.zero;
			iso_object.size      = IsoUtils.Vec3FromXY(map_data.Height, map_data.Width);

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
			var mesh_filter                    = tileset.gameObject.AddComponent<MeshFilter>();
			mesh_filter.mesh                   = GetTilesetMesh(tileset_index, layer_index);
			var mesh_renderer                  = tileset.gameObject.AddComponent<MeshRenderer>();
			mesh_renderer.sharedMaterial       = GetTilesetMaterial(tileset_index);
			mesh_renderer.receiveShadows       = false;
			mesh_renderer.shadowCastingMode    = ShadowCastingMode.Off;
			mesh_renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		}

		Mesh GetTilesetMesh(int tileset_index, int layer_index) {
			var vertices  = new List<Vector3>();
			var triangles = new List<int>();
			var uvs       = new List<Vector2>();
			for ( var tile_y = 0; tile_y < _asset.Data.Height; ++tile_y ) {
				for ( var tile_x = 0; tile_x < _asset.Data.Width; ++tile_x ) {
					var tile_gid = _asset.Data
						.Layers[layer_index]
						.Tiles[tile_y * _asset.Data.Width + tile_x];
					if ( tile_gid > 0 && CheckTileGidByTileset(tile_gid, tileset_index) ) {
						var tile_iso_pos = new Vector3(
							-tile_y + _asset.Data.Height - 1,
							-tile_x + _asset.Data.Width  - 1);
						
						var iso_world        = GetIsoWorld();
						var tile_screen_pos  = iso_world.IsoToScreen(tile_iso_pos);
						var tile_sprite      = GetTileSprite(tile_gid, tileset_index);
						var tile_width       = tile_sprite.rect.width  / _asset.PixelsPerUnit;
						var tile_height      = tile_sprite.rect.height / _asset.PixelsPerUnit;
						var tileset_data     = _asset.Data.Tilesets[tileset_index];
						var tileset_offset_x = tileset_data.TileOffsetX / _asset.PixelsPerUnit;
						var tileset_offset_y = tileset_data.TileOffsetY / _asset.PixelsPerUnit;

						var vertex_pos =
							IsoUtils.Vec3FromVec2(tile_screen_pos) -
							IsoUtils.Vec3FromXY(tile_width, tile_height) * 0.5f +
							IsoUtils.Vec3FromXY(tileset_offset_x, tileset_offset_y);

						vertices.Add(vertex_pos);
						vertices.Add(vertex_pos + IsoUtils.Vec3FromX (tile_width));
						vertices.Add(vertex_pos + IsoUtils.Vec3FromXY(tile_width, tile_height));
						vertices.Add(vertex_pos + IsoUtils.Vec3FromY (tile_height));

						triangles.Add(vertices.Count - 4 + 2);
						triangles.Add(vertices.Count - 4 + 1);
						triangles.Add(vertices.Count - 4 + 0);
						triangles.Add(vertices.Count - 4 + 0);
						triangles.Add(vertices.Count - 4 + 3);
						triangles.Add(vertices.Count - 4 + 2);

						var tex_size = new Vector2(tile_sprite.texture.width, tile_sprite.texture.height);
						uvs.Add(new Vector2(tile_sprite.rect.xMin / tex_size.x, tile_sprite.rect.yMin / tex_size.y));
						uvs.Add(new Vector2(tile_sprite.rect.xMax / tex_size.x, tile_sprite.rect.yMin / tex_size.y));
						uvs.Add(new Vector2(tile_sprite.rect.xMax / tex_size.x, tile_sprite.rect.yMax / tex_size.y));
						uvs.Add(new Vector2(tile_sprite.rect.xMin / tex_size.x, tile_sprite.rect.yMax / tex_size.y));
					}
				}
			}
			var mesh       = new Mesh();
			mesh.vertices  = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.uv        = uvs.ToArray();
			mesh.RecalculateNormals();
			return mesh;
		}

		IsoWorld GetIsoWorld() {
			var iso_world = GameObject.FindObjectOfType<IsoWorld>();
			if ( !iso_world ) {
				throw new UnityException("not found IsoWorld");
			}
			return iso_world;
		}

		string GetTiledMapAssetPath() {
			var asset_path = AssetDatabase.GetAssetPath(_asset);
			if ( string.IsNullOrEmpty(asset_path) ) {
				throw new UnityException(string.Format(
					"not found tiled map asset ({0}) path",
					_asset.name));
			}
			return asset_path;
		}

		bool CheckTileGidByTileset(int tile_gid, int tileset_index) {
			var tileset_data = _asset.Data.Tilesets[tileset_index];
			return
				tile_gid >= tileset_data.FirstGid &&
				tile_gid <  tileset_data.FirstGid + tileset_data.TileCount;
		}

		Sprite GetTileSprite(int tile_gid, int tileset_index) {
			var tileset_data = _asset.Data.Tilesets[tileset_index];
			var tile_sprite_name = string.Format(
				"{0}_{1}",
				Path.GetFileNameWithoutExtension(tileset_data.ImageSource),
				tile_gid);
			var tileset_assets = AssetDatabase.LoadAllAssetsAtPath(Path.Combine(
				Path.GetDirectoryName(GetTiledMapAssetPath()),
				tileset_data.ImageSource));
			var tile_sprite = tileset_assets
				.Where(p => p is Sprite && p.name == tile_sprite_name)
				.Select(p => p as Sprite)
				.FirstOrDefault();
			if ( !tile_sprite ) {
				throw new UnityException(string.Format(
					"sprite ({0}) for tile ({1}) not found",
					tile_sprite_name, tile_gid));
			}
			return tile_sprite;
		}

		Material GetTilesetMaterial(int tileset_index) {
			var shader = Shader.Find("Sprites/Default");
			if ( !shader ) {
				throw new UnityException("'Sprites/Default' shader not found");
			}
			var material = new Material(shader);
			material.SetTexture("_MainTex", GetTilesetTexture(tileset_index));
			return material;
		}

		Texture2D GetTilesetTexture(int tileset_index) {
			var tileset_data = _asset.Data.Tilesets[tileset_index];
			var texture_path = Path.Combine(
				Path.GetDirectoryName(GetTiledMapAssetPath()),
				tileset_data.ImageSource);
			var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texture_path);
			if ( !texture ) {
				throw new UnityException(string.Format(
					"texture ({0}) for tileset ({1}) not found",
					texture_path, tileset_data.Name));
			}
			return texture;
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
