using UnityEngine;
using UnityEditor;
using IsoTools.Internal;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace IsoTools.Tiled.Internal {
	public class TiledMapAssetPostprocessor : AssetPostprocessor {
		static void OnPostprocessAllAssets(
			string[] imported_assets, string[] deleted_assets,
			string[] moved_assets, string[] moved_from_asset_paths)
		{
			var asset_paths = imported_assets
				.Where(p => Path.GetExtension(p).ToLower().Equals(".asset"));
			foreach ( var asset_path in asset_paths ) {
				var asset = AssetDatabase.LoadAssetAtPath<TiledMapAsset>(asset_path);
				if ( asset ) {
					TiledMapAssetProcess(asset_path, asset);
				}
			}
		}

		static void TiledMapAssetProcess(string asset_path, TiledMapAsset asset) {
			try {
				GenerateLayerMeshes(asset);
			} catch ( Exception e ) {
				Debug.LogErrorFormat(
					"Postprocess tiled map asset error: {0}",
					e.Message);
				AssetDatabase.DeleteAsset(asset_path);
				AssetDatabase.SaveAssets();
			}
		}

		static void GenerateLayerMeshes(TiledMapAsset asset) {
			var dirty = false;
			for ( int i = 0; i < asset.Data.Layers.Count; ++i ) {
				for ( int j = 0; j < asset.Data.Tilesets.Count; ++j ) {
					var mesh_name = string.Format("mesh_{0}_{1}", j, i);
					if ( !HasSubAsset(asset, mesh_name) ) {
						var mesh = GenerateTilesetMesh(asset, j, i);
						mesh.name = mesh_name;
						AssetDatabase.AddObjectToAsset(mesh, asset);
						dirty = true;
					}
				}
			}
			for ( int j = 0; j < asset.Data.Tilesets.Count; ++j ) {
				var material_name = string.Format("material_{0}", j);
				if ( !HasSubAsset(asset, material_name) ) {
					var material = GenerateTilesetMaterial(asset, j);
					material.name = material_name;
					AssetDatabase.AddObjectToAsset(material, asset);
					dirty = true;
				}
			}
			if ( dirty ) {
				EditorUtility.SetDirty(asset);
				AssetDatabase.SaveAssets();
			}
		}

		static bool HasSubAsset(TiledMapAsset asset, string subasset_name) {
			var subassets = AssetDatabase.LoadAllAssetsAtPath(
				AssetDatabase.GetAssetPath(asset));
			return subassets.Any(p => p.name == subasset_name);
		}

		static Mesh GenerateTilesetMesh(TiledMapAsset asset, int tileset_index, int layer_index) {
			var vertices  = new List<Vector3>();
			var triangles = new List<int>();
			var uvs       = new List<Vector2>();
			for ( var tile_y = 0; tile_y < asset.Data.Height; ++tile_y ) {
				for ( var tile_x = 0; tile_x < asset.Data.Width; ++tile_x ) {
					var tile_gid = asset.Data
						.Layers[layer_index]
						.Tiles[tile_y * asset.Data.Width + tile_x];
					if ( tile_gid > 0 && CheckTileGidByTileset(asset, tile_gid, tileset_index) ) {
						var tile_iso_pos = new Vector2(
							-tile_y + asset.Data.Height - 1,
							-tile_x + asset.Data.Width  - 1);

						var tile_screen_pos  = TiledIsoToScreen(asset, tile_iso_pos);
						var tile_sprite      = GetTileSprite(asset, tile_gid, tileset_index);
						var tile_width       = tile_sprite.rect.width  / asset.PixelsPerUnit;
						var tile_height      = tile_sprite.rect.height / asset.PixelsPerUnit;
						var tileset_data     = asset.Data.Tilesets[tileset_index];
						var tileset_offset_x = tileset_data.TileOffsetX / asset.PixelsPerUnit;
						var tileset_offset_y = (tileset_data.TileHeight * 0.5f - tileset_data.TileOffsetY) / asset.PixelsPerUnit;

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

		static Material GenerateTilesetMaterial(TiledMapAsset asset, int tileset_index) {
			var shader = Shader.Find("Sprites/Default");
			if ( !shader ) {
				throw new UnityException("'Sprites/Default' shader not found");
			}
			var material = new Material(shader);
			material.SetTexture("_MainTex", GetTilesetTexture(asset, tileset_index));
			return material;
		}

		static Vector2 TiledIsoToScreen(TiledMapAsset asset, Vector2 iso_pnt) {
			return new Vector2(
				(iso_pnt.x - iso_pnt.y) * asset.Data.TileWidth  * 0.5f / asset.PixelsPerUnit,
				(iso_pnt.x + iso_pnt.y) * asset.Data.TileHeight * 0.5f / asset.PixelsPerUnit);
		}

		static bool CheckTileGidByTileset(TiledMapAsset asset, int tile_gid, int tileset_index) {
			var tileset_data = asset.Data.Tilesets[tileset_index];
			return
				tile_gid >= tileset_data.FirstGid &&
				tile_gid <  tileset_data.FirstGid + tileset_data.TileCount;
		}

		static Sprite GetTileSprite(TiledMapAsset asset, int tile_gid, int tileset_index) {
			var tileset_data = asset.Data.Tilesets[tileset_index];
			var tile_sprite_name = string.Format(
				"{0}_{1}",
				Path.GetFileNameWithoutExtension(tileset_data.ImageSource),
				tile_gid);
			var tileset_assets = AssetDatabase.LoadAllAssetsAtPath(Path.Combine(
				Path.GetDirectoryName(AssetDatabase.GetAssetPath(asset)),
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

		static Texture2D GetTilesetTexture(TiledMapAsset asset, int tileset_index) {
			var tileset_data         = asset.Data.Tilesets[tileset_index];
			var tileset_texture_path = Path.Combine(
				Path.GetDirectoryName(AssetDatabase.GetAssetPath(asset)),
				tileset_data.ImageSource);
			var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(tileset_texture_path);
			if ( !texture ) {
				throw new UnityException(string.Format(
					"texture ({0}) for tileset ({1}) not found",
					tileset_texture_path, tileset_index));
			}
			return texture;
		}
	}
}
