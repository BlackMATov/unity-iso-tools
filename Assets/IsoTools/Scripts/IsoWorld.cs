using UnityEngine;
using System.Collections.Generic;

namespace IsoTools {
	[ExecuteInEditMode]
	public class IsoWorld : MonoBehaviour {
		
		/// <summary>World tile types.</summary>
		public enum TileTypes {
			Isometric,
			UpDown
		}
		
		/// <summary>World tile type.</summary>
		public TileTypes TileType   = TileTypes.Isometric;
		/// <summary>Isometric tile size.</summary>
		public float     TileSize   = 32.0f;
		/// <summary>Start sorting depth value.</summary>
		public float     StartDepth = 0.0f;
		/// <summary>Step sorting depth value.</summary>
		public float     StepDepth  = 0.1f;

		class ObjectInfo {
			public IsoObject IsoObject;
			public bool      Visited;
			public int       BeginDepend;
			public int       EndDepend;
			public ObjectInfo(IsoObject obj) {
				IsoObject = obj;
			}
			public void Reset(int first_depend) {
				Visited     = false;
				BeginDepend = first_depend;
				EndDepend   = first_depend;
			}
		}

		bool      _dirty        = true;
		float     _lastTileSize = 0.0f;
		TileTypes _lastTileType = TileTypes.Isometric;
		
		// ------------------------------------------------------------------------
		/// <summary>
		/// Marks world for resorting.
		/// </summary>
		// ------------------------------------------------------------------------
		public void MarkDirty() {
			_dirty = true;
		}
		
		// ------------------------------------------------------------------------
		/// <summary>
		/// Convert isometric coordinates to screen coordinates
		/// </summary>
		/// <returns>Screen coordinates</returns>
		/// <param name="pos">Isometric coordinates.</param>
		// ------------------------------------------------------------------------
		public Vector2 IsoToScreen(Vector3 pos) {
			switch ( TileType ) {
			case TileTypes.Isometric:
				return new Vector2(
					(pos.x - pos.y),
					(pos.x + pos.y) * 0.5f + pos.z) * TileSize;
			case TileTypes.UpDown:
				return new Vector2(
					pos.x,
					pos.y + pos.z) * TileSize;
			default:
				throw new UnityException("IsoWorld. Type is wrong!");
			}
		}
		
		// ------------------------------------------------------------------------
		/// <summary>
		/// Convert screen coordinates to isometric coordinates
		/// </summary>
		/// <returns>Isometric coordinates</returns>
		/// <param name="pos">Screen coordinates.</param>
		// ------------------------------------------------------------------------
		public Vector3 ScreenToIso(Vector2 pos) {
			switch ( TileType ) {
			case TileTypes.Isometric:
				return new Vector3(
					(pos.x * 0.5f + pos.y),
					(pos.y - pos.x * 0.5f),
					0.0f) / TileSize;
			case TileTypes.UpDown:
				return new Vector3(
					pos.x,
					pos.y,
					0.0f) / TileSize;
			default:
				throw new UnityException("IsoWorld. Type is wrong!");
			}
		}
		
		// ------------------------------------------------------------------------
		/// <summary>
		/// Convert screen coordinates to isometric coordinates with specified isometric height
		/// </summary>
		/// <returns>Isometric coordinates</returns>
		/// <param name="pos">Screen coordinates.</param>
		/// <param name="iso_z">Point isometric height.</param>
		// ------------------------------------------------------------------------
		public Vector3 ScreenToIso(Vector2 pos, float iso_z) {
			switch ( TileType ) {
			case TileTypes.Isometric: {
					var iso_pos = ScreenToIso(new Vector2(pos.x, pos.y - iso_z * TileSize));
					iso_pos.z = iso_z;
					return iso_pos;
				}
			case TileTypes.UpDown: {
					var iso_pos = ScreenToIso(new Vector2(pos.x, pos.y - iso_z * TileSize));
					iso_pos.z = iso_z;
					return iso_pos;
				}
			default:
				throw new UnityException("IsoWorld. Type is wrong!");
			}
		}
		
		void _fixAllTransforms() {
			var objects = _scanObjects(false);
			foreach ( var obj in objects ) {
				obj.IsoObject.FixTransform();
			}
		}

		void _fixTileSize() {
			_fixAllTransforms();
			_lastTileSize = TileSize;
		}
		
		void _fixTileType() {
			_fixAllTransforms();
			_lastTileType = TileType;
		}

		void _fixDirty() {
			_manualSort();
			_dirty = false;
		}

		void _fixDisable() {
			var objects = _scanObjects(false);
			foreach ( var obj in objects ) {
				obj.IsoObject.ResetIsoWorld();
			}
		}
		
		IList<ObjectInfo> _scanObjects(bool onlySorting) {
			var iso_objects = GameObject.FindObjectsOfType<IsoObject>();
			var objects = new List<ObjectInfo>(iso_objects.Length);
			foreach ( var iso_object in iso_objects ) {
				if ( !onlySorting || iso_object.Sorting ) {
					var info = new ObjectInfo(iso_object);
					objects.Add(info);
				}
			}
			return objects;
		}

		IList<int> _scanDepends(IList<ObjectInfo> objects) {
			var depends = new List<int>(objects.Count);
			foreach ( var obj_a in objects ) {
				obj_a.Reset(depends.Count);
				var obj_ao = obj_a.IsoObject;
				var max_ax = obj_ao.Position.x + obj_ao.Size.x;
				var max_ay = obj_ao.Position.y + obj_ao.Size.y;
				for ( int i = 0; i < objects.Count; ++i ) {
					var obj_bo = objects[i].IsoObject;
					if ( obj_ao != obj_bo ) {
						if ( obj_bo.Position.x < max_ax && obj_bo.Position.y < max_ay ) {
							var max_bz = obj_bo.Position.z + obj_bo.Size.z;
							if ( obj_ao.Position.z < max_bz ) {
								depends.Add(i);
								++obj_a.EndDepend;
							}
						}
					}
				}
			}
			return depends;
		}

		void _manualSort() {
			var objects = _scanObjects(true);
			var depends = _scanDepends(objects);
			var depth = StartDepth;
			foreach ( var info in objects ) {
				_placeObject(info, objects, depends, ref depth);
			}
		}

		void _placeObject(IsoObject obj, float depth) {
			var pos = obj.gameObject.transform.position;
			obj.gameObject.transform.position = new Vector3(pos.x, pos.y, depth);
		}

		void _placeObject(ObjectInfo info, IList<ObjectInfo> objects, IList<int> depends, ref float depth) {
			if ( !info.Visited ) {
				info.Visited = true;
				for ( int i = info.BeginDepend; i < info.EndDepend && i < depends.Count; ++i ) {
					var object_index = depends[i];
					var obj = objects[object_index];
					_placeObject(obj, objects, depends, ref depth);
				}
				_placeObject(info.IsoObject, depth);
				depth += StepDepth;
			}
		}

		void Start() {
			_fixTileSize();
			_fixTileType();
			_fixDirty();
		}

		void LateUpdate() {
			if ( _lastTileSize != TileSize ) {
				_fixTileSize();
			}
			if ( _lastTileType != TileType ) {
				_fixTileType();
			}
			if ( _dirty ) {
				_fixDirty();
			}
		}

		void OnDisable() {
			_fixDisable();
		}
	}
} // namespace IsoTools