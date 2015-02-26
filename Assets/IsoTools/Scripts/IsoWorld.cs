using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

namespace IsoTools {
	[ExecuteInEditMode]
	public class IsoWorld : MonoBehaviour {
		
		/// <summary>World tile types.</summary>
		public enum TileTypes {
			Isometric,
			UpDown
		}

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
		TileTypes _lastTileType = TileTypes.Isometric;
		float     _lastTileSize = 0.0f;
		float     _lastMinDepth = 0.0f;
		float     _lastMaxDepth = 0.0f;

		[SerializeField]
		public TileTypes _tileType = TileTypes.Isometric;
		/// <summary>World tile type.</summary>
		public TileTypes TileType {
			get { return _tileType; }
			set {
				_tileType = value;
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _tileSize = 32.0f;
		/// <summary>Isometric tile size.</summary>
		public float TileSize {
			get { return _tileSize; }
			set {
				_tileSize = value;
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _minDepth = 0.0f;
		/// <summary>Min sorting depth value.</summary>
		public float MinDepth {
			get { return _minDepth; }
			set {
				_minDepth = value;
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _maxDepth = 100.0f;
		/// <summary>Max sorting depth value.</summary>
		public float MaxDepth {
			get { return _maxDepth; }
			set {
				_maxDepth = value;
				ChangeSortingProperty();
			}
		}
		
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
		/// Marks world for resorting one object only
		/// </summary>
		/// <param name="obj">Isometric object for resorting.</param>
		// ------------------------------------------------------------------------ 
		public void MarkDirty(IsoObject obj) {
			if ( obj && obj.Sorting ) {
				_dirty = true;
			}
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
				throw new UnityException("IsoWorld. TileType is wrong!");
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
				throw new UnityException("IsoWorld. TileType is wrong!");
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
				throw new UnityException("IsoWorld. TileType is wrong!");
			}
		}
		
		void FixAllTransforms() {
			var objects = ScanObjects();
			foreach ( var obj in objects ) {
				obj.IsoObject.FixTransform();
			}
		}

		void ChangeSortingProperty() {
			MarkDirty();
			FixAllTransforms();
			_lastTileType = TileType;
			_lastTileSize = TileSize;
			_lastMinDepth = MinDepth;
			_lastMaxDepth = MaxDepth;
		}

		bool IsDepends(IsoObject obj_ao, IsoObject obj_bo) {
			if ( obj_ao != obj_bo ) {
				var max_ax = obj_ao.Position.x + obj_ao.Size.x;
				var max_ay = obj_ao.Position.y + obj_ao.Size.y;
				if ( obj_bo.Position.x < max_ax && obj_bo.Position.y < max_ay ) {
					var max_bz = obj_bo.Position.z + obj_bo.Size.z;
					if ( obj_ao.Position.z < max_bz ) {
						return true;
					}
				}
			}
			return false;
		}
		
		IList<ObjectInfo> ScanObjects() {
			return ScanObjects(p => true);
		}

		IList<ObjectInfo> ScanObjects(Func<IsoObject, bool> filter) {
			var iso_objects = GameObject.FindObjectsOfType<IsoObject>();
			var objects = new List<ObjectInfo>(iso_objects.Length);
			foreach ( var iso_object in iso_objects ) {
				if ( filter(iso_object) ) {
					objects.Add(new ObjectInfo(iso_object));
				}
			}
			return objects;
		}

		IList<int> ScanDepends(IList<ObjectInfo> objects) {
			var depends = new List<int>(objects.Count);
			foreach ( var obj_a in objects ) {
				obj_a.Reset(depends.Count);
				var obj_ao = obj_a.IsoObject;
				for ( int i = 0; i < objects.Count; ++i ) {
					var obj_bo = objects[i].IsoObject;
					if ( IsDepends(obj_ao, obj_bo) ) {
						depends.Add(i);
						++obj_a.EndDepend;
					}
				}
			}
			return depends;
		}

		void ManualSort(IList<ObjectInfo> objects) {
			var depends = ScanDepends(objects);
			var depth = MinDepth;
			foreach ( var info in objects ) {
				PlaceObject(info, objects, depends, ref depth);
			}
		}

		void PlaceObject(IsoObject obj, float depth) {
			var trans = obj.gameObject.transform;
			trans.position = new Vector3(trans.position.x, trans.position.y, depth);
		}

		void PlaceObject(ObjectInfo info, IList<ObjectInfo> objects, IList<int> depends, ref float depth) {
			if ( !info.Visited ) {
				info.Visited = true;
				for ( int i = info.BeginDepend; i < info.EndDepend && i < depends.Count; ++i ) {
					var object_index = depends[i];
					var obj = objects[object_index];
					PlaceObject(obj, objects, depends, ref depth);
				}
				PlaceObject(info.IsoObject, depth);
				depth += (MaxDepth - MinDepth) / objects.Count;
			}
		}

		void StepSort() {
			if ( _dirty ) {
				ManualSort(ScanObjects(p => p.Sorting));
				_dirty = false;
			}
		}

		void Start() {
			ChangeSortingProperty();
			StepSort();
		}

		void LateUpdate() {
			if ( Application.isEditor ) {
				if ( _lastTileType != _tileType )                     TileType = _tileType;
				if ( !Mathf.Approximately(_lastTileSize, _tileSize) ) TileSize = _tileSize;
				if ( !Mathf.Approximately(_lastMinDepth, _minDepth) ) MinDepth = _minDepth;
				if ( !Mathf.Approximately(_lastMaxDepth, _maxDepth) ) MaxDepth = _maxDepth;
			}
			StepSort();
		}

		void OnEnable() {
			MarkDirty();
		}

		void OnDisable() {
			var objects = ScanObjects();
			foreach ( var obj in objects ) {
				obj.IsoObject.ResetIsoWorld();
			}
		}
	}
} // namespace IsoTools