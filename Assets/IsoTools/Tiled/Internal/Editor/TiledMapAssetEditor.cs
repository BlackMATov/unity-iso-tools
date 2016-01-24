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
				//TestTestTest();
			}
		}

		/*
		void TestTestTest() {
			foreach ( var layer in _asset.Data.Layers ) {
				for ( var i = 0; i < _asset.Data.Height; ++i ) {
					for ( var j = 0; j < _asset.Data.Width; ++j ) {
						var tile_gid = layer.Tiles[i*_asset.Data.Width + j];
						var tileset_data = FindTilesetByGid(tile_gid);
						if ( tileset_data != null ) {
							var sp_go = new GameObject();
							var sp = sp_go.AddComponent<SpriteRenderer>();
							var assets = AssetDatabase.LoadAllAssetsAtPath(
								string.Format("Assets/IsoTools/Tiled/Examples/{0}",
									tileset_data.ImageSource));

							foreach ( var asset in assets ) {
								var aaa = asset as Sprite;
								if ( aaa && aaa.name == string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(tileset_data.ImageSource), tile_gid) ) {
									var go = new GameObject(string.Format("{0}_{1}", j, i));
									var iso_object = go.AddComponent<IsoObject>();
									iso_object.position = new Vector3(-i, -j, 0.0f);
									iso_object.size = Vector3.one;
									//iso_object.isShowBounds = true;
									sp.sprite = aaa;
									sp.transform.SetParent(go.transform, false);
									sp.transform.localPosition = new Vector3(tileset_data.TileOffsetX, tileset_data.TileOffsetY, 0.0f);
									break;
								}
							}
						}
					}
				}
			}
		}

		TiledMapTilesetData FindTilesetByGid(int gid) {
			foreach ( var tileset in _asset.Data.Tilesets ) {
				if ( gid >= tileset.FirstGid && gid < tileset.FirstGid + tileset.TileCount ) {
					return tileset;
				}
			}
			return null;
		}*/
	}
} // namespace IsoTools.Tiled.Internal