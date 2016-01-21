using UnityEngine;
using UnityEditor;
using IsoTools.Tiled;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace IsoTools.Tiled.Internal {
	public class TiledMapPostprocessor : AssetPostprocessor {
		static void OnPostprocessAllAssets(
			string[] imported_assets, string[] deleted_assets,
			string[] moved_assets, string[] moved_from_asset_paths)
		{
			var tmx_assets = imported_assets
				.Where(p => Path.GetExtension(p).ToLower().Equals(".tmx"));
			foreach ( var tmx_asset in tmx_assets ) {
				TmxAssetProcess(tmx_asset);
			}
		}

		static void TmxAssetProcess(string tmx_asset) {
			var tile_map_data = LoadTiledMapFromTmxFile(tmx_asset);
			if ( tile_map_data != null ) {
				var new_asset_path = Path.ChangeExtension(tmx_asset, ".asset");
				var new_asset = AssetDatabase.LoadAssetAtPath<TiledMapAsset>(new_asset_path);
				if ( !new_asset ) {
					new_asset = ScriptableObject.CreateInstance<TiledMapAsset>();
					AssetDatabase.CreateAsset(new_asset, new_asset_path);
				}
				new_asset.Data = tile_map_data;
				EditorUtility.SetDirty(new_asset);
				AssetDatabase.SaveAssets();
			}
		}

		static TiledMapData LoadTiledMapFromTmxFile(string tmx_path) {
			try {
				var tmx_root_elem = XDocument.Load(tmx_path).Document.Root;
				var tiled_map_data = new TiledMapData();
				LoadTiledMapOptsFromTmxRootElem(tmx_root_elem, tiled_map_data);
				LoadTiledMapLayersFromTmxRootElem(tmx_root_elem, tiled_map_data);
				LoadTiledMapTilesetsFromTmxRootElem(tmx_root_elem, tiled_map_data);
				return tiled_map_data;
			} catch ( Exception e ) {
				Debug.LogErrorFormat("Parsing TMX file error: {0}", e.Message);
				return null;
			}
		}

		static void LoadTiledMapOptsFromTmxRootElem(XElement root_elem, TiledMapData data) {
			data.Width      = SafeLoadIntFromElemAttr(root_elem, "width"     , data.Width);
			data.Height     = SafeLoadIntFromElemAttr(root_elem, "height"    , data.Height);
			data.TileWidth  = SafeLoadIntFromElemAttr(root_elem, "tilewidth" , data.TileWidth);
			data.TileHeight = SafeLoadIntFromElemAttr(root_elem, "tileheight", data.TileHeight);
			var orientation_str = root_elem.Attribute("orientation").Value;
			switch ( orientation_str ) {
			case "isometric":
				data.Orientation = TiledMapOrientation.Isometric;
				break;
			default:
				throw new UnityException(string.Format(
					"Parsing TMX file error: unsupported orientation ({0})",
					orientation_str));
			}
			SafeLoadPropertiesFromOwnerElem(root_elem, data.Properties);
		}

		// -----------------------------
		// Layers
		// -----------------------------

		static void LoadTiledMapLayersFromTmxRootElem(XElement root_elem, TiledMapData data) {
			foreach ( var layer_elem in root_elem.Elements("layer") ) {
				var layer = new TiledMapLayerData();
				LoadTiledMapLayerFromTmxLayerElem(layer_elem, layer);
				data.Layers.Add(layer);
			}
		}

		static void LoadTiledMapLayerFromTmxLayerElem(XElement layer_elem, TiledMapLayerData layer) {
			layer.Name    = SafeLoadStrFromElemAttr (layer_elem, "name"   , layer.Name);
			layer.Width   = SafeLoadIntFromElemAttr (layer_elem, "width"  , layer.Width);
			layer.Height  = SafeLoadIntFromElemAttr (layer_elem, "height" , layer.Height);
			layer.OffsetX = SafeLoadIntFromElemAttr (layer_elem, "offsetx", layer.OffsetX);
			layer.OffsetY = SafeLoadIntFromElemAttr (layer_elem, "offsety", layer.OffsetY);
			layer.Visible = SafeLoadBoolFromElemAttr(layer_elem, "visible", layer.Visible);
			LoadTiledMapLayerTilesFromTmxLayerElem(layer_elem, layer);
			SafeLoadPropertiesFromOwnerElem(layer_elem, layer.Properties);
		}

		static void LoadTiledMapLayerTilesFromTmxLayerElem(XElement layer_elem, TiledMapLayerData layer) {
			foreach ( var elem in layer_elem.Element("data").Elements("tile") ) {
				layer.Tiles.Add(SafeLoadIntFromElemAttr(elem, "gid", 0));
			}
		}

		// -----------------------------
		// Tilesets
		// -----------------------------

		static void LoadTiledMapTilesetsFromTmxRootElem(XElement root_elem, TiledMapData data) {
			foreach ( var tileset_elem in root_elem.Elements("tileset") ) {
				var tileset = new TiledMapTilesetData();
				LoadTiledMapTilesetFromTmxTilesetElem(tileset_elem, tileset);
				data.Tilesets.Add(tileset);
			}
		}

		static void LoadTiledMapTilesetFromTmxTilesetElem(XElement tileset_elem, TiledMapTilesetData tileset) {
			tileset.FirstGid    = SafeLoadIntFromElemAttr(tileset_elem, "firstgid"  , tileset.FirstGid);
			tileset.Name        = SafeLoadStrFromElemAttr(tileset_elem, "name"      , tileset.Name);
			tileset.Margin      = SafeLoadIntFromElemAttr(tileset_elem, "margin"    , tileset.Margin);
			tileset.Spacing     = SafeLoadIntFromElemAttr(tileset_elem, "spacing"   , tileset.Spacing);
			tileset.TileWidth   = SafeLoadIntFromElemAttr(tileset_elem, "tilewidth" , tileset.TileWidth);
			tileset.TileHeight  = SafeLoadIntFromElemAttr(tileset_elem, "tileheight", tileset.TileHeight);
			tileset.TileCount   = SafeLoadIntFromElemAttr(tileset_elem, "tilecount" , tileset.TileCount);
			tileset.TileOffsetX = SafeLoadIntFromElemAttr(tileset_elem.Element("tileoffset"), "x"     , tileset.TileOffsetX);
			tileset.TileOffsetY = SafeLoadIntFromElemAttr(tileset_elem.Element("tileoffset"), "y"     , tileset.TileOffsetY);
			tileset.Image       = SafeLoadStrFromElemAttr(tileset_elem.Element("image"     ), "source", tileset.Image);
			tileset.ImageWidth  = SafeLoadIntFromElemAttr(tileset_elem.Element("image"     ), "widht" , tileset.ImageWidth);
			tileset.ImageHeight = SafeLoadIntFromElemAttr(tileset_elem.Element("image"     ), "height", tileset.ImageHeight);
			SafeLoadPropertiesFromOwnerElem(tileset_elem, tileset.Properties);
		}

		// -----------------------------
		// Common
		// -----------------------------

		static string SafeLoadStrFromElemAttr(XElement elem, string attr_name, string def_value) {
			if ( elem != null && elem.Attribute(attr_name) != null ) {
				return elem.Attribute(attr_name).Value;
			}
			return def_value;
		}

		static int SafeLoadIntFromElemAttr(XElement elem, string attr_name, int def_value) {
			int value;
			if ( elem != null && int.TryParse(SafeLoadStrFromElemAttr(elem, attr_name, ""), out value) ) {
				return value;
			}
			return def_value;
		}

		static bool SafeLoadBoolFromElemAttr(XElement elem, string attr_name, bool def_value) {
			int value;
			if ( elem != null && int.TryParse(SafeLoadStrFromElemAttr(elem, attr_name, ""), out value) ) {
				return value != 0;
			}
			return def_value;
		}

		static void SafeLoadPropertiesFromOwnerElem(XElement owner_elem, List<string> props) {
			var props_elem = owner_elem != null ? owner_elem.Element("properties") : null;
			if ( props_elem != null ) {
				foreach ( var prop_elem in props_elem.Elements("property") ) {
					var prop_name  = SafeLoadStrFromElemAttr(prop_elem, "name" , string.Empty);
					var prop_value = SafeLoadStrFromElemAttr(prop_elem, "value", string.Empty);
					if ( !string.IsNullOrEmpty(prop_name) && prop_value != string.Empty ) {
						props.Add(prop_name);
						props.Add(prop_value);
					}
				}
			}
		}
	}
} // namespace IsoTools.Tiled.Internal
